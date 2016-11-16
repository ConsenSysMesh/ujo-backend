using System;
using System.Configuration;
using Microsoft.WindowsAzure;

namespace Ujo.Work.WebJob
{
    public static class ConfigurationSettings
    {
        public const string EthereumRpcUrlKey = "EthereumRPCUrl";
        public const string WorkRegistryContractAdressKey = "WorkRegistryContractAddress";
        public const string StartProcessFromBlockNumberKey = "StartProcessWorkFromBlockNumber";
        public const string SearchApiServiceNameKey = "SearchServiceName";
        public const string SearchApiSearchKey = "SearchServiceApiKey";
        public const string SearchApiAdminKey = "SearchApiAdminKey";
        public const string SearchApiWorkIndexNameKey = "SearchApiWorkIndexName";


        public static string GetEthereumRpcUrl()
        {
            return CloudConfigurationManager.GetSetting(EthereumRpcUrlKey);
        }

        public static string GetSearchApiServiceName()
        {
            return CloudConfigurationManager.GetSetting(SearchApiServiceNameKey);
        }

        public static string GetSearchApiSearchKey()
        {
            return CloudConfigurationManager.GetSetting(SearchApiSearchKey);
        }

        public static string GetSearchApiSearchAdminKey()
        {
            return CloudConfigurationManager.GetSetting(SearchApiAdminKey);
        }

        public static string GetSearchApiWorkIndexName()
        {
            return CloudConfigurationManager.GetSetting(SearchApiWorkIndexNameKey);
        }

        public static string GetWorkRegistryContractAddress()
        {
            return CloudConfigurationManager.GetSetting(WorkRegistryContractAdressKey);
        }

        public static ulong StartProcessFromBlockNumber()
        {
            var blockNubmerString = CloudConfigurationManager.GetSetting(StartProcessFromBlockNumberKey);
            if (string.IsNullOrEmpty(blockNubmerString)) return 0;
            return Convert.ToUInt64(blockNubmerString);
        }

        public static bool VerifyConfiguration()
        {
            var webJobsDashboard = ConfigurationManager.ConnectionStrings["AzureWebJobsDashboard"].ConnectionString;
            var webJobsStorage = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString;


            var configOk = true;
            if (string.IsNullOrWhiteSpace(webJobsDashboard) || string.IsNullOrWhiteSpace(webJobsStorage))
            {
                configOk = false;
                Console.WriteLine("Please add the Azure Storage account credentials in App.config");
            }

            if (string.IsNullOrEmpty(GetEthereumRpcUrl()))
            {
                configOk = false;
                Console.WriteLine("Please add the ethereum rpc url to the configuration");
            }

            if (string.IsNullOrEmpty(GetWorkRegistryContractAddress()))
            {
                configOk = false;
                Console.WriteLine("Please add the work registry contract address to the configuration");
            }


            if (string.IsNullOrEmpty(GetSearchApiSearchAdminKey()) ||
                string.IsNullOrEmpty(GetSearchApiSearchKey()) ||
                string.IsNullOrEmpty(GetSearchApiServiceName()) ||
                string.IsNullOrEmpty(GetSearchApiWorkIndexName())
            )
            {
                configOk = false;
                Console.WriteLine("Please ensure the search api keys have been set correctly");
            }

            return configOk;
        }
    }
}