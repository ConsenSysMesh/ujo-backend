using System;
using System.IO;
using System.Threading.Tasks;

namespace CCC.BlockchainProcessing
{
    public interface IBlockchainProcessor
    {
        Task ProcessAsync(ulong fromBlockNumber, ulong toBlockNumber);
    }
}