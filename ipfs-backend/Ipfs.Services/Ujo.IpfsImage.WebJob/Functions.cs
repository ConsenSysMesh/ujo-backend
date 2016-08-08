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
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Table;
using Ujo.IpfsImage.Services;
using Wintellect.Azure.Storage.Table;

namespace Ujo.IpfsImage.WebJob
{
    public class Functions
    {

        [Singleton]
        public static async Task ProcessWorks([QueueTrigger("IpfsCoverImageProcessingQueue")] string ipfsImage, TextWriter log
            )
        {
            log.WriteLine("Start job");
            
            log.WriteLine("Processing image");
            var service = new IpfsImageService(ConfigurationSettings.GetIpfsRPCUrl());
            var image = await service.ScaleImageByHeight(ipfsImage, 200);
            var node = await service.AddImage(image, ipfsImage + "200h", ImageFormat.Png);

            //todo where to store all the info?
            //image sizes?
        }
    }
}
