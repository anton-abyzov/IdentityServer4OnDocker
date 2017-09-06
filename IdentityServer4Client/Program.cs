using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace IdentityServer4Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            var authority = "http://localhost:5060";
            var discovery = new DiscoveryClient(authority);

            var disco = await discovery.GetAsync();

            //var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "topsecret");
            var tokenClient = new TokenClient(disco.TokenEndpoint, "clientmvc", "topsecret");
            //var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("alice", "password", "api1");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            var myProxy = new HttpClientHandler()
            {
                UseProxy = true,
                //Proxy = new WebProxy("http://127.0.0.1:8888")
                //Proxy = new MyProxy("http://localhost:8888")
                Proxy = new MyProxy("http://DESKTOP-A8DCCM6:8888")
            };
            //var client = new HttpClient();
            var client = new HttpClient(myProxy);

            client.SetBearerToken(tokenResponse.AccessToken);
            
            var response = await client.GetAsync("http://localhost:5077/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}