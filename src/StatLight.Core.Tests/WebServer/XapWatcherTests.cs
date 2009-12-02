﻿using System.IO;
using NUnit.Framework;
using StatLight.Core.WebServer;

namespace StatLight.Core.Tests.WebServer
{
	namespace XapMonitorTests
	{
		[TestFixture]
		public class when_initializing_the_XapWatcher : using_a_random_temp_file_for_testing
		{

			[Test]
			public void Should_be_able_to_initialize_the_XapWatcher_by_specifying_the_xap_path()
			{
				var xapWatcher = new XapFileBuildChangedMonitor(PathToTempXapFile);
			}

			[Test]
			public void Should_throw_a_FileNotFoundException_if_the_given_file_does_not_exist()
			{
				typeof(FileNotFoundException).ShouldBeThrownBy(() => new XapFileBuildChangedMonitor(PathToTempXapFile + "badpath"));
			}

		}

		[TestFixture]
		public class when_the_XapWatcher_has_been_initialized_with_a_file_and_it_has_been_loaded : using_a_random_temp_file_for_testing
		{
			XapFileBuildChangedMonitor _xapWatcher;

			protected override void Before_each_test()
			{
				base.Before_each_test();

				_xapWatcher = new XapFileBuildChangedMonitor(PathToTempXapFile);
			}

			[Test]
			public void Should_raise_file_refreshed_event_when_existing_file_changes()
			{
				bool wasXapFileRefreshed = false;
				_xapWatcher.FileChanged += (sender, e) =>
				{
					wasXapFileRefreshed = true;
				};

				base.replace_test_file();

				// refresh event doesn't fire in time so we wait
				System.Threading.Thread.Sleep(50);

				wasXapFileRefreshed.ShouldBeTrue();
			}


			[Test]
			public void when_simulating_a_build_the_changed_event_should_only_be_raised_once_in_a_short_amount_of_time()
			{

				int raisedCount = 0;
				_xapWatcher.FileChanged += (sender, e) =>
				{
					raisedCount++;
				};

				// when building in Visual Studio, it appeared that there were
				// 5 different changed events thrown.
				base.replace_test_file();
				base.replace_test_file();
				base.replace_test_file();
				base.replace_test_file();
				base.replace_test_file();

				// refresh event doesn't fire in time so we wait
				System.Threading.Thread.Sleep(50);

				raisedCount.ShouldEqual(1);
			}
		}
	}
}
