using System.Collections.Generic;
using System.Threading.Tasks;
using CCC.StandardDataProcessing;
using Nethereum.RPC.Eth.Filters;
using Nethereum.Web3;
using Ujo.Work.Services.Ethereum;

namespace Ujo.Work.Services
{
    public class WorkDataChangedLogProcessor : StandardDataChangedLogProcessor<Model.Work>
    {
        public WorkDataChangedLogProcessor(Web3 web3, IStandardDataRegistry standardDataRegistry, IEnumerable<IStandardDataChangedService<Model.Work>> dataChangedServices) : base(web3, standardDataRegistry, dataChangedServices)
        {
        }

        public override Task<Model.Work> GetObjectModelAsync(string contractAddress)
        {
            var workService = new WorkService(Web3, contractAddress);
            return workService.GetWorkAsync();
        }

        public override bool IsLogForEvent(FilterLog log)
        {
            var worksService = new WorksService(Web3);
            return worksService.IsStandardDataChangeLog(log);
        }
    }
}