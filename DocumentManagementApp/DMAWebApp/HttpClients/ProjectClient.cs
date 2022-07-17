using DataAccess.DBModels;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Project>> GetProjectAsync(string email)
        {

            List<Project> projects = new();
            HttpResponseMessage response = await client.GetAsync("https://localhost:5001/api/Project?email=" + email);
            
            if (response.IsSuccessStatusCode)
            {
                string res = await response.Content.ReadAsStringAsync();
                projects = JsonConvert.DeserializeObject<List<Project>>(res);
            }
            return projects;
        }

        public async Task<string> CreateProjectAsync(string projectName, string email)
        {
            var payload = new {
                projectName = projectName
            };

            var stringPayload = JsonConvert.SerializeObject(payload);
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:5001/api/Project?email=" + email, httpContent);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
    }
}
