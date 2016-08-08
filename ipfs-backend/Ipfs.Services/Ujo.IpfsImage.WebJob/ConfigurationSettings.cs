using System;
using System.Configuration;
using Microsoft.WindowsAzure;

namespace Ujo.IpfsImage.WebJob
{
    public static class ConfigurationSettings
    {
        public const string IPFS_RPC_URL_KEY = "IpfsRPCUrl";
       

        public static string GetIpfsRPCUrl()
        {
            return CloudConfigurationManager.GetSetting(IPFS_RPC_URL_KEY);
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

            if (string.IsNullOrEmpty(GetIpfsRPCUrl()))
            {
                configOK = false;
                Console.WriteLine("Please add the ipfs rpc url to the configuration");

            }

            return configOK;
        }
    }
}