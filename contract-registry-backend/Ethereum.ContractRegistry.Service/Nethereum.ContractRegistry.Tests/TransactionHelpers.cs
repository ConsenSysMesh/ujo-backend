using System;
using System.Threading;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.ContractRegistry.Tests
{
    public class TransactionHelpers
    {
        public async Task<string> DeployContract(Web3.Web3 web3, string addressFrom, string pass, string bytecode)
        {
            var  receipt = await SendAndMineTransactionAsync(web3, addressFrom, pass, () => web3.Eth.DeployContract.SendRequestAsync(bytecode, addressFrom, new HexBigInteger(900000)));
            return receipt.ContractAddress;
        }

        public async Task<TransactionReceipt> SendAndMineTransactionAsync(Web3.Web3 web3, string addressFrom,
            string pass, Func<Task<string>> transactionTask)
        {
            var result = await web3.Personal.UnlockAccount.SendRequestAsync(addressFrom, pass, new HexBigInteger(600000));
            if (result != true) throw new Exception("Acccount not unlocked");

            var trasaction = await transactionTask();

            result = await web3.Miner.Start.SendRequestAsync(6);
            if (result != true) throw new Exception("Could not start mining");
            var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(trasaction);

            while (receipt == null)
            {
                Thread.Sleep(5000);
                receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(trasaction);
            }

            result = await web3.Miner.Stop.SendRequestAsync();
            if (result != true) throw new Exception("Could not stop mining");
            return receipt;
        }

    }
}