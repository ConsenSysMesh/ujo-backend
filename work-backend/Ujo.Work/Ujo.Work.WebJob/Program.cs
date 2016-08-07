//----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage;

namespace Ujo.Work.WebJob
{
    //******************************************************************************************************
    // This will show you how to perform common scenarios using the Microsoft Azure Table storage service using 
    // the Microsoft Azure WebJobs SDK. The scenarios covered include reading and writing data to Tables.
    //
    // In this sample, the Program class starts the JobHost and creates the demo data. The Functions class
    // contains methods that will be invoked when messages are placed on the queues and tables, based on the 
    // attributes in the method headers.
    //
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    //
    // TODO: Open app.config and paste your Storage connection string into the AzureWebJobsDashboard and
    //      AzureWebJobsStorage connection string settings.      
    //*****************************************************************************************************

    class Program
    {

        // The following code will write a sentence as a message on a queue called textinput.
        // The SDK will trigger a function called CountAndSplitInWords which is listening on textinput queue.
        // CountAndSplitInWords will split the sentence into words and store results in Table storage.
        // 
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
