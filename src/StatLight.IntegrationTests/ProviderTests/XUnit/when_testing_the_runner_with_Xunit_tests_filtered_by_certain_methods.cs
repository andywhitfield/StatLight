﻿using System.Collections.Generic;
using NUnit.Framework;
using StatLight.Core.UnitTestProviders;
using StatLight.Core.WebServer;

namespace StatLight.IntegrationTests.ProviderTests.XUnit
{
	[TestFixture]
	public class when_testing_the_runner_with_Xunit_tests_filtered_by_certain_methods
		: filtered_tests____
	{
		private TestRunConfiguration _testRunConfiguration;

		protected override TestRunConfiguration TestRunConfiguration
		{
			get { return this._testRunConfiguration; }
		}

		protected override void Before_all_tests()
		{
			base.PathToIntegrationTestXap = TestXapFileLocations.XUnit;

			_testRunConfiguration = new TestRunConfiguration
			                        	{
			                        		TagFilter = string.Empty,
			                        		UnitTestProviderType = UnitTestProviderType.XUnit,
			                        		MethodsToTest = new List<string>()
			                        		                	{
			                        		                		(base.NormalClassTestName = "StatLight.IntegrationTests.Silverlight.XunitTests+XunitNestedClassTests") + ".this_should_be_a_passing_test",
			                        		                		(base.NestedClassTestName = "StatLight.IntegrationTests.Silverlight.XunitTests") + ".this_should_be_a_passing_test",
			                        		                	}
			                        	};

			base.Before_all_tests();

			TestResults = base.Runner.Run();
		}
	}
}