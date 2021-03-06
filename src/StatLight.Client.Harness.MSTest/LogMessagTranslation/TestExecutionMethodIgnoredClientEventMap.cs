﻿using System;
using Microsoft.Silverlight.Testing.Harness;
#if March2010 || April2010 || May2010
#elif May2010 || July2009 || October2009 || November2009
using Microsoft.Silverlight.Testing.UnitTesting.Harness;
#endif
using StatLight.Client.Harness.Events;

namespace StatLight.Client.Harness.Hosts.MSTest.LogMessagTranslation
{
    public class TestExecutionMethodIgnoredClientEventMap : ILogMessageToClientEventTranslation
    {
        public bool CanTranslate(LogMessage message)
        {

            if (message.MessageType == LogMessageType.TestExecution)
            {
                if (message.Is(TestGranularity.TestScenario)
                    && message.DecoratorMatches(UnitTestLogDecorator.IgnoreMessage, v => (bool)v)
                    )
                {
                    return true;
                }
            }
            return false;
        }

        public ClientEvent Translate(LogMessage message)
        {
            var testName = (string)message.Decorators[LogDecorator.NameProperty];
            var clientEventX = new TestExecutionMethodIgnoredClientEvent
                                   {
                                       ClassName = null,
                                       NamespaceName = null,
                                       MethodName = testName,
                                       Message = testName,
                                       Started = DateTime.Now,
                                   };

            return clientEventX;
        }
    }
}