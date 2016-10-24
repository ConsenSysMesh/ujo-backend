using System;
using System.Configuration;
using Microsoft.WindowsAzure;

namespace Ujo.Work.WebJob
{
    public static class ConfigurationSettings
    {
        public const string ETHEREUM_RPC_URL_KEY = "EthereumRPCUrl";
        public const string WORK_REGISTRY_CONTRACT_ADRESS_KEY = "WorkRegistryContractAddress";
        public const string START_PROCESS_FROM_BLOCK_NUMBER_KEY = "StartProcessWorkFromBlockNumber";
        public const string SEARCH_API_SERVICE_NAME_KEY = "SearchServiceName";
        public const string SEARCH_API_SEARCH_KEY = "SearchServiceApiKey";
        public const string SEARCH_API_ADMIN_KEY = "SearchApiAdminKey";
        public const string SEARCH_API_WORK_INDEX_NAME_KEY = "SearchApiWorkIndexName";


        public static string GetEthereumRPCUrl()
        {
            return CloudConfigurationManager.GetSetting(ETHEREUM_RPC_URL_KEY);
        }

        public static string GetSearchApiServiceName()
        {
            return CloudConfigurationManager.GetSetting(SEARCH_API_SERVICE_NAME_KEY);
        }

        public static string GetSearchApiSearchKey()
        {
            return CloudConfigurationManager.GetSetting(SEARCH_API_SEARCH_KEY);
        }

        public static string GetSearchApiSearchAdminKey()
        {
            return CloudConfigurationManager.GetSetting(SEARCH_API_ADMIN_KEY);
        }

        public static string GetSearchApiWorkIndexName()
        {
            return CloudConfigurationManager.GetSetting(SEARCH_API_WORK_INDEX_NAME_KEY);
        }

        public static string GetWorkRegistryContractAddress()
        {
            return CloudConfigurationManager.GetSetting(WORK_REGISTRY_CONTRACT_ADRESS_KEY);
        }

        public static long StartProcessFromBlockNumber()
        {
            var blockNubmerString = CloudConfigurationManager.GetSetting(START_PROCESS_FROM_BLOCK_NUMBER_KEY);
            if (string.IsNullOrEmpty(blockNubmerString)) return 0;
            return Convert.ToInt64(blockNubmerString);
        }

        public static bool VerifyConfiguration()
        {
            string webJobsDashboard = ConfigurationManager.ConnectionStrings["AzureWebJobsDashboard"].ConnectionString;
            string webJobsStorage = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString;


            bool configOK = true;
            if (string.IsNullOrWhiteSpace(webJobsDashboard) || string.IsNullOrWhiteSpace(webJobsStorage))
            {
                configOK = false;
                Console.WriteLine("Please add the Azure Storage account credentials in App.config");

            }

            if (string.IsNullOrEmpty(GetEthereumRPCUrl()))
            {
                configOK = false;
                Console.WriteLine("Please add the ethereum rpc url to the configuration");

            }

            if (string.IsNullOrEmpty(GetWorkRegistryContractAddress()))
            {
                configOK = false;
                Console.WriteLine("Please add the work registry contract address to the configuration");
            }


            if (string.IsNullOrEmpty(GetSearchApiSearchAdminKey()) || 
                string.IsNullOrEmpty(GetSearchApiSearchKey()) || 
                string.IsNullOrEmpty(GetSearchApiServiceName()) || 
                string.IsNullOrEmpty(GetSearchApiWorkIndexName()) 
                )
            {
                configOK = false;
                Console.WriteLine("Please ensure the search api keys have been set correctly");
            }

            return configOK;
        }
    }
}