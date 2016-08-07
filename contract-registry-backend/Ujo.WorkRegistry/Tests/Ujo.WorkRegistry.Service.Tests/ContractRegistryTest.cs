using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.IntegrationTesting;
using Nethereum.Web3;
using Ujo.WorkRegistry.Service;
using Ujo.WorkRegistry.Service.Tests;
using Ujo.WorkRegistry.Service.Tests.xUnitPriority;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Ujo.ContractRegistry.Tests
{
    [TestCaseOrderer("Ujo.WorkRegistry.Service.Tests.xUnitPriority.PriorityOrderer", "Ujo.WorkRegistry.Service.Tests")]
    public class ContractRegistryTest:IClassFixture<DeployedContractFixture>
    {
        private readonly DeployedContractFixture deployedContractFixture;
        private static TransactionHelpers txHelper = new TransactionHelpers();
        private static HexBigInteger defaultGas = new HexBigInteger(900000);


        public ContractRegistryTest(DeployedContractFixture deployedContractFixture)
        {
            this.deployedContractFixture = deployedContractFixture;
        }


        [Fact, TestPriority(1)]
        public async Task Should_1_RegisterContract()
        {
            var web3 = deployedContractFixture.GetWeb3();
            var contractRegistryService = GetWorkRegistryService(web3);
            var registeredEvent =  contractRegistryService.GetRegisteredEvent();
            var filter = await registeredEvent.CreateFilterAsync();

            await txHelper.SendAndMineTransactionAsync(web3, DefaultSettings.AddressFrom, DefaultSettings.Password,
                () => contractRegistryService.RegisterAsync(DefaultSettings.AddressFrom, DefaultSettings.AddressFrom, defaultGas));

            var eventLogs = await registeredEvent.GetFilterChanges<RegisteredEvent>(filter);
            Assert.Equal(1, eventLogs[0].Event.Id);
            Assert.Equal(DefaultSettings.AddressFrom, eventLogs[0].Event.RegisteredAddress);
            Assert.Equal(DefaultSettings.AddressFrom, eventLogs[0].Event.Owner);
            var address = await contractRegistryService.GetWorkRegisteredAsyncCall(1);
            Assert.Equal(DefaultSettings.AddressFrom, address);
        }

        private WorkRegistryService GetWorkRegistryService(Web3 web3)
        {
            var contractAddress = deployedContractFixture.ContractAddress;
            //Given a contract is deployed
            Assert.False(string.IsNullOrEmpty(contractAddress));

            var contractRegistryService = new WorkRegistryService(web3, contractAddress);
            return contractRegistryService;
        }


        [Fact, TestPriority(2)]
        public async Task Should_2_RetrieveNumberOfRegisteredWork()
        {
            var web3 = deployedContractFixture.GetWeb3();
            var contractRegistryService = GetWorkRegistryService(web3);
            var numberOfRecords = await contractRegistryService.NumRecordsAsyncCall();
            Assert.Equal(1, numberOfRecords);
        }

       
        [Fact, TestPriority(4)]
        public async Task Should_4_BeAbleToRegisterManyAddresses()
        {
            var address1 = "0xb794f5ea0ba39494ce839613fffba74279579268";
            var address2 = "0xe853c56864a2ebe4576a807d26fdc4a0ada51919";
            var address3 = "0xab7c74abc0c4d48d1bdad5dcb26153fc8780f83e";
            var address4 = "0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae";

            var web3 = deployedContractFixture.GetWeb3();
            var contractRegistryService = GetWorkRegistryService(web3);
            var registeredEvent = contractRegistryService.GetRegisteredEvent();
            var filter = await registeredEvent.CreateFilterAsync();

             var receipts = await txHelper.SendAndMineTransactionsAsync(web3, 
                                                       DefaultSettings.AddressFrom, 
                                                       DefaultSettings.Password,
                         () => contractRegistryService.RegisterAsync(DefaultSettings.AddressFrom, 
                                                                    address1, 
                                                                    defaultGas),
                         () => contractRegistryService.RegisterAsync(DefaultSettings.AddressFrom,
                                                                    address2,
                                                                    defaultGas),
                         () => contractRegistryService.RegisterAsync(DefaultSettings.AddressFrom,
                                                                    address3,
                                                                    defaultGas),
                         () => contractRegistryService.RegisterAsync(DefaultSettings.AddressFrom,
                                                                    address4,
                                                                    defaultGas));

          

            var eventLogs = await registeredEvent.GetFilterChanges<RegisteredEvent>(filter);
            Assert.Equal(4, eventLogs.Count);
        }

        [Fact, TestPriority(5)]
        public async Task Should_5_BeAbleToGetRegisteredFromBlockNumber()
        {
            var address1 = "0xc794f5ea0ba39494ce839613fffba74279579268";
            var address2 = "0xc853c56864a2ebe4576a807d26fdc4a0ada51919";
            var address3 = "0xcb7c74abc0c4d48d1bdad5dcb26153fc8780f83e";
            var address4 = "0xce0b295669a9fd93d5f28d9ec85e40f4cb697bae";

            var web3 = deployedContractFixture.GetWeb3();
            var contractRegistryService = GetWorkRegistryService(web3);
            var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();

            var receipts = await txHelper.SendAndMineTransactionsAsync(web3,
                                                      DefaultSettings.AddressFrom,
                                                      DefaultSettings.Password,
                        () => contractRegistryService.RegisterAsync(DefaultSettings.AddressFrom,
                                                                   address1,
                                                                   defaultGas),
                        () => contractRegistryService.RegisterAsync(DefaultSettings.AddressFrom,
                                                                   address2,
                                                                   defaultGas),
                        () => contractRegistryService.RegisterAsync(DefaultSettings.AddressFrom,
                                                                   address3,
                                                                   defaultGas),
                        () => contractRegistryService.RegisterAsync(DefaultSettings.AddressFrom,
                                                                   address4,
                                                                   defaultGas));


            var eventLogs = await contractRegistryService.GetRegisteredFromBlockNumber(blockNumber);
            Assert.Equal(4, eventLogs.Count);


            blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();

            receipts = await txHelper.SendAndMineTransactionsAsync(web3,
                                                      DefaultSettings.AddressFrom,
                                                      DefaultSettings.Password,
                        () => contractRegistryService.UnregisterAsync(DefaultSettings.AddressFrom,
                                                                   address1,
                                                                   defaultGas),
                        () => contractRegistryService.UnregisterAsync(DefaultSettings.AddressFrom,
                                                                   address2,
                                                                   defaultGas),
                        () => contractRegistryService.UnregisterAsync(DefaultSettings.AddressFrom,
                                                                   address3,
                                                                   defaultGas),
                        () => contractRegistryService.UnregisterAsync(DefaultSettings.AddressFrom,
                                                                   address4,
                                                                   defaultGas));

            var uneventLogs = await contractRegistryService.GetUnregisteredFromBlockNumber(blockNumber);
            Assert.Equal(4, uneventLogs.Count);

        }


        [Fact, TestPriority(6)]
        public async Task Should_6_BeAbleToGetRegisteredUnregisteredInOrderFromBlockNumber()
        {
            var address1 = "0xc794f5ea0ba39494ce839613fffba74279579268";
            var address2 = "0xc853c56864a2ebe4576a807d26fdc4a0ada51919";
            var address3 = "0xcb7c74abc0c4d48d1bdad5dcb26153fc8780f83e";
            var address4 = "0xce0b295669a9fd93d5f28d9ec85e40f4cb697bae";

            var web3 = deployedContractFixture.GetWeb3();
            var contractRegistryService = GetWorkRegistryService(web3);
            var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();

            var receipts = await txHelper.SendAndMineTransactionsAsync(web3,
                                                      DefaultSettings.AddressFrom,
                                                      DefaultSettings.Password,
                        () => contractRegistryService.RegisterAsync(DefaultSettings.AddressFrom,
                                                                   address1,
                                                                   defaultGas),
                        () => contractRegistryService.RegisterAsync(DefaultSettings.AddressFrom,
                                                                   address2,
                                                                   defaultGas),
                        () => contractRegistryService.UnregisterAsync(DefaultSettings.AddressFrom,
                                                                   address2,
                                                                   defaultGas),
                        () => contractRegistryService.RegisterAsync(DefaultSettings.AddressFrom,
                                                                   address3,
                                                                   defaultGas),
                        () => contractRegistryService.UnregisterAsync(DefaultSettings.AddressFrom,
                                                                   address1,
                                                                   defaultGas),
                        () => contractRegistryService.RegisterAsync(DefaultSettings.AddressFrom,
                                                                   address4,
                                                                   defaultGas));
                        


            var eventLogs = await contractRegistryService.GetRegisteredUnregisteredFromBlockNumber(blockNumber);
            Assert.Equal(6, eventLogs.Count);
            var unregistered2 = eventLogs[2] as EventLog<UnregisteredEvent>;
            Assert.NotNull(unregistered2);
            Assert.Equal(address2, unregistered2.Event.RegisteredAddress);
           
        }

    }
}