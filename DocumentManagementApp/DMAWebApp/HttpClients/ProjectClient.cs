using DataAccess.DBModels;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DMAWebApp.HttpClients
{
    public class ProjectClient
    {
        private HttpClient client;

        public ProjectClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);
        }

        public async Task<List<Project>> GetProjectAsync(string email)
        {

            List<Project> projects = new();
            HttpResponseMessage response = await client.GetAsync("https://localhost:5001/api/Project?email=" + email);
            
            if (response.IsSuccessStatusCode)
            {
                string res = await response.Content.ReadAsStringAsync();
                projects = JsonConvert.DeserializeObject<List<Project>>(res);
                Console.Write(projects);
            }
            return projects;
        }
    }
}
