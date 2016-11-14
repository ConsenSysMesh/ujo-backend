using System.Collections.Generic;
using System.Threading.Tasks;
using CCC.StandardDataProcessing;
using Nethereum.RPC.Eth.Filters;
using Nethereum.Web3;
using Ujo.Search.Service;
using Ujo.Work.Services.Ethereum;
using Ujo.Work.Storage;

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

        public static WorkDataChangedLogProcessor Create(Web3 web3, IStandardDataRegistry dataRegistry,
            IIpfsImageQueue imageQueue, WorkRepository workRepository, WorkSearchService workSearchService)
        {
            var services = new List<IStandardDataChangedService<Model.Work>>();
            services.Add(workRepository);
            services.Add(workSearchService);
            services.Add(new WorkIpfsImagesStandardDataChangedService(imageQueue));
            return new WorkDataChangedLogProcessor(web3, dataRegistry, services);
        }
    }


}