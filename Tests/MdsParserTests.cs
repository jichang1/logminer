// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MdsParserTests.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.Hackathon.Tests
{
    using System;
    using System.Globalization;
    using System.Linq;

    using Microsoft.Hackathon.DataParser;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for MdsParser.
    /// </summary>
    [TestClass]
    public class MdsParserTests
    {
        /// <summary>
        /// As a user I expect I can parse a simple line of CSV text, i.e. a line with no embedded commas in either
        /// the EventMessage or Message columns.
        /// </summary>
        [TestMethod]
        public void CanParseSimpleCsvText()
        {
            var expectedPreciseTimeStamp = DateTime.Now;
            var expectedTenant = Guid.NewGuid();
            const string ExpectedRole = "RunbookWorker.Cloud";
            const string ExpectedRoleInstance = "RunbookWorker.Cloud_IN_13";
            const int ExpectedLevel = 4;
            var expectedProviderGuid = Guid.NewGuid();
            const string ExpectedProviderName = "Provider-Name";
            const int ExpectedEventId = 3408;
            const int ExpectedProcessId = 1234;
            const int ExpectedThreadId = 567;
            const string ExpectedOpcodeName  = "OpcodeName";
            const string ExpectedKeywordName = "Session3;Session2;Session1;Session0";
            const string ExpectedTaskName = "DebugInformational";
            const string ExpectedChannelName = "Microsoft-SMA/Debug";
            const string ExpectedEventMessage = "%1";
            var expectedActivityId = Guid.NewGuid();
            const string ExpectedMessage = @"param1=""text""";
            const string ExpectedPartitionKey = "0000000000000000002___0635391900000000000";
            const string ExpectedRowKey = "3f4d4ee862d5482b92a46c7b3d55c889___Orchestrator.WebService.Cloud___Orchestrator.WebService.Cloud_IN_0___0000000004295156001";
            const string ExpectedN = "0000000000000000002";
            const string ExpectedRowIndex = "0000000004295156001";
            const string ExpectedSourceTableName = "OaaSProdEtwAllVer2v0";

            var expectedOaasMdsEvent = new OaasMdsEvent
                                   {
                                       TimeStamp = expectedPreciseTimeStamp,
                                       PreciseTimeStamp = expectedPreciseTimeStamp,
                                       Tenant = expectedTenant,
                                       Role = ExpectedRole,
                                       RoleInstance = ExpectedRoleInstance,
                                       Level = ExpectedLevel,
                                       ProviderGuid = expectedProviderGuid,
                                       ProviderName = ExpectedProviderName,
                                       EventId = ExpectedEventId,
                                       ProcessId = ExpectedProcessId,
                                       ThreadId = ExpectedThreadId,
                                       OpcodeName = ExpectedOpcodeName,
                                       KeywordName = ExpectedKeywordName,
                                       TaskName = ExpectedTaskName,
                                       ChannelName = ExpectedChannelName,
                                       EventMessage = ExpectedEventMessage,
                                       ActivityId = expectedActivityId,
                                       Message = ExpectedMessage,
                                       PartitionKey = ExpectedPartitionKey,
                                       RowKey = ExpectedRowKey,
                                       N = ExpectedN,
                                       RowIndex = ExpectedRowIndex,
                                       SourceTableName = ExpectedSourceTableName
                                   };

            var actualMinimumMdsEvent = MdsParser.ParseOaasLine(expectedOaasMdsEvent.ToString());

            AssertOaasMdsEventsAreEqual(expectedOaasMdsEvent, actualMinimumMdsEvent);
        }

        /// <summary>
        /// As a user I expect I can parse a line of CSV text with commas embedded in the EventMessage column.
        /// </summary>
        [TestMethod]
        public void CanParseCsvTextWithCommasInEventMessage()
        {
            var expectedPreciseTimeStamp = DateTime.Now;
            var expectedTenant = Guid.NewGuid();
            const string ExpectedRole = "RunbookWorker.Cloud";
            const string ExpectedRoleInstance = "RunbookWorker.Cloud_IN_13";
            const int ExpectedLevel = 4;
            var expectedProviderGuid = Guid.NewGuid();
            const string ExpectedProviderName = "Provider-Name";
            const int ExpectedEventId = 3408;
            const int ExpectedProcessId = 1234;
            const int ExpectedThreadId = 567;
            const string ExpectedOpcodeName = "OpcodeName";
            const string ExpectedKeywordName = "Session3;Session2;Session1;Session0";
            const string ExpectedTaskName = "DebugInformational";
            const string ExpectedChannelName = "Microsoft-SMA/Debug";
            const string ExpectedEventMessage = "%1,%2";
            var expectedActivityId = Guid.NewGuid();
            const string ExpectedMessage = @"param1=""text1"" param2=""text2""";
            const string ExpectedPartitionKey = "0000000000000000002___0635391900000000000";
            const string ExpectedRowKey = "3f4d4ee862d5482b92a46c7b3d55c889___Orchestrator.WebService.Cloud___Orchestrator.WebService.Cloud_IN_0___0000000004295156001";
            const string ExpectedN = "0000000000000000002";
            const string ExpectedRowIndex = "0000000004295156001";
            const string ExpectedSourceTableName = "OaaSProdEtwAllVer2v0";

            var expectedOaasMdsEvent = new OaasMdsEvent
            {
                TimeStamp = expectedPreciseTimeStamp,
                PreciseTimeStamp = expectedPreciseTimeStamp,
                Tenant = expectedTenant,
                Role = ExpectedRole,
                RoleInstance = ExpectedRoleInstance,
                Level = ExpectedLevel,
                ProviderGuid = expectedProviderGuid,
                ProviderName = ExpectedProviderName,
                EventId = ExpectedEventId,
                ProcessId = ExpectedProcessId,
                ThreadId = ExpectedThreadId,
                OpcodeName = ExpectedOpcodeName,
                KeywordName = ExpectedKeywordName,
                TaskName = ExpectedTaskName,
                ChannelName = ExpectedChannelName,
                EventMessage = ExpectedEventMessage,
                ActivityId = expectedActivityId,
                Message = ExpectedMessage,
                PartitionKey = ExpectedPartitionKey,
                RowKey = ExpectedRowKey,
                N = ExpectedN,
                RowIndex = ExpectedRowIndex,
                SourceTableName = ExpectedSourceTableName
            };

            var actualMinimumMdsEvent = MdsParser.ParseOaasLine(expectedOaasMdsEvent.ToString());

            AssertOaasMdsEventsAreEqual(expectedOaasMdsEvent, actualMinimumMdsEvent);
        }

        /// <summary>
        /// As a user I expect I can parse a line of CSV text with commas embedded in the Message column.
        /// </summary>
        [TestMethod]
        public void CanParseCsvTextWithCommasInMessage()
        {
            var expectedPreciseTimeStamp = DateTime.Now;
            var expectedTenant = Guid.NewGuid();
            const string ExpectedRole = "RunbookWorker.Cloud";
            const string ExpectedRoleInstance = "RunbookWorker.Cloud_IN_13";
            const int ExpectedLevel = 4;
            var expectedProviderGuid = Guid.NewGuid();
            const string ExpectedProviderName = "Provider-Name";
            const int ExpectedEventId = 3408;
            const int ExpectedProcessId = 1234;
            const int ExpectedThreadId = 567;
            const string ExpectedOpcodeName = "OpcodeName";
            const string ExpectedKeywordName = "Session3;Session2;Session1;Session0";
            const string ExpectedTaskName = "DebugInformational";
            const string ExpectedChannelName = "Microsoft-SMA/Debug";
            const string ExpectedEventMessage = "%1";
            var expectedActivityId = Guid.NewGuid();
            const string ExpectedMessage = @"param1=""1, 2, 3, 4, 5""";
            const string ExpectedPartitionKey = "0000000000000000002___0635391900000000000";
            const string ExpectedRowKey = "3f4d4ee862d5482b92a46c7b3d55c889___Orchestrator.WebService.Cloud___Orchestrator.WebService.Cloud_IN_0___0000000004295156001";
            const string ExpectedN = "0000000000000000002";
            const string ExpectedRowIndex = "0000000004295156001";
            const string ExpectedSourceTableName = "OaaSProdEtwAllVer2v0";

            var expectedOaasMdsEvent = new OaasMdsEvent
            {
                TimeStamp = expectedPreciseTimeStamp,
                PreciseTimeStamp = expectedPreciseTimeStamp,
                Tenant = expectedTenant,
                Role = ExpectedRole,
                RoleInstance = ExpectedRoleInstance,
                Level = ExpectedLevel,
                ProviderGuid = expectedProviderGuid,
                ProviderName = ExpectedProviderName,
                EventId = ExpectedEventId,
                ProcessId = ExpectedProcessId,
                ThreadId = ExpectedThreadId,
                OpcodeName = ExpectedOpcodeName,
                KeywordName = ExpectedKeywordName,
                TaskName = ExpectedTaskName,
                ChannelName = ExpectedChannelName,
                EventMessage = ExpectedEventMessage,
                ActivityId = expectedActivityId,
                Message = ExpectedMessage,
                PartitionKey = ExpectedPartitionKey,
                RowKey = ExpectedRowKey,
                N = ExpectedN,
                RowIndex = ExpectedRowIndex,
                SourceTableName = ExpectedSourceTableName
            };

            var actualMinimumMdsEvent = MdsParser.ParseOaasLine(expectedOaasMdsEvent.ToString());

            AssertOaasMdsEventsAreEqual(expectedOaasMdsEvent, actualMinimumMdsEvent);
        }

        /// <summary>
        /// As a user I expect I can parse a line of CSV text with only the minimum MDS event columns.
        /// </summary>
        [TestMethod]
        public void CanParseCsvTextWithMinimumMdsEventData()
        {
            var expectedPreciseTimeStamp = DateTime.Now;
            var expectedTenant = Guid.NewGuid();
            const string ExpectedRole = "RunbookWorker.Cloud";
            const string ExpectedRoleInstance = "RunbookWorker.Cloud_IN_13";
            const int ExpectedLevel = 4;
            var expectedProviderGuid = Guid.NewGuid();
            const string ExpectedProviderName = "Provider-Name";
            const int ExpectedEventId = 3408;
            const int ExpectedProcessId = 1234;
            const int ExpectedThreadId = 567;
            const string ExpectedOpcodeName = "OpcodeName";
            const string ExpectedKeywordName = "Session3;Session2;Session1;Session0";
            const string ExpectedTaskName = "DebugInformational";
            const string ExpectedChannelName = "Microsoft-SMA/Debug";
            const string ExpectedEventMessage = "%1";
            var expectedActivityId = Guid.NewGuid();
            const string ExpectedMessage = @"param1=""1, 2, 3, 4, 5""";
            const string ExpectedPartitionKey = "0000000000000000002___0635391900000000000";
            const string ExpectedRowKey = "3f4d4ee862d5482b92a46c7b3d55c889___Orchestrator.WebService.Cloud___Orchestrator.WebService.Cloud_IN_0___0000000004295156001";
            const string ExpectedN = "0000000000000000002";
            const string ExpectedRowIndex = "0000000004295156001";
            const string ExpectedSourceTableName = "OaaSProdEtwAllVer2v0";

            var expectedOaasMdsEvent = new OaasMdsEvent
            {
                TimeStamp = expectedPreciseTimeStamp,
                PreciseTimeStamp = expectedPreciseTimeStamp,
                Tenant = expectedTenant,
                Role = ExpectedRole,
                RoleInstance = ExpectedRoleInstance,
                Level = ExpectedLevel,
                ProviderGuid = expectedProviderGuid,
                ProviderName = ExpectedProviderName,
                EventId = ExpectedEventId,
                ProcessId = ExpectedProcessId,
                ThreadId = ExpectedThreadId,
                OpcodeName = ExpectedOpcodeName,
                KeywordName = ExpectedKeywordName,
                TaskName = ExpectedTaskName,
                ChannelName = ExpectedChannelName,
                EventMessage = ExpectedEventMessage,
                ActivityId = expectedActivityId,
                Message = ExpectedMessage,
                PartitionKey = ExpectedPartitionKey,
                RowKey = ExpectedRowKey,
                N = ExpectedN,
                RowIndex = ExpectedRowIndex,
                SourceTableName = ExpectedSourceTableName
            };

            var csvText =
                    string.Format(
                        "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
                        expectedOaasMdsEvent.TimeStamp,
                        expectedOaasMdsEvent.PreciseTimeStamp,
                        expectedOaasMdsEvent.Tenant,
                        expectedOaasMdsEvent.Role,
                        expectedOaasMdsEvent.RoleInstance,
                        expectedOaasMdsEvent.Level,
                        expectedOaasMdsEvent.ProviderGuid,
                        expectedOaasMdsEvent.ProviderName,
                        expectedOaasMdsEvent.EventId,
                        expectedOaasMdsEvent.ProcessId);

            var actualMinimumMdsEvent = MdsParser.ParseOaasLine(csvText);

            AssertOaasMdsEventsAreEqual(expectedOaasMdsEvent, actualMinimumMdsEvent);
        }

        /// <summary>
        /// As a user I expect an exception when the CSV text does not have at least the minimum number of columns.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidMinimumColumnCountThrows()
        {
            MdsParser.ParseOaasLine(string.Join(",", Enumerable.Range('1', MdsParser.MinimumMdsColumnCount - 1).Select(x => x.ToString(CultureInfo.InvariantCulture).ToArray())));
        }

        /// <summary>
        /// As a user I expect an exception when the TIMESTAMP column is not a parsable DateTime.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidTimestampColumnThrows()
        {
            const string ColumnName = "TIMESTAMP";
            const int ColumnIndex = 0;

            TryParseThrowHelper(ColumnName, ColumnIndex);
        }

        /// <summary>
        /// As a user I expect an exception when the PreciseTimeStamp column is not a parsable DateTime.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidPreciseTimeStampColumnThrows()
        {
            const string ColumnName = "PreciseTimeStamp";
            const int ColumnIndex = 1;

            TryParseThrowHelper(ColumnName, ColumnIndex);
        }

        /// <summary>
        /// As a user I expect an exception when the Tenant column is not a parsable Guid.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidTenantColumnThrows()
        {
            const string ColumnName = "Tenant";
            const int ColumnIndex = 2;

            TryParseThrowHelper(ColumnName, ColumnIndex);
        }

        /// <summary>
        /// As a user I expect an exception when the Level column is not a parsable int.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidLevelColumnThrows()
        {
            const string ColumnName = "Level";
            const int ColumnIndex = 5;

            TryParseThrowHelper(ColumnName, ColumnIndex);
        }

        /// <summary>
        /// As a user I expect an exception when the ProviderGuid column is not a parsable Guid.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidProviderGuidColumnThrows()
        {
            const string ColumnName = "ProviderGuid";
            const int ColumnIndex = 6;

            TryParseThrowHelper(ColumnName, ColumnIndex);
        }

        /// <summary>
        /// As a user I expect an exception when the EventId column is not a parsable int.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidEventIdColumnThrows()
        {
            const string ColumnName = "EventId";
            const int ColumnIndex = 8;

            TryParseThrowHelper(ColumnName, ColumnIndex);
        }

        /// <summary>
        /// As a user I expect an exception when the Pid (ProcessId) column is not a parsable int.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidProcessIdColumnThrows()
        {
            const string ColumnName = "Pid";
            const int ColumnIndex = 9;

            TryParseThrowHelper(ColumnName, ColumnIndex);
       }

        /// <summary>
        /// Asserts that the actual MinimumMdsEvent contains the values from the expected OaasMdsEvent.
        /// </summary>
        /// <param name="expected">An instance of an OaasMdsEvent containing the correct, expected properties.</param>
        /// <param name="actual">An instance of an MinimumMdsEvent to compare against the expected value.</param>
        private static void AssertOaasMdsEventsAreEqual(OaasMdsEvent expected, MinimumMdsEvent actual)
        {
            var expectedPreciseTimeStamp = expected.PreciseTimeStamp.ToUniversalTime();

            // compare timestamps for given resolution
            Assert.AreEqual(expectedPreciseTimeStamp.Year, actual.PreciseTimeStamp.Year);
            Assert.AreEqual(expectedPreciseTimeStamp.Month, actual.PreciseTimeStamp.Month);
            Assert.AreEqual(expectedPreciseTimeStamp.Day, actual.PreciseTimeStamp.Day);
            Assert.AreEqual(expectedPreciseTimeStamp.Hour, actual.PreciseTimeStamp.Hour);
            Assert.AreEqual(expectedPreciseTimeStamp.Minute, actual.PreciseTimeStamp.Minute);
            Assert.AreEqual(expectedPreciseTimeStamp.Second, actual.PreciseTimeStamp.Second);

            Assert.AreEqual(expected.ProcessId, actual.ProcessId);
            Assert.AreEqual(expected.EventId, actual.EventId);
            Assert.AreEqual(expected.Level, actual.Level);
            Assert.AreEqual(expected.RoleInstance, actual.RoleInstance);
        }

        /// <summary>
        /// Gets an array of strings representing valid values for the minimum MDS event data.
        /// </summary>
        /// <returns>
        /// An array of strings representing valid values for the minimum MDS event data.
        /// </returns>
        private static string[] GetMinimumMdsEventTestData()
        {
            var minimumMdsEvent = new MinimumMdsEvent
            {
                EventId = 1234,
                Level = 3,
                PreciseTimeStamp = DateTime.Now,
                ProcessId = 1234,
                RoleInstance = "RunbookWorker.Cloud_IN_13"
            };

            return new[]
                       {
                           minimumMdsEvent.PreciseTimeStamp.ToString(CultureInfo.InvariantCulture),
                           minimumMdsEvent.PreciseTimeStamp.ToString(CultureInfo.InvariantCulture),
                           Guid.NewGuid().ToString(),
                           "role",
                           minimumMdsEvent.RoleInstance,
                           minimumMdsEvent.Level.ToString(CultureInfo.InvariantCulture),
                           Guid.NewGuid().ToString(),
                           "provider-name",
                           minimumMdsEvent.EventId.ToString(CultureInfo.InvariantCulture),
                           minimumMdsEvent.ProcessId.ToString(CultureInfo.InvariantCulture)
                       };
        }

        /// <summary>
        /// Helper function that gets the minimum valid parsable data, invalidates the specified column, tries to parse
        /// the data and verifies the exception message contains the expected column name.
        /// </summary>
        /// <param name="columnName">The column name that is expected to appear in the exception message.</param>
        /// <param name="columnIndex">The index of the column to pollute with unparsable data.</param>
        // ReSharper disable UnusedParameter.Local
        private static void TryParseThrowHelper(string columnName, int columnIndex)
        {
            const string UnparsableValue = "UNPARSABLE-DATETIME-GUID-INT";

            var data = GetMinimumMdsEventTestData();
            data[columnIndex] = UnparsableValue;

            try
            {
                MdsParser.ParseOaasLine(string.Join(",", data));
            }
            catch (InvalidOperationException exception)
            {
                Assert.IsTrue(exception.Message.Contains(string.Format("the {0} column", columnName)));

                throw;
            }
        }
        // ReSharper restore UnusedParameter.Local

        /// <summary>
        /// Data class representing a single MDS event for the Orchestrator service.
        /// </summary>
        private class OaasMdsEvent : MinimumMdsEvent
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            // ReSharper disable MemberCanBePrivate.Local

            /// <summary>
            /// Gets or sets the MDS timestamp.
            /// </summary>
            public DateTime TimeStamp { get; set; }

            /// <summary>
            /// Gets or sets the tenant.
            /// </summary>
            public Guid Tenant { get; set; }

            /// <summary>
            /// Gets or sets the role.
            /// </summary>
            public string Role { get; set; }

            /// <summary>
            /// Gets or sets the unique provider identifier.
            /// </summary>
            public Guid ProviderGuid { get; set; }

            /// <summary>
            /// Gets or sets the provider name.
            /// </summary>
            public string ProviderName { get; set; }

            /// <summary>
            /// Gets or sets the ThreadId.
            /// </summary>
            public int ThreadId { get; set; }

            /// <summary>
            /// Gets or sets the opcode name.
            /// </summary>
            public string OpcodeName { get; set; }

            /// <summary>
            /// Gets or sets the keyword name.
            /// </summary>
            public string KeywordName { get; set; }

            /// <summary>
            /// Gets or sets the task name.
            /// </summary>
            public string TaskName { get; set; }

            /// <summary>
            /// Gets or sets the channel name.
            /// </summary>
            public string ChannelName { get; set; }

            /// <summary>
            /// Gets or sets the event message.
            /// </summary>
            public string EventMessage { get; set; }

            /// <summary>
            /// Gets or sets the ActivityId.
            /// </summary>
            public Guid ActivityId { get; set; }

            /// <summary>
            /// Gets or sets the message.
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Gets or sets the partition key.
            /// </summary>
            public string PartitionKey { get; set; }

            /// <summary>
            /// Gets or sets the row key.
            /// </summary>
            public string RowKey { get; set; }

            /// <summary>
            /// Gets or sets the 'N' column entry.
            /// </summary>
            public string N { get; set; }

            /// <summary>
            /// Gets or sets the row index.
            /// </summary>
            public string RowIndex { get; set; }

            /// <summary>
            /// Gets or sets the name of the source table.
            /// </summary>
            public string SourceTableName { get; set; }

            // ReSharper restore MemberCanBePrivate.Local
            // ReSharper restore UnusedAutoPropertyAccessor.Local

            /// <summary>
            /// Returns a string representation of the Orchestrator MDS data in CSV format.
            /// </summary>
            /// <returns>
            /// A string representation of the Orchestrator MDS data in CSV format.
            /// </returns>
            public new string ToString()
            {
                return
                    string.Format(
                        "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22}",
                        this.TimeStamp,
                        this.PreciseTimeStamp,
                        this.Tenant,
                        this.Role,
                        this.RoleInstance,
                        this.Level,
                        this.ProviderGuid,
                        this.ProviderName,
                        this.EventId,
                        this.ProcessId,
                        this.ThreadId,
                        this.OpcodeName,
                        this.KeywordName,
                        this.TaskName,
                        this.ChannelName,
                        this.EventMessage,
                        this.ActivityId,
                        this.Message,
                        this.PartitionKey,
                        this.RowKey,
                        this.N,
                        this.RowIndex,
                        this.SourceTableName);
            }
        }
    }
}
