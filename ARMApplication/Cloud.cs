using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ARMApplication
{
    class Cloud
    {
        private string _token;
        public string clientId { get; set; }
        public string clientSecret { get; set; }
        public string directoryTenantName { get; set; }
        public string loginEndpoint { get; set; }
        public string armResourceId { get; set; }
        public string armEndpoint { get; set; }
        public string armApiVersion { get; set; }

        public Cloud() {}

        private async Task<AuthenticationResult> AuthenticateAsync()
        {
            var cc = new ClientCredential(this.clientId, this.clientSecret);
            var context = new AuthenticationContext(String.Concat(this.loginEndpoint, this.directoryTenantName));
            var authenticationResult = await context.AcquireTokenAsync(this.armResourceId, cc);
            if (authenticationResult == null)
            {
                throw new InvalidOperationException("Could not get the token");
            }
            return authenticationResult;
        }

        public void Authenticate()
        {
            Task<AuthenticationResult> test = AuthenticateAsync();
            _token = test.Result.AccessToken;
        }

        public string GetToken() {
            return _token;
        }

        public string ListSubscriptions()
        {
            string url = String.Format("{0}subscriptions?api-version={1}",
                armEndpoint, armApiVersion);

            return CallAPI(url);
        }

        public string GetSubscriptionById(string subscriptionId)
        {
            string url = String.Format("{0}subscriptions/{1}?api-version={2}",
                armEndpoint, subscriptionId, armApiVersion);
  
            return CallAPI(url);
        }

        public string ListResourceGroups(string subscriptionId)
        {
            string url = String.Format("{0}subscriptions/{1}/resourcegroups?api-version={2}",
                armEndpoint,subscriptionId, armApiVersion);

            return CallAPI(url);
        }

        public string GetResourceGroupByName(string subscriptionId, string resourceGroupName)
        {
            string url = String.Format("{0}subscriptions/{1}/resourcegroups/{2}?api-version={3}",
                armEndpoint, subscriptionId, resourceGroupName, armApiVersion);

            return CallAPI(url);
        }

        public string ListResourcesInResourceGroup(string subscriptionId, string resourceGroupName)
        {
            string url = String.Format("{0}subscriptions/{1}/resourcegroups/{2}/resources?api-version={3}",
                armEndpoint, subscriptionId, resourceGroupName, armApiVersion);

            return CallAPI(url);
        }

        public string ListResourcesByNamespaceInResourceGroup(string subscriptionId, string resourceGroupName, string resourceNamespace, string resourceTypeName, string apiVersion)
        {
            string url = String.Format("{0}subscriptions/{1}/resourcegroups/{2}/providers/{3}/{4}?api-version={5}",
                armEndpoint, subscriptionId, resourceGroupName, resourceNamespace, resourceTypeName, apiVersion);

            return CallAPI(url);
        }

        public string ListResourceByName(string subscriptionId, string resourceGroupName, string resourceNamespace, string resourceTypeName, string resourceName, string apiVersion)
        {
            string url = String.Format("{0}subscriptions/{1}/resourcegroups/{2}/providers/{3}/{4}/{5}?api-version={6}",
                armEndpoint, subscriptionId, resourceGroupName, resourceNamespace, resourceTypeName, resourceName, apiVersion);

            return CallAPI(url);
        }

        public string CallAPI(string url, string urlParameters = "")
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            
            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call!
            return (response.IsSuccessStatusCode ? response.Content.ReadAsStringAsync().Result : String.Concat(response.StatusCode, response.ReasonPhrase));    
        }

    }
}
