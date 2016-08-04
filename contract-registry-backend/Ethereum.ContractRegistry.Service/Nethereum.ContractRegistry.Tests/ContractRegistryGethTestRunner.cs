using System.IO;

namespace Nethereum.ContractRegistry.Tests
{
    public class ContractRegistryGethTestRunner : GethTestRunner
    {
        public override string ExePath { get { return @"C:\Users\JuanFran\Source\Repos\Nethereum\testchain"; } }
        public override string ChainDirectory { get { return Path.Combine(ExePath, "devChain"); } }
    }
}