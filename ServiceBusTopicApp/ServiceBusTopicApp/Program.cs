using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusTopicApp
{
    class Program
    {
        //ConnectionString for the Service Bus
        static string connectionString = ConfigurationManager.AppSettings["ServiceBus_ConnectionString"].ToString();
        static string topicName = ConfigurationManager.AppSettings["TopicName"].ToString();
        static string subscriptionName = ConfigurationManager.AppSettings["SubscriptionName"].ToString();
        static void Main(string[] args)
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
                
            if (!namespaceManager.TopicExists(topicName))
            {
               var topicDescription= namespaceManager.CreateTopic(topicName);
                var sharedAccessSign = "RootManage";
                var authRule = topicDescription.Authorization.OfType<SharedAccessAuthorizationRule>().FirstOrDefault(c => c.KeyName == sharedAccessSign);
                if(authRule==null)
                {
                    authRule = new SharedAccessAuthorizationRule(sharedAccessSign, SharedAccessAuthorizationRule.GenerateRandomKey(), new[] { AccessRights.Manage,AccessRights.Send,AccessRights.Listen });
                    topicDescription.Authorization.Add(authRule);
                    namespaceManager.UpdateTopic(topicDescription);
                }
            }
            
            //Create the subscription if not exist to send the message 
            if (!namespaceManager.SubscriptionExists(topicName, subscriptionName))
            {
                namespaceManager.CreateSubscription(topicName, subscriptionName);
            }
            
            for (int i = 0; i < 5; i++)
            {
                // Create message, passing a string message for the body.
                BrokeredMessage message = new BrokeredMessage("Test message " + i);

                // Set additional custom app-specific property.
                message.Properties["MessageId"] = i;
                TopicClient Client = TopicClient.CreateFromConnectionString(connectionString, topicName);

                // Send message to the topic.
                Client.Send(message);
            }

            Console.ReadKey();
        }
    }
}
