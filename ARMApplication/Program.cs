using System;
using System.Collections.Specialized;
using System.Configuration;

namespace ARMApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var azureSettings = (NameValueCollection) ConfigurationManager.GetSection("azureAppSettings");
            Cloud azureCloud = new Cloud
            {
                clientId = azureSettings["clientId"], // Change this value in app.config
                clientSecret = azureSettings["clientSecret"], // Change this value in app.config
                loginEndpoint = azureSettings["loginEndpoint"],
                directoryTenantName = azureSettings["directoryTenantName"],
                armEndpoint = azureSettings["armEndpoint"],
                armResourceId = azureSettings["armResourceId"],
                armApiVersion = azureSettings["armApiVersion"]
            };

            // Authenticate to the specific Cloud's Resource Manager.
            azureCloud.Authenticate();
            // Console.WriteLine(azureCloud.GetToken());

            string subscriptionId = "<INSERT VALUE HERE>";
            string resourceGroupName = "<INSERT VALUE HERE>";
            string virtualMachineName = "<INSERT VALUE HERE>";
            string virtualNetworkName = "<INSERT VALUE HERE>";
            string publicIpAddressName = "<INSERT VALUE HERE>";

            #region Subscription Methods
            Console.WriteLine("Listing all subscriptions.");
            Console.WriteLine(azureCloud.ListSubscriptions());
            Console.WriteLine("Getting Subscription by Subscription Id.");
            Console.WriteLine(azureCloud.GetSubscriptionById(subscriptionId));
            #endregion

            #region Resource Group Methods
            Console.WriteLine("Listing all Resource Groups in a Subscription.");
            Console.WriteLine(azureCloud.ListResourceGroups(subscriptionId));
            Console.WriteLine("Listing Resource Group by Resource Group Name.");
            Console.WriteLine(azureCloud.GetResourceGroupByName(subscriptionId, resourceGroupName));
            Console.WriteLine("Listing all Resources in a Resource Group.");
            Console.WriteLine(azureCloud.ListResourcesInResourceGroup(subscriptionId, resourceGroupName));
            #endregion

            #region Compute Resource Provider Methods
            // Virtual Machines
            Console.WriteLine("Listing Virtual Machines in a Resource Group");
            Console.WriteLine(azureCloud.ListResourcesByNamespaceInResourceGroup(subscriptionId, resourceGroupName, azureSettings["computeNamespace"], "virtualmachines", azureSettings["computeApiVersion"]));
            Console.WriteLine("Listing Virtual machine by Name");
            Console.WriteLine(azureCloud.ListResourceByName(subscriptionId, resourceGroupName, azureSettings["computeNamespace"], "virtualmachines", virtualMachineName, azureSettings["computeApiVersion"]));
            #endregion

            #region Network Resource Provider Methods
            // Virtual Networks
            Console.WriteLine("Listing Virtual Networks in a Resource Group");
            Console.WriteLine(azureCloud.ListResourcesByNamespaceInResourceGroup(subscriptionId, resourceGroupName, azureSettings["networkNamespace"], "virtualnetworks", azureSettings["networkApiVersion"]));
            Console.WriteLine("Listing Virtual Network by Name");
            Console.WriteLine(azureCloud.ListResourceByName(subscriptionId, resourceGroupName, azureSettings["networkNamespace"], "virtualnetworks", virtualNetworkName, azureSettings["networkApiVersion"]));

            // Public IP Addresses
            Console.WriteLine("Listing Public IP Addresses in a Resource Group");
            Console.WriteLine(azureCloud.ListResourcesByNamespaceInResourceGroup(subscriptionId, resourceGroupName, azureSettings["networkNamespace"], "publicIPAddresses", azureSettings["networkApiVersion"]));
            Console.WriteLine("Listing Public IP Address by Name");
            Console.WriteLine(azureCloud.ListResourceByName(subscriptionId, resourceGroupName, azureSettings["networkNamespace"], "publicIPAddresses", publicIpAddressName, azureSettings["networkApiVersion"]));
            #endregion

            #region Usage

            Console.WriteLine("Getting Usage Aggregates");
            Console.WriteLine(azureCloud.ListUsage(subscriptionId, azureSettings["usageNamespace"], "UsageAggregates", azureSettings["usageApiVersion"], new DateTime(2015, 03, 01, 00, 00, 00), new DateTime(2015, 05, 18, 00, 00, 00)));
            #endregion
        }
    }
}
