using System;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace SasAuthForServiceBus
{
    public class NamespaceMgr
    {
        private readonly NamespaceManager _internalNamespaceManager;


        public NamespaceMgr(string namespaceName, string namespaceSharedAccessKey, string namespacePolicyName)
        {
            //First we need to build the uri
            Uri runtimeUri = ServiceBusEnvironment.CreateServiceUri("sb", namespaceName, string.Empty);

            //We need to generate the token using the shared key and the key name
            TokenProvider token = TokenProvider.CreateSharedAccessSignatureTokenProvider(namespacePolicyName, namespaceSharedAccessKey);

            //We can create a new namespace manager now
            _internalNamespaceManager = new NamespaceManager(runtimeUri, token);
        }

     
        //Namespace police must has manage clains in order to create, delete, update or list topics, queues or subscriptions
        public  void CreateTopic(TopicDescription td )
        {
            //Just delete it if already exists. Only for testing.
            DeleteTopic(td.Path);
            _internalNamespaceManager.CreateTopic(td);
         
        }

        public void DeleteTopic(string pathname)
        {
            _internalNamespaceManager.DeleteTopic(pathname);
        }

        public void UpdateTopic(TopicDescription td)
        {
            _internalNamespaceManager.UpdateTopic(td);
        }
        
        //Namespace police must has manage clains in order to create, delete, update or list topics, queues or subscriptions
        public  void CreateSubscription(string pathName, string subscriptionName)
        {
            if (!_internalNamespaceManager.TopicExists(pathName))
            {
                _internalNamespaceManager.CreateSubscription(pathName, subscriptionName);
            }
        }

    }
}
