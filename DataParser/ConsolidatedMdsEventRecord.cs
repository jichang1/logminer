// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedMdsEventRecord.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.Hackathon.DataParser
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Represents a series of consolidated, related MDS events.
    /// </summary>
    public class ConsolidatedMdsEventRecord
    {
        /// <summary>
        /// Initializes a new instance of the ConsolidatedMdsEventRecord class.
        /// </summary>
        /// <param name="processId">The process identifier for all the consolidated events.</param>
        /// <param name="roleInstance">The role instance for all the consolidated events.</param>
        /// <param name="timestamp">The timestamp for the initial event.</param>
        /// <param name="initialEvent">The initial event.</param>
        public ConsolidatedMdsEventRecord(int processId, string roleInstance, DateTime timestamp, int initialEvent)
        {
            // for now, ActivityId is always Guid.Empty
            this.ActivityId = Guid.Empty;

            this.ProcessId = processId;
            this.RoleInstance = roleInstance;
            this.Timestamp = timestamp;

            this.Events = new Queue<int>();
            this.AddEvent(initialEvent);
        }

        /// <summary>
        /// Gets the ActivityId.
        /// </summary>
        /// <remarks>
        /// This is currently always Guid.Empty.
        /// </remarks>
        public Guid ActivityId { get; private set; }

        /// <summary>
        /// Gets the process identifier.
        /// </summary>
        public int ProcessId { get; private set; }

        /// <summary>
        /// Gets the role instance.
        /// </summary>
        public string RoleInstance { get; private set; }

        /// <summary>
        /// Gets the timestamp for the first event in the record.
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// Gets or sets the ordered collection of events.
        /// </summary>
        private Queue<int> Events { get; set; } 

        /// <summary>
        /// Adds another event to the list.
        /// </summary>
        /// <param name="eventId">The event to add.</param>
        public void AddEvent(int eventId)
        {
            this.Events.Enqueue(eventId);
        }

        /// <summary>
        /// Returns a string representation of the Consolidated event data in CSV format.
        /// </summary>
        /// <returns>
        /// A string representation of the Consolidated event data in CSV format.
        /// </returns>
        public override string ToString()
        {
            var events = this.Events.Select(e => e.ToString(CultureInfo.InvariantCulture)).ToArray();

            return string.Format(
                "{0},{1},{2:O},{3},{4}",
                this.ProcessId,
                this.RoleInstance,
                this.Timestamp,
                events.Length,
                string.Join(",", events));
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object. </param>
        /// <returns>
        /// True if the specified object is equal to the current object; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((ConsolidatedMdsEventRecord)obj);
        }

        /// <summary>
        /// Gets a numeric value that is used to insert and identify an object in a hash-based collection such as the
        /// Dictionary.
        /// </summary>
        /// <returns>The hash code for this ConsolidatedMdsEventRecord instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.ProcessId;
                hashCode = (hashCode * 397) ^ (this.RoleInstance != null ? this.RoleInstance.GetHashCode() : 0);

                return hashCode;
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <remarks>
        /// This implementation leaves something to be desired -- namely the events comparison.
        /// </remarks>
        /// <param name="other">The object to compare with the current object. </param>
        /// <returns>
        /// True if the specified object is equal to the current object; otherwise, false.
        /// </returns>
        protected bool Equals(ConsolidatedMdsEventRecord other)
        {
            return this.ProcessId == other.ProcessId
                && string.Equals(this.RoleInstance, other.RoleInstance)
                && string.Equals(this.Timestamp, other.Timestamp)
                && this.Events.Equals(other.Events);
        }
    }
}
