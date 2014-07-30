// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MinimumMdsEvent.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.Hackathon.DataParser
{
    using System;

    /// <summary>
    /// Data class representing the minimum MDS event data required to create event records.
    /// </summary>
    internal class MinimumMdsEvent
    {
        /// <summary>
        /// Gets or sets the EventId.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the event level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the precise timestamp for the event.
        /// </summary>
        public DateTime PreciseTimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the ProcessId.
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// Gets or sets the role instance.
        /// </summary>
        public string RoleInstance { get; set; }
    }
}
