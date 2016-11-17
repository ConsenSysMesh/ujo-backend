#Blockchain Processing
The blockchain processing core components provides the plugable infastructure components to process transactions / logs from the blockchain.

## Blockhain batch processing service

The blockchain batch processing is the top layer component to process a range of blocks based on the values provided by the Blockhain Process Progress Service.
The processing can be of any type that implements IBlockchainProcessor, for example event logs, or specific calls to match a specific filter.

The following diagram explains the overall relationships


