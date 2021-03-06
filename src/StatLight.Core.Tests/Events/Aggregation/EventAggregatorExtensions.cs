﻿using System;

namespace StatLight.Core.Events.Aggregation
{
    public static class EventAggregatorExtensions
    {
        public static IEventSubscriptionManager AddListener<T>(this IEventSubscriptionManager eventAggregator, Action<T> listener)
        {
            var delegateListener = new DelegateListener<T>(listener);
            eventAggregator.AddListener(delegateListener);
            return eventAggregator;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IEventSubscriptionManager AddListener<T>(this IEventSubscriptionManager eventAggregator, Action listener)
        {
            var delegateListener = new DelegateListener<T>(msg => listener());
            eventAggregator.AddListener(delegateListener);
            return eventAggregator;
        }

        public static IEventSubscriptionManager AddListener(this IEventSubscriptionManager eventAggregator, Action listener, Predicate<object> filter)
        {
            return eventAggregator.AddListener(listener, filter);
        }

        private class DelegateListener<T> : IListener<T>
        {
            private readonly Action<T> _listener;

            public DelegateListener(Action<T> listener)
            {
                _listener = listener;
            }

            public void Handle(T message)
            {
                _listener(message);
            }
        }
    }
}