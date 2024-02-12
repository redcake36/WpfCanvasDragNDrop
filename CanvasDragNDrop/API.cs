using CanvasDragNDrop.APIClases;
using CanvasDragNDrop.Windows.BlockModelCreationWindow.Classes;
using CanvasDragNDrop.Windows.MainWindow.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace CanvasDragNDrop
{
    public static class API
    {

        //public static string rootServer = "https://3369-81-177-58-204.ngrok-free.app";
        public static string rootServer = "http://91.103.252.95:3101";

        public static bool AutomotiveWork = false;

        public static string MockDataFolder = "MockAPIData/";

        /// <summary> Метод парсинга ответа от сервера и формирования объекта данных </summary>
        public static (T? data, bool isSuccess) GenerateResponse<T>(string json, bool isSuccess)
        {
            T Data = default;
            bool IsSuccess = isSuccess;
            if (!IsSuccess)
            {
                return (Data, IsSuccess);
            }
            try
            {
                Data = JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                IsSuccess = false;
            }
            return (Data, IsSuccess);
        }

        /// <summary> Метод GET запроса к серверу </summary>
        public static (string data, bool isSuccess) GetRequest(string path)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var response = httpClient.GetStringAsync(rootServer + path);
                    var data = response.GetAwaiter().GetResult();
                    return (data, true);
                }

            }
            catch (Exception ex)
            {
                return (ex.Message, false);
            }

        }

        /// <summary> Метод POST запроса к серверу </summary>
        public static (string data, bool isSuccess) PostRequest(string path, string data)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var encodedContent = new StringContent(data, Encoding.Unicode, "application/json");
                    var response = httpClient.PostAsync(rootServer + path, encodedContent);
                    var results = response.GetAwaiter().GetResult();
                    return (results.Content.ReadAsStringAsync().GetAwaiter().GetResult(), results.IsSuccessStatusCode);
                }

            }
            catch (Exception ex)
            {
                return (ex.Message, false);
            }

        }

        /// <summary> Запрос получения каталога моделей блоков </summary>
        public static (List<APIDirBlockModelClass> catalogModels, bool isSuccess) GetCatalogs()
        {
            if (AutomotiveWork)
            {
                return GenerateResponse<List<APIDirBlockModelClass>>(File.ReadAllText(MockDataFolder + "get_catalogs.json"), true);
            }

            var requestResult = GetRequest("/get_model_catalog");
            return GenerateResponse<List<APIDirBlockModelClass>>(requestResult.data, requestResult.isSuccess);
        }

        /// <summary> Запрос получения моделей блоков </summary>
        public static (List<APIBlockModelClass> blockModels, bool isSuccess) GetBlockModels()
        {
            if (AutomotiveWork)
            {
                return GenerateResponse<List<APIBlockModelClass>>(File.ReadAllText(MockDataFolder + "get_models.json"), true);
            }

            var requestResult = GetRequest("/get_models");
            return GenerateResponse<List<APIBlockModelClass>>(requestResult.data, requestResult.isSuccess);
        }


        /// <summary> Запрос получения типов сред и списка базовых параметров </summary>
        public static (APIGetEnvsResponseClass environments, bool isSuccess) GetEnvironments()
        {
            if (AutomotiveWork)
            {
                return GenerateResponse<APIGetEnvsResponseClass>(File.ReadAllText(MockDataFolder + "get_envs.json"), true);
            }

            var requestResult = GetRequest("/get_envs");
            return GenerateResponse<APIGetEnvsResponseClass>(requestResult.data, requestResult.isSuccess);
        }

        /// <summary> Запрос получения типов сред и списка базовых параметров </summary>
        public static (List<APISchemeClass> Schemes, bool isSuccess) GetAllSchemes()
        {
            if (AutomotiveWork)
            {
                return GenerateResponse<List<APISchemeClass>>(File.ReadAllText(MockDataFolder + "show_all_schemes.json"), true);
            }

            var requestResult = GetRequest("/show_all_schemes");
            return GenerateResponse<List<APISchemeClass>>(requestResult.data, requestResult.isSuccess);
        }

        /// <summary> Запрос на создание модели блока </summary>
        public static (string response, bool isSuccess) CreateBlockModel(BlockModelCreationClass BlockModel)
        {
            string JSON = JsonConvert.SerializeObject(BlockModel);
            //MessageBox.Show(JSON);
            return PostRequest("/create_model", JSON);
        }

        public static (string response, bool isSuccess) SaveSchema(SchemaClass schemaClass)
        {
            string JSON = JsonConvert.SerializeObject(schemaClass);
            //MessageBox.Show(JSON);
            return PostRequest("/create_scheme", JSON);
        }



    }
}
