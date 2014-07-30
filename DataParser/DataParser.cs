// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataParser.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.Hackathon.DataParser
{
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Parse data tables to create event rows.
    /// </summary>
    public class DataParser
    {
        /// <summary>
        /// Defines the supported MDS schemas.
        /// </summary>
        public enum MdsSchema
        {
            /// <summary>
            /// Use the OaaS MDS schema.
            /// </summary>
            Oaas,

            /// <summary>
            /// Use the CloudML MDS schema.
            /// </summary>
            Ml
        }

        /// <summary>
        /// Main entry point for DataParser.
        /// </summary>
        /// <remarks>
        /// The current expected CSV input is not ideally formatted.  This is because the default MDS export schema places
        /// useful fixed and easily parsable data like the ActivityId column after things like the EventMessage and/or
        /// the Message columns, which can contain free-text, such as commas.  This makes a simple parsing operation
        /// unnecessarily difficult.  There is also other data columns that are of no use.
        /// <p />
        /// This problem could be eliminated by using a more specific SELECT statement to re-order the columns.  It
        /// would also be useful to format the PreciseTimeStamp using something with more precision.  The best .NET
        /// format is the Round-trip ("O", "o") format specifier and second best is the RFC1123 ("R", "r") format
        /// specifier.
        /// <p />
        /// TODO: Re-implement as a PowerShell cmdlet or better yet, an Activity.
        /// </remarks>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException("TODO: Add usage help.");
            }

            MdsSchema schema;
            if (!Enum.TryParse(args[0], true, out schema))
            {
                throw new ArgumentException("TODO: Add usage help.", "schema");
            }

            var path = args[1];
            var destination = args[2];

            var directoryInfo = new DirectoryInfo(Path.GetDirectoryName(path) ?? string.Empty);
            var searchPattern = Path.GetFileName(path) ?? string.Empty;
            var files = directoryInfo.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);

            if (files.Length <= 0)
            {
                throw new InvalidOperationException(string.Format("No files found for path: {0}", path));
            }

            var eventBuilder = new ConsolidatedMdsEventRecordBuilder();

            var count = 0;
            foreach (var file in files.OrderBy(f => f.Name))
            {
                System.Diagnostics.Trace.TraceInformation("Processing {0}...", file.Name);
                MdsParser.ParseFile(file, eventBuilder, schema);

                eventBuilder.Consolidate(destination);
            }

            eventBuilder.Consolidate(destination, force: true);
        }
    }
}
