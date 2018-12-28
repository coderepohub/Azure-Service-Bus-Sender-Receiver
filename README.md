# Azure Service Bus Topic Sender and Receiver application 
This is a simple console application project which contains two project one *Sender* and one *Receiver* . We can make both the project as statrtup project two send and recieve messages simultaneously or can have these projects running seperately.
( I have made both the project as STARTUP project by default).

It contains two projects : 
1. **ServiceBusTopicApp** - Service Bus Message Sender Application
2. **ServiceBusTopicRecieverApp** - Service Bus Message Receiver Application

## Create Service Bus in Azure

1.  Visit [Azure Portal](https://portal.azure.com/) and Create a new Service Bus .
2.  Note down the Service Bus namespace connection string and keys 

- NOTE : Update the *App.config* 

