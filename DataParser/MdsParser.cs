// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MdsParser.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.Hackathon.DataParser
{
    using System;
    using System.Globalization;
    using System.IO;

    /// <summary>
    /// Parser for MDS table data exported in CSV format.
    /// </summary>
    internal class MdsParser
    {
        /// <summary>
        /// Defines the minimum number of expected MDS data columns in a single event row.
        /// </summary>
        public const int MinimumMdsColumnCount = 10;

        /// <summary>
        /// Parse an entire file containing MDS table data exported in CSV format.
        /// </summary>
        /// <param name="fileInfo">The file to parse.</param>
        /// <param name="eventBuilder">The event record builder.</param>
        /// <param name="schema">The MDS schema to use.</param>
        public static void ParseFile(FileInfo fileInfo, ConsolidatedMdsEventRecordBuilder eventBuilder, DataParser.MdsSchema schema)
        {
            if (fileInfo == null)
            {
                throw new ArgumentNullException("fileInfo");
            }

            if (!fileInfo.Exists)
            {
                throw new ArgumentException(string.Format("The file does not exist: {0}", fileInfo.FullName), "fileInfo");
            }

            using (var streamReader = fileInfo.OpenText())
            {
                string csvText;
                while ((csvText = streamReader.ReadLine()) != null)
                {
                    try
                    {
                        MinimumMdsEvent mdsEvent;
                        if (schema == DataParser.MdsSchema.Oaas)
                        {
                            mdsEvent = ParseOaasLine(csvText);
                        }
                        else
                        {
                            mdsEvent = ParseMlLine(csvText);
                        }

                        eventBuilder.AddEvent(mdsEvent);
                    }
                    catch (InvalidOperationException exception)
                    {
                        System.Diagnostics.Trace.TraceWarning("{0} [csvText={1}]", exception.Message, csvText);
                    }
                }
            }
        }

        /// <summary>
        /// Parse a line of text taken from OaaS MDS table data exported in CSV format.
        /// </summary>
        /// <remarks>
        /// This parser assumes that the first X rows are as follows:
        ///     <c>TIMESTAMP,PreciseTimeStamp,Tenant,Role,RoleInstance,Level,ProviderGuid,ProviderName,EventId,Pid</c>
        /// </remarks>
        /// <param name="csvText">A line of text in CSV format.</param>
        /// <returns>
        /// The parsed minimum MDS event data.
        /// </returns>
        public static MinimumMdsEvent ParseOaasLine(string csvText)
        {
            var csvSeparators = new[] { ',' };
            var splits = csvText.Split(csvSeparators);

            if (splits.Length < MinimumMdsColumnCount)
            {
                throw new InvalidOperationException(string.Format("Expected at least {0} columns.", MinimumMdsColumnCount));
            }

            // column 0 is expected to be a parsable date-time format.
            DateTime timestamp;
            var columnText = splits[0];
            if (!DateTime.TryParse(columnText, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeLocal, out timestamp))
            {
                throw new InvalidOperationException(string.Format("Failed to parse the value in the {0} column. [columnText={1}][expectedType={2}", "TIMESTAMP", columnText, typeof(DateTime).FullName));
            }

            // column 1 is expected to be a parsable date-time format.
            DateTime preciseTimeStamp;
            columnText = splits[1];
            if (!DateTime.TryParse(columnText, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeLocal, out preciseTimeStamp))
            {
                throw new InvalidOperationException(string.Format("Failed to parse the value in the {0} column. [columnText={1}][expectedType={2}", "PreciseTimeStamp", columnText, typeof(DateTime).FullName));
            }

            // column 2 is expected to be a parsable Guid format.
            Guid tenant;
            columnText = splits[2];
            if (!Guid.TryParse(columnText, out tenant))
            {
                throw new InvalidOperationException(string.Format("Failed to parse the value in the {0} column. [columnText={1}][expectedType={2}", "Tenant", columnText, typeof(Guid).FullName));
            }

            // column 3 is string data, requires no parsing and is ignored (Role).

            // column 4 is string data, so it requires no parsing.
            var roleInstance = splits[4];

            // column 5 is expected to be a parsable int format.
            int level;
            columnText = splits[5];
            if (!int.TryParse(columnText, out level))
            {
                throw new InvalidOperationException(string.Format("Failed to parse the value in the {0} column. [columnText={1}][expectedType={2}", "Level", columnText, typeof(int).FullName));
            }

            // column 6 is expected to be a parsable Guid format.
            Guid providerGuid;
            columnText = splits[6];
            if (!Guid.TryParse(columnText, out providerGuid))
            {
                throw new InvalidOperationException(string.Format("Failed to parse the value in the {0} column. [columnText={1}][expectedType={2}", "ProviderGuid", columnText, typeof(Guid).FullName));
            }

            // column 7 is string data, requires no parsing and is ignored (ProviderName).

            // column 8 is expected to be a parsable int format.
            int eventId;
            columnText = splits[8];
            if (!int.TryParse(columnText, out eventId))
            {
                throw new InvalidOperationException(string.Format("Failed to parse the value in the {0} column. [columnText={1}][expectedType={2}", "EventId", columnText, typeof(int).FullName));
            }

            // column 9 is expected to be a parsable int format.
            int processId;
            columnText = splits[9];
            if (!int.TryParse(columnText, out processId))
            {
                throw new InvalidOperationException(string.Format("Failed to parse the value in the {0} column. [columnText={1}][expectedType={2}", "Pid", columnText, typeof(int).FullName));
            }

            return new MinimumMdsEvent
                       {
                           EventId = eventId,
                           Level = level,
                           PreciseTimeStamp = preciseTimeStamp,
                           ProcessId = processId,
                           RoleInstance = roleInstance
                       };
        }

        /// <summary>
        /// Parse a line of text taken from ML MDS table data exported in CSV format.
        /// </summary>
        /// <remarks>
        /// This parser assumes that the first X rows are as follows:
        ///     <c>TIMESTAMP,PreciseTimeStamp,Tenant,Role,RoleInstance,Level,ProviderGuid,ProviderName,EventId,Pid</c>
        /// </remarks>
        /// <param name="csvText">A line of text in CSV format.</param>
        /// <returns>
        /// The parsed minimum MDS event data.
        /// </returns>
        public static MinimumMdsEvent ParseMlLine(string csvText)
        {
            var csvSeparators = new[] { ',' };
            var splits = csvText.Split(csvSeparators);

            if (splits.Length < MinimumMdsColumnCount - 1)
            {
                throw new InvalidOperationException(string.Format("Expected at least {0} columns.", MinimumMdsColumnCount));
            }

            // column 0 is expected to be a parsable date-time format.
            DateTime timestamp;
            var columnText = splits[0];
            if (!DateTime.TryParse(columnText, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeLocal, out timestamp))
            {
                throw new InvalidOperationException(string.Format("Failed to parse the value in the {0} column. [columnText={1}][expectedType={2}", "TIMESTAMP", columnText, typeof(DateTime).FullName));
            }

            // column 1 is expected to be a parsable date-time format.
            DateTime preciseTimeStamp;
            columnText = splits[1];
            if (!DateTime.TryParse(columnText, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeLocal, out preciseTimeStamp))
            {
                throw new InvalidOperationException(string.Format("Failed to parse the value in the {0} column. [columnText={1}][expectedType={2}", "PreciseTimeStamp", columnText, typeof(DateTime).FullName));
            }

            // column 2 is expected to be a parsable Guid format.
            Guid tenant;
            columnText = splits[2];
            if (!Guid.TryParse(columnText, out tenant))
            {
                throw new InvalidOperationException(string.Format("Failed to parse the value in the {0} column. [columnText={1}][expectedType={2}", "Tenant", columnText, typeof(Guid).FullName));
            }

            // column 3 is string data, requires no parsing and is ignored (Role).

            // column 4 is string data, so it requires no parsing.
            var roleInstance = splits[4];

            // column 5 is expected to be a parsable int format.
            int level;
            columnText = splits[5];
            if (!int.TryParse(columnText, out level))
            {
                throw new InvalidOperationException(string.Format("Failed to parse the value in the {0} column. [columnText={1}][expectedType={2}", "Level", columnText, typeof(int).FullName));
            }

            // column 6 is expected to be a parsable Guid format.
            Guid providerGuid;
            columnText = splits[6];
            if (!Guid.TryParse(columnText, out providerGuid))
            {
                throw new InvalidOperationException(string.Format("Failed to parse the value in the {0} column. [columnText={1}][expectedType={2}", "ProviderGuid", columnText, typeof(Guid).FullName));
            }

            // column 7 is expected to be a parsable int format.
            int eventId;
            columnText = splits[7];
            if (!int.TryParse(columnText, out eventId))
            {
                throw new InvalidOperationException(string.Format("Failed to parse the value in the {0} column. [columnText={1}][expectedType={2}", "EventId", columnText, typeof(int).FullName));
            }

            // column 8 is expected to be a parsable int format.
            int processId;
            columnText = splits[8];
            if (!int.TryParse(columnText, out processId))
            {
                throw new InvalidOperationException(string.Format("Failed to parse the value in the {0} column. [columnText={1}][expectedType={2}", "Pid", columnText, typeof(int).FullName));
            }

            return new MinimumMdsEvent
            {
                EventId = eventId,
                Level = level,
                PreciseTimeStamp = preciseTimeStamp,
                ProcessId = processId,
                RoleInstance = roleInstance
            };
        }
    }
}
