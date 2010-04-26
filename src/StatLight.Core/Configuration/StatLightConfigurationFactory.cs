﻿
namespace StatLight.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Ionic.Zip;
    using StatLight.Core.Common;
    using StatLight.Core.UnitTestProviders;
    using StatLight.Core.WebServer.XapHost;
    using StatLight.Core.WebServer.XapInspection;

    public class StatLightConfigurationFactory
    {
        public const int DefaultDialogSmackDownElapseMilliseconds = 5000;
        private readonly ILogger _logger;
        private readonly XapHostFileLoaderFactory _xapHostFileLoaderFactory;

        public StatLightConfigurationFactory(ILogger logger)
        {
            _logger = logger;
            _xapHostFileLoaderFactory = new XapHostFileLoaderFactory(_logger);
        }

        public StatLightConfiguration GetStatLightConfiguration(UnitTestProviderType unitTestProviderType, string xapPath, MicrosoftTestingFrameworkVersion? microsoftTestingFrameworkVersion, List<string> methodsToTest, string tagFilters)
        {
            AssertXapToTestFileExists(xapPath);

            XapReadItems xapReadItems = new XapReader(_logger).GetTestAssembly(xapPath);
            if (unitTestProviderType == UnitTestProviderType.Undefined || microsoftTestingFrameworkVersion == null)
            {
                //TODO: Print message telling the user what the type is - and if they give it
                // we don't have to "reflect" on the xap to determine the test provider type.

                if (unitTestProviderType == UnitTestProviderType.Undefined)
                {
                    unitTestProviderType = xapReadItems.UnitTestProvider;
                }

                if (
                    (xapReadItems.UnitTestProvider == UnitTestProviderType.MSTest || unitTestProviderType == UnitTestProviderType.MSTest)
                    && microsoftTestingFrameworkVersion == null)
                {
                    microsoftTestingFrameworkVersion = xapReadItems.MicrosoftSilverlightTestingFrameworkVersion;
                }
            }

            var clientConfig = new ClientTestRunConfiguration(unitTestProviderType, methodsToTest, tagFilters);

            var serverConfig = CreateServerConfiguration(
                xapPath,
                clientConfig.UnitTestProviderType,
                microsoftTestingFrameworkVersion,
                xapReadItems);

            return new StatLightConfiguration(clientConfig, serverConfig);
        }

        private static void AssertXapToTestFileExists(string xapPath)
        {
            if (!File.Exists(xapPath))
            {
                throw new FileNotFoundException("File could not be found. [{0}]".FormatWith(xapPath));
            }
        }

        private ServerTestRunConfiguration CreateServerConfiguration(
            string xapPath,
            UnitTestProviderType unitTestProviderType,
            MicrosoftTestingFrameworkVersion? microsoftTestingFrameworkVersion,
            XapReadItems xapReadItems)
        {
            return CreateServerConfiguration(xapPath, unitTestProviderType, microsoftTestingFrameworkVersion, xapReadItems, DefaultDialogSmackDownElapseMilliseconds);
        }

        private ServerTestRunConfiguration CreateServerConfiguration(
            string xapPath,
            UnitTestProviderType unitTestProviderType,
            MicrosoftTestingFrameworkVersion? microsoftTestingFrameworkVersion,
            XapReadItems xapReadItems,
            long dialogSmackDownElapseMilliseconds
            )
        {
            XapHostType xapHostType = _xapHostFileLoaderFactory.MapToXapHostType(unitTestProviderType, microsoftTestingFrameworkVersion);

            byte[] hostXap = _xapHostFileLoaderFactory.LoadXapHostFor(xapHostType);
            hostXap = RewriteXapWithSpecialFiles(hostXap, xapReadItems);

            Func<byte[]> xapToTestFactory = () =>
            {
                AssertXapToTestFileExists(xapPath);
                return File.ReadAllBytes(xapPath);
            };

            return new ServerTestRunConfiguration(hostXap, dialogSmackDownElapseMilliseconds, xapPath, xapHostType, xapToTestFactory);
        }


        private byte[] RewriteXapWithSpecialFiles(byte[] xapHost, XapReadItems xapReadItems)
        {
            if (xapReadItems != null)
            {
                //TODO: maybe specify this list as something passed in by the user???
                var specialFilesToCopyIntoHostXap = new List<string>
                                                        {
                                                            "ServiceReferences.ClientConfig",
                                                        };

                var filesToCopyIntoHostXap = (from x in xapReadItems.FilesContianedWithinXap
                                              from specialFile in specialFilesToCopyIntoHostXap
                                              where x.FileName.Equals(specialFile, StringComparison.OrdinalIgnoreCase)
                                              select x).ToList();

                if (filesToCopyIntoHostXap.Any())
                {
                    xapHost = RewriteZipHostWithFiles(xapHost, filesToCopyIntoHostXap);
                }
            }

            return xapHost;
        }

        private byte[] RewriteZipHostWithFiles(byte[] hostXap, IEnumerable<IXapFile> filesToPlaceIntoHostXap)
        {
            ZipFile zipFile = ZipFile.Read(hostXap);

            _logger.Debug("re-writing host xap with the following files");
            foreach (var file in filesToPlaceIntoHostXap)
            {
                _logger.Debug("    -  {0}".FormatWith(file.FileName));
                zipFile.AddEntry(file.FileName, "/", file.File);
            }

            using (var stream = new MemoryStream())
            {
                zipFile.Save(stream);
                return stream.ToArray();
            }
        }
    }
}