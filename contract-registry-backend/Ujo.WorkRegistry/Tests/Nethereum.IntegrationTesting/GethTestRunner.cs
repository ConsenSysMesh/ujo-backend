using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Nethereum.IntegrationTesting
{
    public abstract class GethTestRunner
    {
        public abstract string ExePath { get; }
        public abstract string ChainDirectory { get; }

        public Process Process { get; private set; }

        public string Arguments { get; set; }

        private string defaultArguments =
            @" --rpc --networkid=39318 --maxpeers=0 --datadir=devChain  --rpccorsdomain ""*"" --rpcapi ""eth,web3,personal,net,miner,admin"" --ipcapi ""eth,web3,personal,net,miner,admin"" --verbosity 0 console";

        private string initArguments = " --datadir=devChain init genesis_dev.json";


        public void CleanUp()
        {
            DeleteChainDirectory("chaindata");
            DeleteChainDirectory("dapp");
            DeleteChainDirectory("nodes");
            InitGeth();
        }

        public void DeleteChainDirectory(string name)
        {
            var path = Path.Combine(ChainDirectory, name);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public void InitGeth()
        {
            ProcessStartInfo psi = new ProcessStartInfo(Path.Combine(ExePath, "geth.exe"), initArguments)
            {
                CreateNoWindow = false,
                WindowStyle = ProcessWindowStyle.Normal,
                UseShellExecute = true,
                WorkingDirectory = ExePath

            };
            Process.Start(psi);
            Thread.Sleep(5000);
        }


        public void StartGeth()
        {
            
            var args = string.IsNullOrEmpty(Arguments) ? defaultArguments : Arguments;
            ProcessStartInfo psi = new ProcessStartInfo(Path.Combine(ExePath, "geth.exe"), args)
            {
                CreateNoWindow = false,
                WindowStyle = ProcessWindowStyle.Normal,
                UseShellExecute = true,
                WorkingDirectory = ExePath
                
            };
            Process = Process.Start(psi);
        }

        public void StopGeth()
        {
            if (Process != null && !Process.HasExited)
            {
                Process.Kill();
                Process = null;
            }
        }
    }
}