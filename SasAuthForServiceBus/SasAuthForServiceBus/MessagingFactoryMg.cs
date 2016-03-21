using System;
using System.Collections.Generic;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace SasAuthForServiceBus
{
    public class MessagingFactoryMg
    {
        private readonly MessagingFactory _internalMessagingFactory;

        public MessagingFactoryMg(string namespaceName, string ruleSharedAccessKey, string ruleName)
        {
            //First we need to build the uri
            Uri runtimeUri = ServiceBusEnvironment.CreateServiceUri("sb", namespaceName, string.Empty);

            //We need to generate the token using the shared key and the key name
            TokenProvider token = TokenProvider.CreateSharedAccessSignatureTokenProvider(ruleName, ruleSharedAccessKey);

            //We can create a new namespace manager now
            _internalMessagingFactory = MessagingFactory.Create(new List<Uri> { runtimeUri }, token);
        }

        public TopicClient GetTopicClient(string pathName)
        {
            return _internalMessagingFactory.CreateTopicClient(pathName);
        }
        public SubscriptionClient GetSubscriptionClientClient(string topicPath, string subscriptionName)
        {
            return _internalMessagingFactory.CreateSubscriptionClient(topicPath,subscriptionName);
        }
    }
}
