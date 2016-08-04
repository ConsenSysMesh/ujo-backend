using System.IO;
using Nethereum.IntegrationTesting;

namespace Ujo.WorkRegistry.Service.Tests
{
    public class WorkRegistryGethTestRunner : GethTestRunner
    {
        public override string ExePath { get { return @"C:\Users\JuanFran\Source\Repos\Nethereum\testchain"; } }
        public override string ChainDirectory { get { return Path.Combine(ExePath, "devChain"); } }
    }
}