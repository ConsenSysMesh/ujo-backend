# Blockchain Processing

The blockchain processing component provides a pluggable infrastructure to monitor and process transactions, smart contracts  changes on state and / or  events (logs) raised.
 
For example, the continuous processing and monitoring of token transfers (Erc20)  made by a specific address, or in a more complex scenario the monitoring and processing of all the transactions made by many  token contracts (Erc20) registered in an exchange.

Processing can be of any type, storage of transaction history, indexing of data, data analytics, monitoring of payments, etc.

## Blockchain batch processing service

The blockchain batch processing is an example of a top layer processing component which can be used to continuously process blocks and delegate its processing to implementors of IBlockchainProcessor.

### Blockchain Process progress service

This simple service is used to store the latest block processed and retrieve the next range to process. There are two types of implementations, latest and child. 

The latest uses the latest block number retrieved from ethereum as the block number to process to. The previously recorded (stored using the repository) or a default configured value will be the block to process from.

There are ocassions when the processing depends of blocks depends on a previous performed process. (for example specialised log processing might depend on a registry block processing). In this scenario the child process progress can be used, using the repository of the dependent parent instead of web3.

### Blockchain Log Processor

An example of an IBlockchainProcessor is the blockchain log processor. The blockchain log processor provides a mechanism to plug processing for transaction logs. 

A filter is created to retrieve all the transaction logs for a specific to the range (provided by the batch processing service if that is the consumer) as follows:

```csharp
 var logs = await _web3.Eth.Filters.GetLogs.SendRequestAsync(new NewFilterInput
            {
                FromBlock = new BlockParameter(fromBlockNumber),
                ToBlock = new BlockParameter(toBlockNumber)
            });
```

A collection of specialised log processors will then validate if the topic belongs to them, and further processing. Physical processing (as an example storage of the data) could be done by the implementor or delegated for queue processing.
Many specialised log processors can be configured, preventing the continous connection with an Ethereum client to retrieve the same logs.


### Class diagram of overall relationships

![Blockchain Processing](blockchain-processing.png)
