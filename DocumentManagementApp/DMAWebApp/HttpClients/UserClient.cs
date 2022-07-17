using DataAccess.DBModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DMAWebApp.HttpClients
{
    public class UserClient
    {
        private HttpClient client;

        public UserClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);
        }

        public async Task<List<User>> GetUsersAsync()
        {

            List<User> users = new();
            HttpResponseMessage response = await client.GetAsync("https://localhost:5001/api/User");
           

            if (response.IsSuccessStatusCode)
            {
                string res = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<User>>(res);
            }
            return users;
        }
    }
}
