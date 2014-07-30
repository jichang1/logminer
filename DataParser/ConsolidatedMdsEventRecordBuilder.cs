Enter file contents here// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedMdsEventRecordBuilder.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.Hackathon.DataParser
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Builder class for ConsolidatedMdsEventRecords.
    /// </summary>
    internal class ConsolidatedMdsEventRecordBuilder
    {
        /// <summary>
        /// Defines the lowest MDS severity level considered for consolidation.
        /// </summary>
        /// <remarks>
        /// The highest possible MDS severity level is 1 (Critical) and the lowest possible is 5 (Verbose).  This class
        /// ignores severity levels 4 (Informational) and 5 (Verbose).
        /// </remarks>
        public const int LowestAcceptableSeverityLevel = 3;

        /// <summary>
        /// Defines the default MaxEventRecordTimespan.
        /// </summary>
        public static readonly TimeSpan DefaultMaxEventRecordTimespan = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Initializes a new instance of the ConsolidatedMdsEventRecordBuilder class with the default maximum event
        /// record timespan.
        /// </summary>
        public ConsolidatedMdsEventRecordBuilder()
            : this(DefaultMaxEventRecordTimespan)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ConsolidatedMdsEventRecordBuilder class.
        /// </summary>
        /// <param name="maxEventRecordTimespan">The maximum timespan that can elapse for a set of events from the same source to be considered related.</param>
        public ConsolidatedMdsEventRecordBuilder(TimeSpan maxEventRecordTimespan)
        {
            this.MaxEventRecordTimespan = maxEventRecordTimespan;
            this.ConsolidationThresholdTimespan = TimeSpan.FromMilliseconds((int)(maxEventRecordTimespan.TotalMilliseconds * 3));
            this.FirstTimestamp = DateTime.MinValue;
            this.LastTimestamp = DateTime.MinValue;
            this.Map = new Dictionary<int, List<ConsolidatedMdsEventRecord>>();
        }

        /// <summary>
        /// Gets the maximum timespan that can elapse for a set of events from the same source to be considered related.
        /// </summary>
        public TimeSpan MaxEventRecordTimespan { get; private set; }

        /// <summary>
        /// Gets or sets the consolidation threshold timespan; consolidation is deferred if all records occurred within
        /// this timespan unless the force flag is specified.
        /// </summary>
        private TimeSpan ConsolidationThresholdTimespan { get; set; }

        /// <summary>
        /// Gets or sets the timestamp for the first consolidated record.
        /// </summary>
        private DateTime FirstTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the timestamp for the last consolidated record.
        /// </summary>
        private DateTime LastTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the map of event records.
        /// </summary>
        private Dictionary<int, List<ConsolidatedMdsEventRecord>> Map { get; set; }

        /// <summary>
        /// Adds the MDS event data to the builder.
        /// </summary>
        /// <param name="mdsEvent">The event to add.</param>
        public void AddEvent(MinimumMdsEvent mdsEvent)
        {
            if (mdsEvent == null)
            {
                throw new ArgumentNullException("mdsEvent");
            }

            if (mdsEvent.Level > LowestAcceptableSeverityLevel || mdsEvent.Level < 1)
            {
                // Ignore this event, it is either insignificant or invalid.
                return;
            }

            List<ConsolidatedMdsEventRecord> relatedRecords;
            var newRecord = new ConsolidatedMdsEventRecord(mdsEvent.ProcessId, mdsEvent.RoleInstance, mdsEvent.PreciseTimeStamp, mdsEvent.EventId);
            var hashCode = newRecord.GetHashCode();

            if (this.Map.TryGetValue(hashCode, out relatedRecords))
            {
                var lastRecord = relatedRecords.Last();
                if (newRecord.Timestamp - lastRecord.Timestamp > this.MaxEventRecordTimespan)
                {
                    relatedRecords.Add(newRecord);
                }
                else
                {
                    lastRecord.AddEvent(mdsEvent.EventId);
                }
            }
            else
            {
                relatedRecords = new List<ConsolidatedMdsEventRecord> { newRecord };
                this.Map.Add(hashCode, relatedRecords);

                if (this.FirstTimestamp == DateTime.MinValue)
                {
                    this.FirstTimestamp = newRecord.Timestamp;
                }
            }

            this.LastTimestamp = mdsEvent.PreciseTimeStamp;
        }

        /// <summary>
        /// Writes all the consolidated records to the file.
        /// </summary>
        /// <remarks>
        /// This method writes all the records that have a timestamp MaxEventRecordTimespan older than the last timestamp
        /// and prunes them from the map.  However, if the force flag is specified then all records are written regardless
        /// of their age.
        /// </remarks>
        /// <param name="path">The path to the consolidated records file.</param>
        /// <param name="force">True to force the consolidation of all records; false otherwise.</param>
        public void Consolidate(string path, bool force = false)
        {
            if (!force && this.LastTimestamp - this.FirstTimestamp < this.ConsolidationThresholdTimespan)
            {
                return;
            }

            using (var writer = new StreamWriter(path, true, Encoding.Default))
            {
                writer.AutoFlush = true;

                var values = this.Map.Values;
                this.Map = new Dictionary<int, List<ConsolidatedMdsEventRecord>>();

                foreach (var relatedRecords in values)
                {
                    var unconsolidatedRecords = new List<ConsolidatedMdsEventRecord>();

                    foreach (var record in relatedRecords)
                    {
                        if (force || this.LastTimestamp - record.Timestamp > this.MaxEventRecordTimespan)
                        {
                            writer.WriteLine(record.ToString());
                        }
                        else
                        {
                            // There's probably a more efficient method for pruning the values using AddRange, but I'm too tired to think of it right now. -- miroman
                            unconsolidatedRecords.Add(record);
                        }
                    }

                    if (unconsolidatedRecords.Count != 0)
                    {
                        // Again, probably not super efficient here. -- miroman
                        this.Map.Add(unconsolidatedRecords.First().GetHashCode(), unconsolidatedRecords);
                    }
                }
            }
        }
    }
}
