using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace CanvasDragNDrop
{
    public static class API
    {

        public static string rootServer = "https://0c67-81-177-58-204.ngrok-free.app";

        public static bool AutomotiveWork = true;

        public static (string data, bool isSuccess) GetRequest(string path)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var response = httpClient.GetStringAsync(API.rootServer + path);
                    response.Wait();
                    return (response.Result, true);
                }

            }
            catch (Exception ex)
            {
                return ("", false);
            }

        }

        public static (List<DBBlockModelClass> blockModels, bool isSuccess) GetBlockModels()
        {
            var result = (data:new List<DBBlockModelClass>(), isSuccess:false);
            if (!AutomotiveWork)
            {
                var requestResult = GetRequest("/get_models");
                if (requestResult.isSuccess)
                {
                    result.data = JsonConvert.DeserializeObject<List<DBBlockModelClass>>(requestResult.data);
                    result.isSuccess = true;
                }
            }
            else
            {
                result.data = JsonConvert.DeserializeObject<List<DBBlockModelClass>>(File.ReadAllText("element.json"));
                result.isSuccess = true;
            }
            return result;
        }

    }
}
