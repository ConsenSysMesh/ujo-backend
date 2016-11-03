using System;
using Microsoft.Azure.WebJobs;

namespace Ujo.Work.WebJob
{
    internal class Program
    {
        private static void Main()
        {
            if (!ConfigurationSettings.VerifyConfiguration())
            {
                Console.ReadLine();
                return;
            }

            var config = new JobHostConfiguration();
            config.UseTimers();
            var host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}