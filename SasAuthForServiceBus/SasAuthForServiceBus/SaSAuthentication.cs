using System;
using System.Collections.Generic;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace SasAuthForServiceBus
{
    internal class SaSAuthentication
    {
        private const string NamespaceName = "";
        private const string NamespaceSharedAccessKey = "";
        private const string NamespacePolicyName = "NuevaPolicy";

        static void Main()
        {
            NamespaceMgr mgrWithRights = new NamespaceMgr(NamespaceName,NamespaceSharedAccessKey,NamespacePolicyName);

            TopicDescription td = new TopicDescription("Vegetables");
            var authListen = new SharedAccessAuthorizationRule("VegetablesListenRule", new[] {AccessRights.Listen});
            var authSend = new SharedAccessAuthorizationRule("VegetablesSendRule", new[] { AccessRights.Send });

            td.Authorization.Add(authListen);
            td.Authorization.Add(authSend);

            mgrWithRights.CreateTopic(td);
            mgrWithRights.CreateSubscription("Vegetables", "VegetablesSubscription");


            //If policy has manage or listen claims must be possible to subscribe to a topic and start listening
            //The policy must be created AFTER the topic is created
            MessagingFactoryMg listenFactory = new MessagingFactoryMg(NamespaceName, authListen.PrimaryKey, "VegetablesListenRule");

            //We can create a subscription since the policy allow to listen
            SubscriptionClient subscription = listenFactory.GetSubscriptionClientClient(td.Path, "AllMessages");

            //If policy has manage or send claims must be possible to send a messag3e to the topic
            //The policy must be created AFTER the topic is created
            MessagingFactoryMg sendFactory = new MessagingFactoryMg(NamespaceName, authSend.PrimaryKey, "VegetablesSendRule");

            //We can create a new topic client since the policy allow to send
            TopicClient topicClient = sendFactory.GetTopicClient(td.Path);


            OnMessageOptions options = new OnMessageOptions
            {
                AutoComplete = false,
                AutoRenewTimeout = TimeSpan.FromMinutes(1)
            };

            //Callback to be executed on message
            subscription.OnMessage(message =>
            {
                try
                {
                    Console.WriteLine("We have receieved a message");
                    Console.WriteLine($"With messageId = {message.MessageId}");
                }
                catch (Exception)
                {
                    message.Abandon();
                }

            },options);


            Console.WriteLine("Enter M to send a message or F to finish");
            var imput = Console.ReadLine();
            while (imput != "f")
            {
                if (imput == "m")
                {
                    //Send a new message
                    topicClient.Send(new BrokeredMessage());
                }

                imput = Console.ReadLine();
            }
        }
    }
}
