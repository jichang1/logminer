// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataParserTests.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.Hackathon.Tests
{
    using Microsoft.Hackathon.DataParser;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for MdsParser.
    /// </summary>
    [TestClass]
    public class DataParserTests
    {
        /// <summary>
        /// As a user I expect the supported arguments work.
        /// </summary>
        //// Note: Used during dev; functional tests not supported.
        ////[TestMethod]
        public void CanHandleKnownArguments()
        {
            var defaultOptions = new[] { @"C:\folder\file.ext" };
            DataParser.Main(defaultOptions);

            var powerShellStylePathOption = new[] { "-Path", @"C:\folder\file.ext", "-Destination", @"C:\folder\file.ext" };
            DataParser.Main(powerShellStylePathOption);

            var dosStylePathOption = new[] { "/PATH", @"C:\folder\file.ext" };
            DataParser.Main(dosStylePathOption);

            var ossStylePathOption = new[] { "--path", @"C:\folder\file.ext" };
            DataParser.Main(ossStylePathOption);

            var destOption = new[] { "-dest", @"C:\folder\file.ext" };
            DataParser.Main(destOption);

            var destinationOption = new[] { "-destination", @"C:\folder\file.ext" };
            DataParser.Main(destOption);
        }

        /// <summary>
        /// As a user I expect the path can contain wildcards.
        /// </summary>
        //// Note: Used during dev; functional tests not supported.
        ////[TestMethod]
        public void PathCanContainWildcards()
        {
            var fullPath = new[] { "-Path", @"C:\folder\file*" };
            DataParser.Main(fullPath);

            var fileOnly = new[] { "-Path", @"file*" };
            DataParser.Main(fileOnly);

            var dirOnly = new[] { "-Path", @"C:\folder\" };
            DataParser.Main(dirOnly);

            var realPath = new[] { "-Path", @"\\jichang01\public\oneweek\oaasprod\CDMOaaSProdEtwAllVer2v0*" };
            DataParser.Main(realPath);
        }

        /// <summary>
        /// As a user I expect the files are processed in order.
        /// </summary>
        //// Note: Used during dev; functional tests not supported.
        ////[TestMethod]
        public void CanEnumerateFilesInOrder()
        {
            var path = new[] { "oaas", @"\\jichang01\public\oneweek\oaasprod\CDMOaaSProdEtwAllVer2v0*", @"C:\Users\miroman\Projects\hackathon-oneweek-2014\OaaSProdEventRecords.csv" };
            ////var path = new[] { "ml", @"\\jichang01\public\oneweek\oaasprod\datalabCustomLogEventlkgVer2v0*", @"C:\Users\miroman\Projects\hackathon-oneweek-2014\LkgEventRecords.csv" };
            DataParser.Main(path);
        }
    }
}
