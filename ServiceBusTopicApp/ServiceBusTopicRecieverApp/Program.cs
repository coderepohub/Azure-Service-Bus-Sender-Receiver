using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusTopicRecieverApp
{
    class Program
    {
        static string connectionString = ConfigurationManager.AppSettings["ServiceBus_ConnectionString"].ToString();
        static string topicName = ConfigurationManager.AppSettings["TopicName"].ToString();
        static string subscriptionName = ConfigurationManager.AppSettings["SubscriptionName"].ToString();
        static void Main(string[] args)
        {
            Console.WriteLine("Message Recieving ---->");
            Console.Title = "Message Reciever";
         
            SubscriptionClient Client = SubscriptionClient.CreateFromConnectionString(connectionString, topicName, subscriptionName);

            // Configure the callback options.
            OnMessageOptions options = new OnMessageOptions();
            options.AutoComplete = false;
            options.AutoRenewTimeout = TimeSpan.FromMinutes(1);


            Client.OnMessage((message) =>
            {
                try
                {
                    // Process message from subscription.
                    Console.WriteLine("\n**Stream Messages**");
                    Console.WriteLine("Body: " + message.GetBody<string>());
                    //WE CAN ALSO DESERIALIZE THE MESSAGE BASED ON DEFINED ENTITY 
                    //e.g : JsonConvert.DeserializeObject<ENTITYNAME>(message.GetBody<string>());  
                    Console.WriteLine("MessageID: " + message.MessageId);
                    Console.WriteLine("Message Number: " + message.Properties["MessageNumber"]);
                    // Remove message from subscription.
                    message.Complete();
                }
                catch (Exception)
                {
                    // Indicates a problem, unlock message in subscription.
                    message.Abandon();
                }
            }, options);


            Console.ReadKey();
        }
    }
}
