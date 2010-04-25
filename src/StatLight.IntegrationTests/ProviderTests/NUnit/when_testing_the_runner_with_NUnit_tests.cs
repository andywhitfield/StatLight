using NUnit.Framework;
using StatLight.Core.Configuration;
using StatLight.Core.Tests;
using StatLight.Core.UnitTestProviders;

namespace StatLight.IntegrationTests.ProviderTests.NUnit
{
	[TestFixture]
	public class when_testing_the_runner_with_NUnit_tests
		: IntegrationFixtureBase
	{
		private ClientTestRunConfiguration clientTestRunConfiguration;

		protected override ClientTestRunConfiguration ClientTestRunConfiguration
		{
			get { return this.clientTestRunConfiguration; }
		}

		protected override void Before_all_tests()
		{
            base.Before_all_tests();
            base.PathToIntegrationTestXap = TestXapFileLocations.NUnit;
            this.clientTestRunConfiguration = new IntegrationTestClientTestRunConfiguration(UnitTestProviderType.NUnit);
		}


		[Test]
		public void Should_have_correct_TotalFailed_count()
		{
			TestReport.TotalFailed.ShouldEqual(1, "Failed count wrong");
		}

		[Test]
		public void Should_have_correct_TotalPassed_count()
		{
			TestReport.TotalPassed.ShouldEqual(4, "Passed count wrong");
		}

		[Test]
		public void Should_have_correct_TotalIgnored_count()
		{
			TestReport.TotalIgnored.ShouldEqual(1, "Ignored count wrong");
		}
	}
}