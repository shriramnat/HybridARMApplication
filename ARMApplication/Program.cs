using System;
using System.Configuration;

namespace ARMApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Cloud azureCloud = new Cloud
            {
                clientId = ConfigurationManager.AppSettings["clientId"],
                clientSecret = ConfigurationManager.AppSettings["clientSecret"],
                loginEndpoint = "https://login.windows.net/",
                directoryTenantName = "microsoft.onmicrosoft.com",
                armEndpoint = "https://management.azure.com/",
                armResourceId = "https://management.azure.com/",
                armApiVersion = "2016-09-01"
            };

            // Authenticate to the specific Cloud's Resource Manager.
            azureCloud.Authenticate();

            // Console.WriteLine(azureCloud.GetToken());
            string subscriptionId = "";
            string resourceGroupName = "";

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
            string computeApiVersion = "2017-03-30";
            string computeNamespace = "Microsoft.Compute";
            
            // Virtual Machines
            Console.WriteLine("Listing Virtual Machines in a Resource Group");
            Console.WriteLine(azureCloud.ListResourcesByNamespaceInResourceGroup(subscriptionId, resourceGroupName, computeNamespace, "virtualmachines", computeApiVersion));
            Console.WriteLine("Listing Virtual machine by Name");
            Console.WriteLine(azureCloud.ListResourceByName(subscriptionId, resourceGroupName, computeNamespace, "virtualmachines", "shriqpab7-mn0", computeApiVersion));
            #endregion

            #region Network
            string networkApiVersion = "2016-12-01";
            string networkNamespace = "Microsoft.Network";

            // Virtual Networks
            Console.WriteLine("Listing Virtual Networks in a Resource Group");
            Console.WriteLine(azureCloud.ListResourcesByNamespaceInResourceGroup(subscriptionId, resourceGroupName, networkNamespace, "virtualnetworks", networkApiVersion));
            Console.WriteLine("Listing Virtual Network by Name");
            Console.WriteLine(azureCloud.ListResourceByName(subscriptionId, resourceGroupName, networkNamespace, "virtualnetworks", "shriqpab7vnet", networkApiVersion));

            // Public IP Addresses
            Console.WriteLine("Listing Public IP Addresses in a Resource Group");
            Console.WriteLine(azureCloud.ListResourcesByNamespaceInResourceGroup(subscriptionId, resourceGroupName, networkNamespace, "publicIPAddresses", networkApiVersion));
            Console.WriteLine("Listing Public IP Address by Name");
            Console.WriteLine(azureCloud.ListResourceByName(subscriptionId, resourceGroupName, networkNamespace, "publicIPAddresses", "shriqpab7-publicip", networkApiVersion));
            #endregion
        }
    }
}
