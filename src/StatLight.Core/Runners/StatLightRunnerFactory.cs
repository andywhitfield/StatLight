﻿
namespace StatLight.Core.Runners
{
	using System;
	using System.Collections.Generic;
	using Microsoft.Practices.Composite.Events;
	using StatLight.Core.Common;
	using StatLight.Core.Monitoring;
	using StatLight.Core.Reporting.Providers.Console;
	using StatLight.Core.Reporting.Providers.TeamCity;
	using StatLight.Core.Timing;
	using StatLight.Core.WebBrowser;
	using StatLight.Core.WebServer;

	public static class StatLightRunnerFactory
	{
		private static readonly IEventAggregator EventAggregator = new EventAggregator();
		private static BrowserCommunicationTimeoutMonitor _browserCommunicationTimeoutMonitor;

		public static IRunner CreateContinuousTestRunner(ILogger logger, string xapPath, TestRunConfiguration testRunConfiguration, bool showTestingBrowserHost, ServerTestRunConfiguration serverTestRunConfiguration)
		{
			StatLightService statLightService;
			StatLightServiceHost statLightServiceHost;
			BrowserFormHost browserFormHost;

			BuildAndReturnWebServiceAndBrowser(
				logger,
				xapPath,
				showTestingBrowserHost,
				testRunConfiguration,
				serverTestRunConfiguration,
				out statLightService,
				out statLightServiceHost,
				out browserFormHost);

			IRunner runner = new ContinuousConsoleRunner(logger, EventAggregator, xapPath, statLightService, statLightServiceHost, browserFormHost, new ConsoleResultHandler());
			return runner;
		}

		public static IRunner CreateTeamCityRunner(string xapPath, TestRunConfiguration testRunConfiguration, ServerTestRunConfiguration serverTestRunConfiguration)
		{
			ILogger logger = new NullLogger();

			StatLightService statLightService;
			StatLightServiceHost statLightServiceHost;
			BrowserFormHost browserFormHost;

			BuildAndReturnWebServiceAndBrowser(
				logger,
				xapPath,
				false,
				testRunConfiguration,
				serverTestRunConfiguration,
				out statLightService,
				out statLightServiceHost,
				out browserFormHost);

			var publisher = new TeamCityTestResultHandler(new ConsoleCommandWriter(), xapPath);

			IRunner runner = new TeamCityRunner(new NullLogger(), EventAggregator, statLightServiceHost, browserFormHost, publisher);

			return runner;
		}

		public static IRunner CreateOnetimeConsoleRunner(ILogger logger, string xapPath, TestRunConfiguration testRunConfiguration, ServerTestRunConfiguration serverTestRunConfiguration, bool showTestingBrowserHost)
		{
			StatLightService statLightService;
			StatLightServiceHost statLightServiceHost;
			BrowserFormHost browserFormHost;

			BuildAndReturnWebServiceAndBrowser(
				logger,
				xapPath,
				showTestingBrowserHost,
				testRunConfiguration,
				serverTestRunConfiguration,
				out statLightService,
				out statLightServiceHost,
				out browserFormHost);

			IRunner runner = new OnetimeRunner(logger, EventAggregator, statLightServiceHost, browserFormHost, new ConsoleResultHandler());
			return runner;
		}

		private static void BuildAndReturnWebServiceAndBrowser(
			ILogger logger,
			string xapPath,
			bool showTestingBrowserHost,
			TestRunConfiguration testRunConfiguration,
			ServerTestRunConfiguration serverTestRunConfiguration,
			out StatLightService statLightService,
			out StatLightServiceHost statLightServiceHost,
			out BrowserFormHost browserFormHost)
		{
			var location = new WebServerLocation();
			var debugAssertMonitorTimer = new TimerWrapper(5000);
			var dialogMonitors = new List<IDialogMonitor>
			{
				new DebugAssertMonitor(logger),
				new MessageBoxMonitor(logger),
			};
			var dialogMonitorRunner = new DialogMonitorRunner(logger, EventAggregator, debugAssertMonitorTimer, dialogMonitors);

			statLightService = new StatLightService(logger, EventAggregator, xapPath, testRunConfiguration, serverTestRunConfiguration);
			statLightServiceHost = new StatLightServiceHost(logger, statLightService, location.BaseUrl);
			browserFormHost = new BrowserFormHost(logger, location.TestPageUrl, showTestingBrowserHost, dialogMonitorRunner);

			StartupBrowserCommunicationTimeoutMonitor(new TimeSpan(0, 0, 5, 0));
		}

		private static void StartupBrowserCommunicationTimeoutMonitor(TimeSpan maxTimeAllowedBeforeCommErrorSent)
		{
			_browserCommunicationTimeoutMonitor = new BrowserCommunicationTimeoutMonitor(EventAggregator, new TimerWrapper(3000), maxTimeAllowedBeforeCommErrorSent);
		}


		public static IRunner CreateWebServerOnlyRunner(ILogger logger, string xapPath, TestRunConfiguration testRunConfiguration, ServerTestRunConfiguration serverTestRunConfiguration)
		{
			var location = new WebServerLocation();

			var statLightService = new StatLightService(logger, EventAggregator, xapPath, testRunConfiguration, serverTestRunConfiguration);
			var statLightServiceHost = new StatLightServiceHost(logger, statLightService, location.BaseUrl);

			IRunner runner = new WebServerOnlyRunner(logger, EventAggregator, statLightServiceHost, location.TestPageUrl, new ConsoleResultHandler());

			return runner;
		}
	}
}
