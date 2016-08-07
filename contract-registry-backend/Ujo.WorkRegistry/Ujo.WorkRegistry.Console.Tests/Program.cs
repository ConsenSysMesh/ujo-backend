using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ujo.WorkRegistry.Console.Tests.Strategies;


namespace Ujo.WorkRegistry.Console.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var deployContract = new DeployContract();
            var contractAddress = deployContract.DeployContractAsync().Result;
        }
    }
}
