using System;
using System.Collections.Specialized;
using System.Configuration;

namespace ARMApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var loginInformation = (NameValueCollection)ConfigurationManager.GetSection("azureAppSettings/loginInformation");
            var resourceProviders = (NameValueCollection)ConfigurationManager.GetSection("azureAppSettings/resourceProviders");
            var resourceNames = (NameValueCollection)ConfigurationManager.GetSection("azureAppSettings/resourceNames");

            Cloud azureCloud = new Cloud
            {
                clientId = loginInformation["clientId"], // Change this value in app.config
                clientSecret = loginInformation["clientSecret"], // Change this value in app.config
                loginEndpoint = loginInformation["loginEndpoint"],
                directoryTenantName = loginInformation["directoryTenantName"],
                armEndpoint = loginInformation["armEndpoint"],
                armResourceId = loginInformation["armResourceId"],
                armApiVersion = resourceProviders["armApiVersion"]
            };

            // Authenticate to the specific Cloud's Resource Manager.
            azureCloud.Authenticate();
            // Console.WriteLine(azureCloud.GetToken());

            #region Subscription Methods
            Console.WriteLine("Listing all subscriptions.");
            Console.WriteLine(azureCloud.ListSubscriptions());
            Console.WriteLine("Getting Subscription by Subscription Id.");
            Console.WriteLine(azureCloud.GetSubscriptionById(resourceNames["subscriptionId"]));
            #endregion

            #region Resource Group Methods
            Console.WriteLine("Listing all Resource Groups in a Subscription.");
            Console.WriteLine(azureCloud.ListResourceGroups(resourceNames["subscriptionId"]));
            Console.WriteLine("Listing Resource Group by Resource Group Name.");
            Console.WriteLine(azureCloud.GetResourceGroupByName(resourceNames["subscriptionId"], resourceNames["resourceGroupName"]));
            Console.WriteLine("Listing all Resources in a Resource Group.");
            Console.WriteLine(azureCloud.ListResourcesInResourceGroup(resourceNames["subscriptionId"], resourceNames["resourceGroupName"]));
            #endregion

            #region Compute Resource Provider Methods
            // Virtual Machines
            Console.WriteLine("Listing Virtual Machines in a Resource Group");
            Console.WriteLine(azureCloud.ListResourcesByNamespaceInResourceGroup(resourceNames["subscriptionId"], resourceNames["resourceGroupName"], resourceProviders["computeNamespace"], "virtualmachines", resourceProviders["computeApiVersion"]));
            Console.WriteLine("Listing Virtual machine by Name");
            Console.WriteLine(azureCloud.ListResourceByName(resourceNames["subscriptionId"], resourceNames["resourceGroupName"], resourceProviders["computeNamespace"], "virtualmachines", resourceNames["virtualMachineName"], resourceProviders["computeApiVersion"]));
            #endregion

            #region Network Resource Provider Methods
            // Virtual Networks
            Console.WriteLine("Listing Virtual Networks in a Resource Group");
            Console.WriteLine(azureCloud.ListResourcesByNamespaceInResourceGroup(resourceNames["subscriptionId"], resourceNames["resourceGroupName"], resourceProviders["networkNamespace"], "virtualnetworks", resourceProviders["networkApiVersion"]));
            Console.WriteLine("Listing Virtual Network by Name");
            Console.WriteLine(azureCloud.ListResourceByName(resourceNames["subscriptionId"], resourceNames["resourceGroupName"], resourceProviders["networkNamespace"], "virtualnetworks", resourceNames["virtualNetworkName"], resourceProviders["networkApiVersion"]));

            // Public IP Addresses
            Console.WriteLine("Listing Public IP Addresses in a Resource Group");
            Console.WriteLine(azureCloud.ListResourcesByNamespaceInResourceGroup(resourceNames["subscriptionId"], resourceNames["resourceGroupName"], resourceProviders["networkNamespace"], "publicIPAddresses", resourceProviders["networkApiVersion"]));
            Console.WriteLine("Listing Public IP Address by Name");
            Console.WriteLine(azureCloud.ListResourceByName(resourceNames["subscriptionId"], resourceNames["resourceGroupName"], resourceProviders["networkNamespace"], "publicIPAddresses", resourceNames["publicIpAddressName"], resourceProviders["networkApiVersion"]));
            #endregion

            #region Usage

            Console.WriteLine("Getting Usage Aggregates");
            Console.WriteLine(azureCloud.ListUsage(resourceNames["subscriptionId"], resourceProviders["usageNamespace"], "UsageAggregates", resourceProviders["usageApiVersion"], new DateTime(2015, 03, 01, 00, 00, 00), new DateTime(2015, 05, 18, 00, 00, 00)));
            #endregion
        }
    }
}
