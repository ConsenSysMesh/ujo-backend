using System;
using Nethereum.IntegrationTesting;

namespace Ujo.Work.Service.Tests
{
    public class GethServerFixture : IDisposable
    {
        public GethTestRunner GethTestRunner { get; private set; }
        public Nethereum.Web3.Web3 GetWeb3()
        {
            return new Nethereum.Web3.Web3();
        }

        public GethServerFixture()
        {
            Init();
        }

        public virtual void Init()
        {
            GethTestRunner = new WorkGethTestRunner();
            GethTestRunner.CleanUp();
            GethTestRunner.StartGeth();
        }

        public void Dispose()
        {
            GethTestRunner.StopGeth();
        }
    }
}