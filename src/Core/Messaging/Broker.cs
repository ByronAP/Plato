﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlatoCore.Messaging.Abstractions;

namespace PlatoCore.Messaging
{
   
    public class Broker : IBroker
    {

        private readonly ConcurrentDictionary<Type,  List<DescribedDelegate>> _subscribers;

        public Broker()
        {
            _subscribers = new ConcurrentDictionary<Type, List<DescribedDelegate>>();
        }

        public IEnumerable<Func<Message<T>, Task<T>>> Pub<T>(object sender, string key, T message)
            where T : class
        {
            return Pub<T>(sender, new MessageOptions()
            {
                Key = key
            }, message);
        }

        public IEnumerable<Func<Message<T>, Task<T>>> Pub<T>(object sender, string key) where T : class
        {
            return Pub<T>(sender, new MessageOptions()
            {
                Key = key
            }, null);
        }

        public IEnumerable<Func<Message<T>, Task<T>>> Pub<T>(object sender, MessageOptions options, T message) where T : class
        {

            var output = new List<Func<Message<T>, Task<T>>>();
            
            // Nothing to process return empty collection
            if (sender == null)
            {
                return output;
            }

            // No _subscribers for given type return empty collection
            if (!_subscribers.ContainsKey(typeof(T)))
            {
                return output;
            }

            // No delegates within subscriber for given type return empty collection
            var delegates = _subscribers[typeof(T)];
            if (delegates == null || delegates.Count == 0)
            {
                return output;
            }

            // The payload passed to each subscriber delegate
            //var delegatePayload = new Message<T>(message, sender);

            // Iterate through subscriber action delegates matching our key
            foreach (var handler in delegates
                .Where(d => d.Options.Key == options.Key)
                .OrderBy(d => d.Options.Order)
                .Select(s => s.Subscription as Action<Message<T>>)
                )
            {
                if (handler != null)
                {
                    // Action delegates return void and as such cannot be awaited
                    // Wrap action delegates within a dummy func delegate ensuring 
                    // the action can be executed consistently and asynchronously 
                    output.Add(async input =>
                    {
                        return await Task.Factory.StartNew(() =>
                        {
                            handler.Invoke(input);
                            return input.What;
                        });
                    });
                }         
              
            }
            
            // Iterate through subscriber func delegates matching our key
            foreach (var func in delegates
                .Where(d => d.Options.Key == options.Key)
                .OrderBy(d => d.Options.Order)
                .Select(s => s.Subscription as Func<Message<T>, Task<T>>))
            {
                if (func != null)
                {
                    // Wrap our subscriber delegate within a dummy delegate
                    // This allows us to invoke the dummy delegate externally
                    // passing in a custom message for our real subscriber delegate
                    output.Add(async input => await func(input));
                }
            }

            // Return funcs to invoke
            return output;

        }

        public void Sub<T>(MessageOptions options, Action<Message<T>> subscription) where T : class
        {
            var describedDelegate = new DescribedDelegate(options, subscription);

            var delegates = _subscribers.ContainsKey(typeof(T))
                ? _subscribers[typeof(T)]
                : new List<DescribedDelegate>();

            if (!delegates.Contains(describedDelegate))
            {
                delegates.Add(describedDelegate);
            }
            _subscribers[typeof(T)] = delegates;
        }
        
        public void Sub<T>(MessageOptions options, Func<Message<T>, Task<T>> subscription) where T : class
        {
            var describedDelegate = new DescribedDelegate(options, subscription);

            var delegates = _subscribers.ContainsKey(typeof(T)) 
                ? _subscribers[typeof(T)] 
                : new List<DescribedDelegate>();

            if (!delegates.Contains(describedDelegate))
            {
                delegates.Add(describedDelegate);
            }

            _subscribers[typeof(T)] = delegates;

        }

        public void Unsub<T>(MessageOptions options, Action<Message<T>> subscription) where T : class
        {
          
            if (!_subscribers.ContainsKey(typeof(T))) return;

            // Get delegates for our type
            var typeDelegates = _subscribers[typeof(T)];

            // Get delegates for type matching our key
            var matchingKey = typeDelegates
                .Where(d => d.Options.Key == options.Key)
                .ToList();

            // Get delegates matching our subscription target
            var matchingDelegates = matchingKey
                .Where(d => d.Subscription.Target.Equals(subscription.Target))
                .ToList();

            // Remove
            foreach (var matchingDelegate in matchingDelegates)
            {
                if (typeDelegates.Contains(matchingDelegate))
                {
                    typeDelegates.Remove(matchingDelegate);
                }
            }
            
            if (typeDelegates.Count == 0)
            {
                _subscribers.TryRemove(typeof(T), out List<DescribedDelegate> method);
            }
        }

        public void Unsub<T>(MessageOptions options, Func<Message<T>, Task<T>> subscription) where T : class
        {

            if (!_subscribers.ContainsKey(typeof(T))) return;

            // Get delegates for our type
            var typeDelegates = _subscribers[typeof(T)];

            // Get delegates for type matching our key
            var matchingKey = typeDelegates
                .Where(d => d.Options.Key == options.Key)
                .ToList();

            // Get delegates matching our subscription target
            var matchingDelegates = matchingKey
                .Where(d => d.Subscription.Target.Equals(subscription.Target))
                .ToList();
            
            // Remove
            foreach (var matchingDelegate in matchingDelegates)
            {
                if (typeDelegates.Contains(matchingDelegate))
                {
                    typeDelegates.Remove(matchingDelegate);
                }
            }

            if (typeDelegates.Count == 0)
            {
                _subscribers.TryRemove(typeof(T), out List<DescribedDelegate> method);
            }
                
        }
        
        public void Dispose()
        {
            _subscribers?.Clear();
        }
        
    }

}
