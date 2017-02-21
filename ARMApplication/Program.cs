using System;
using System.Collections.Specialized;
using System.Configuration;

namespace ARMApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string subscriptionId = "";
            string resourceGroupName = "";

            var azureSettings = (NameValueCollection) ConfigurationManager.GetSection("azureAppSettings");
            Cloud azureCloud = new Cloud
            {
                clientId = azureSettings["clientId"],
                clientSecret = azureSettings["clientSecret"],
                loginEndpoint = azureSettings["loginEndpoint"],
                directoryTenantName = azureSettings["directoryTenantName"],
                armEndpoint = azureSettings["armEndpoint"],
                armResourceId = azureSettings["armResourceId"],
                armApiVersion = azureSettings["armApiVersion"]
            };

            // Authenticate to the specific Cloud's Resource Manager.
            azureCloud.Authenticate();
            // Console.WriteLine(azureCloud.GetToken());
           
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

            #region Compute
            // Virtual Machines
            Console.WriteLine("Listing Virtual Machines in a Resource Group");
            Console.WriteLine(azureCloud.ListResourcesByNamespaceInResourceGroup(subscriptionId, resourceGroupName, azureSettings["computeNamespace"], "virtualmachines", azureSettings["computeApiVersion"]));
            Console.WriteLine("Listing Virtual machine by Name");
            Console.WriteLine(azureCloud.ListResourceByName(subscriptionId, resourceGroupName, azureSettings["computeNamespace"], "virtualmachines", "shriqpab7-mn0", azureSettings["computeApiVersion"]));
            #endregion

            #region Network
            // Virtual Networks
            Console.WriteLine("Listing Virtual Networks in a Resource Group");
            Console.WriteLine(azureCloud.ListResourcesByNamespaceInResourceGroup(subscriptionId, resourceGroupName, azureSettings["networkNamespace"], "virtualnetworks", azureSettings["networkApiVersion"]));
            Console.WriteLine("Listing Virtual Network by Name");
            Console.WriteLine(azureCloud.ListResourceByName(subscriptionId, resourceGroupName, azureSettings["networkNamespace"], "virtualnetworks", "shriqpab7vnet", azureSettings["networkApiVersion"]));

            // Public IP Addresses
            Console.WriteLine("Listing Public IP Addresses in a Resource Group");
            Console.WriteLine(azureCloud.ListResourcesByNamespaceInResourceGroup(subscriptionId, resourceGroupName, azureSettings["networkNamespace"], "publicIPAddresses", azureSettings["networkApiVersion"]));
            Console.WriteLine("Listing Public IP Address by Name");
            Console.WriteLine(azureCloud.ListResourceByName(subscriptionId, resourceGroupName, azureSettings["networkNamespace"], "publicIPAddresses", "shriqpab7-publicip", azureSettings["networkApiVersion"]));
            #endregion
        }
    }
}
