# ujo-backend-spike

Initial spike (prototype) for a backend in Azure to process blockchain data, events etc, store them and index the information for searching.

## Components

[Blockchain Service](https://github.com/ConsenSys/ujo-backend-spike/tree/master/UjoSpike.Service)
Service wrapper for a contract, including event, function calls.

[Console Application (Helper)](https://github.com/ConsenSys/ujo-backend-spike/tree/master/UjoSpike.ArtistWriter.Console)
Application with different helper methods to
* Deploy the contract
* Populate with artists
* Retrive the artists

[Simple Contract](https://github.com/ConsenSys/ujo-backend-spike/tree/master/contracts)
A simple artist contract to register Artists

[Web Job](https://github.com/ConsenSys/ujo-backend-spike/tree/master/UjoSpike.WebJob)
The web job pulls the information from the contract and stores it in an Azure Table Storage
Deployed to https://manage.windowsazure.com/@andrewkeysconsensys.onmicrosoft.com#Workspaces/WebsiteExtension/Website/ujobackendspike/jobs
* Configured to use a timer (runs every minute)
* Connects to a public rpc (Augur in Morden)
* Checks for the current processed and writes new artists added to Azure Table storage
Account: ujostorage
Table: ArtistEntity
NOTE Configuration settings are held in Azure

TODO:
Integrate Azure Search with Table Storage
Sample web site to do the search
Queueing to HdInsight and PowerBI ...


