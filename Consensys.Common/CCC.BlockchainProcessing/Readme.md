# Blockchain Processing
The blockchain processing core components provides the plugable infastructure components to process transactions / logs from the blockchain.

## Blockchain batch processing service

The blockchain batch processing is the top layer component to process a range of blocks based on the values provided by the Blockhain Process Progress Service.
The processing can be of any type that implements IBlockchainProcessor, for example event logs, or specific calls to match a specific filter.

The following diagram explains the overall relationships.

![Blockchain Processing](blockchain-processing.png)

### Blockchain Log Processor

The blockchain log processor provides a mechanism to plug processing for transaction logs. This mainly uses a filter to get all the transaction logs specific to the range (provided by the batch processing service if that is the consumer) as follows:

```csharp
 var logs = await _web3.Eth.Filters.GetLogs.SendRequestAsync(new NewFilterInput
            {
                FromBlock = new BlockParameter(fromBlockNumber),
                ToBlock = new BlockParameter(toBlockNumber)
            });
```

A collection of specialised log processors will then validate if the topic belongs to them, and further processing. Physical processing (as an example storage of the data) could be done by the implementor or delegated for queue processing.

### Blockchain Process progress

There are two types of process progress implementations, latest and child. 

The latest uses the latest block number retrieved from ethereum as the block number to process to. The previously recorded (stored using the repository) or a default configured value will be the block to process from.

There are ocassions when the processing depends of blocks depends on a previous performed process. (for example specialised log processing might depend on a registry block processing). In this scenario the child process progress can be used, using the repository of the dependent parent instead of web3.
