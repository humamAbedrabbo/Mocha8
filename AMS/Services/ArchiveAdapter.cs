using AMS.ViewModels.Archive;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AMS.Services
{
    public class ArchiveAdapter : IArchiveAdapter
    {
        private readonly IHttpClientFactory clientFactory;

        public ArchiveAdapter(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<DocumentDetailModel> PostDocument(DocumentAddModel doc)
        {
            
            DocumentDetailModel result = new DocumentDetailModel();
            var client = clientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(doc);
            var msg = await client.PostAsync($"http://localhost:5000/archive/docs/add", 
                new StringContent(json, Encoding.UTF8, "application/json"));
            if (msg.IsSuccessStatusCode)
            {
                var responseStream = await msg.Content.ReadAsStringAsync();
                result = Newtonsoft.Json.JsonConvert.DeserializeObject
                    <DocumentDetailModel>(responseStream);
            }
            else
            {
                result = null;
            }

            return result;
        }

        public async Task<bool> UploadChunk(ChunkAddModel doc)
        {

            bool result = false;
            var client = clientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(doc);
            var msg = await client.PostAsync($"http://localhost:5000/archive/docs/upload",
                new StringContent(json, Encoding.UTF8, "application/json"));
            if (msg.IsSuccessStatusCode)
            {
                var responseStream = await msg.Content.ReadAsStringAsync();
                result = Newtonsoft.Json.JsonConvert.DeserializeObject
                    <bool>(responseStream);
            }
            else
            {
                result = false;
            }

            return result;
        }
    }
}
