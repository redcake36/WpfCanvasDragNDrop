using CanvasDragNDrop.APIClases;
using CanvasDragNDrop.Windows.BlockModelCreationWindow.Classes;
using CanvasDragNDrop.Windows.MainWindow.Classes;
using CanvasDragNDrop.Windows.ModelsExplorer.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;

namespace CanvasDragNDrop
{
    public static class API
    {
        public const string SettingsFile = "Settings.json";
        public struct StoreDataFormat
        {
            public string Address;
            public int Port;
            public string UserName;
            public string Password;
        }

        public static string Address { get; set; } = "http://91.103.252.95";
        public static int Port { get; set; } = 3101;
        public static string Username { get; set; } = string.Empty;
        public static string Password { get; set; } = string.Empty;

        //private static StoreDataFormat Settings = new StoreDataFormat() {};

        //public static string rootServer = "https://4805-79-111-219-20.ngrok-free.app";
        //public static string rootServer = "http://91.103.252.95:3101";

        public static string rootServer { get => $"{Address}:{Port}"; }

        public static bool AutomotiveWork = false;

        public static string MockDataFolder = "MockAPIData/";

        public const int Timeout = 10;



        static API()
        {
            LoadSettings();
        }

        static void LoadSettings()
        {
            if (File.Exists(SettingsFile))
            {
                string readSettings = File.ReadAllText(SettingsFile);
                StoreDataFormat Parsed = JsonConvert.DeserializeObject<StoreDataFormat>(readSettings);
                Address = Parsed.Address;
                Port = Parsed.Port;
                Username = Parsed.UserName;
                Password = Parsed.Password;
                return;
            }
            StoreSettings();
        }

        public static void StoreSettings()
        {
            StoreDataFormat defaultData = new StoreDataFormat();
            defaultData.Address = Address;
            defaultData.Port = Port;
            defaultData.UserName = Username;
            defaultData.Password = Password;
            File.WriteAllText(SettingsFile, JsonConvert.SerializeObject(defaultData));
        }

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
                    httpClient.Timeout = TimeSpan.FromSeconds(Timeout);
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
        public static (APIDirBlockModelClass catalogModels, bool isSuccess) GetCatalogs()
        {
            if (AutomotiveWork)
            {
                return GenerateResponse<APIDirBlockModelClass>(File.ReadAllText(MockDataFolder + "get_catalogs.json"), true);
            }

            //var requestResult = GetRequest("/get_model_catalog");
            var requestResult = GetRequest("/get_version_catalog");
            return GenerateResponse<APIDirBlockModelClass>(requestResult.data, requestResult.isSuccess);
        }

        /// <summary> Запрос получения моделей блоков </summary>
        public static (List<APIBlockModelVersionClass> blockModels, bool isSuccess) GetBlockModels()
        {
            if (AutomotiveWork)
            {
                return GenerateResponse<List<APIBlockModelVersionClass>>(File.ReadAllText(MockDataFolder + "get_models.json"), true);
            }

            var requestResult = GetRequest("/get_models");
            return GenerateResponse<List<APIBlockModelVersionClass>>(requestResult.data, requestResult.isSuccess);
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
        public static (List<APISchemeClass> Schemas, bool isSuccess) GetAllSchemas()
        {
            if (AutomotiveWork)
            {
                return GenerateResponse<List<APISchemeClass>>(File.ReadAllText(MockDataFolder + "show_all_schemes.json"), true);
            }

            var requestResult = GetRequest("/show_all_schemas");
            return GenerateResponse<List<APISchemeClass>>(requestResult.data, requestResult.isSuccess);
        }

        /// <summary> Запрос получения типов сред и списка базовых параметров </summary>
        public static (SchemaClass Schemas, bool isSuccess) GetSchema(int schemaId)
        {
            //if (AutomotiveWork)
            //{
            //    return GenerateResponse<List<APISchemeClass>>(File.ReadAllText(MockDataFolder + "show_all_schemes.json"), true);
            //}

            var requestResult = GetRequest($"/show_schema/{schemaId}");
            return GenerateResponse<SchemaClass>(requestResult.data, requestResult.isSuccess);
        }

        /// <summary> Запрос на создание модели блока </summary>
        public static (string response, bool isSuccess) CreateBlockModel(BlockModelCreationClass BlockModel)
        {
            string JSON = JsonConvert.SerializeObject(BlockModel);
            //MessageBox.Show(JSON);
            return PostRequest("/create_model", JSON);
        }
        /// <summary> Запрос на перемещение модели между папками </summary>
        public static (string response, bool isSuccess) MovingBlockModel(DropAndTargetId macId)
        {
            string JSON = JsonConvert.SerializeObject(macId);
            MessageBox.Show("Перемещение " + JSON);
            return PostRequest("/??", JSON);
        }

        /// <summary> Запрос на удаление модели или каталога </summary>
        public static (string response, bool isSuccess) DeleteBlockModel(DropAndTargetId macId)
        {
            string JSON = JsonConvert.SerializeObject(macId);
            MessageBox.Show("Удаление " + JSON);
            return PostRequest("/??", JSON);
        }

        /// <summary> Запрос на создание нового каталога </summary>
        public static (string response, bool isSuccess) AddNewCatalog(DropAndTargetId macId)
        {
            string JSON = JsonConvert.SerializeObject(macId);
            MessageBox.Show("Создание папки в " + JSON);
            return PostRequest("/??", JSON);
        }

        /// <summary> Запрос на создание новой схемы </summary>
        public static (string response, bool isSuccess) CreateSchema(SchemaClass schemaClass)
        {
            string JSON = JsonConvert.SerializeObject(schemaClass);
            MessageBox.Show(JSON);
            return PostRequest("/create_schema", JSON);
        }

        /// <summary> Запрос на получени еверсии модели по е id </summary>
        public static (APIBlockModelVersionClass blockModelVersion, bool isSuccess) GetModelVersion(int VersionId)
        {
            var requestResult = GetRequest($"/get_model/{VersionId}");
            return GenerateResponse<APIBlockModelVersionClass>(requestResult.data, requestResult.isSuccess);
        }


    }
}
