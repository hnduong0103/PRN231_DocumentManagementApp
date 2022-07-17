using DataAccess.DBModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DMAWebApp.HttpClients
{
    public class DocumentClient
    {
        private HttpClient client;
        public DocumentClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);
        }

        public async Task<List<Document>> GetDocumentAsync(string email, int page)
        {

            List<Document> documents = new();
            HttpResponseMessage response = await client.GetAsync("https://localhost:5001/api/Document?email="+ email + "&page=" + page);

            if (response.IsSuccessStatusCode)
            {
                string res = await response.Content.ReadAsStringAsync();
                documents = JsonConvert.DeserializeObject<List<Document>>(res);
            }
            return documents;
        }
    }
}
