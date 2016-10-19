using System.IO;
using Nethereum.IntegrationTesting;

namespace Ujo.Work.Service.Tests
{
    public class WorkGethTestRunner : GethTestRunner
    {
        public override string ExePath { get { return @"I:\JuanFran\Documents\Source\Repos\Nethereum\testchain"; } }
        public override string ChainDirectory { get { return Path.Combine(ExePath, "devChain"); } }
    }
}