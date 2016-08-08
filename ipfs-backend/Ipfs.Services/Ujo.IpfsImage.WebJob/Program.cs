using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage;

namespace Ujo.IpfsImage.WebJob
{

    class Program
    {

        static void Main()
        {
            if (!ConfigurationSettings.VerifyConfiguration())
            {
                Console.ReadLine();
                return;
            }

            JobHostConfiguration config = new JobHostConfiguration();
            config.UseTimers();
            var host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}
