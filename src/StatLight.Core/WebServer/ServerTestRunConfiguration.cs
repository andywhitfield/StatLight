﻿using StatLight.Core.Common;

namespace StatLight.Core.WebServer
{
    using StatLight.Core.WebServer.XapHost;

    public class ServerTestRunConfiguration
    {
        private readonly XapHostFileLoaderFactory _xapHostFileLoaderFactory;
        private readonly MicrosoftTestingFrameworkVersion _microsoftTestingFrameworkVersion;
        private byte[] _hostXap;
        public long DialogSmackDownElapseMilliseconds { get; set; }

        public ServerTestRunConfiguration(XapHostFileLoaderFactory xapHostFileLoaderFactory, MicrosoftTestingFrameworkVersion microsoftTestingFrameworkVersion)
        {
            _xapHostFileLoaderFactory = xapHostFileLoaderFactory;
            _microsoftTestingFrameworkVersion = microsoftTestingFrameworkVersion;

            DialogSmackDownElapseMilliseconds = 5000;
        }

        public byte[] HostXap
        {
            get
            {
                if (_hostXap == null)
                    _hostXap = _xapHostFileLoaderFactory.LoadXapHostFor(_microsoftTestingFrameworkVersion);

                return _hostXap;
            }
        }
    }
}
