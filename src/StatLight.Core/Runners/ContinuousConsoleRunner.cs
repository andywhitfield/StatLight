﻿
namespace StatLight.Core.Runners
{
    using System;
    using System.Threading;
    using StatLight.Core.Common;
    using StatLight.Core.Configuration;
    using StatLight.Core.Events.Aggregation;
    using StatLight.Core.Monitoring;
    using StatLight.Core.Reporting;
    using StatLight.Core.WebBrowser;
    using StatLight.Core.WebServer;

    internal class ContinuousConsoleRunner : IRunner
    {
        private readonly string _xapPath;
        private readonly IWebServer _webServer;
        private readonly Thread _continuousRunnerThread;
        private readonly XapFileBuildChangedMonitor _xapFileBuildChangedMonitor;

        internal ContinuousConsoleRunner(
            ILogger logger,
            IEventSubscriptionManager eventSubscriptionManager,
            IEventPublisher eventPublisher ,
            string xapPath,
            ClientTestRunConfiguration clientTestRunConfiguration,
            IWebServer webServer,
            IWebBrowser webBrowser)
        {
            _xapPath = xapPath;
            _webServer = webServer;
            _xapFileBuildChangedMonitor = new XapFileBuildChangedMonitor(eventPublisher, _xapPath);

            _continuousRunnerThread = new Thread(() =>
            {
                using (var runner = new ContinuousTestRunner(logger, eventSubscriptionManager, eventPublisher, webBrowser, clientTestRunConfiguration, _xapPath))
                {
                    string line;
                    while (!(line = System.Console.ReadLine()).Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        runner.ForceFilteredTest(line);
                    }
                }
            });
        }

        public TestReport Run()
        {
            _webServer.Start();

            _continuousRunnerThread.Start();
            _continuousRunnerThread.Join();

            return new TestReport(_xapPath);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _xapFileBuildChangedMonitor.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
