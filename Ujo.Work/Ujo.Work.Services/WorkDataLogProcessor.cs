using System.Collections.Generic;
using System.Threading.Tasks;
using CCC.Contracts.StandardData.Processing;
using Nethereum.Web3;
using Ujo.Work.Search.Service;
using Ujo.Work.Services.Ethereum;
using Ujo.Work.Storage;

namespace Ujo.Work.Services
{
    public class WorkDataLogProcessor : StandardDataLogProcessor<Model.Work>
    {
        public WorkDataLogProcessor(Web3 web3, IStandardDataRegistry standardDataRegistry, IEnumerable<IStandardDataProcessingService<Model.Work>> dataChangedServices) : base(web3, standardDataRegistry, dataChangedServices)
        {
        }

        public override Task<Model.Work> GetObjectModelAsync(string contractAddress)
        {
            var workService = new WorkService(Web3, contractAddress);
            return workService.GetWorkAsync();
        }

    

        public static WorkDataLogProcessor Create(Web3 web3, IStandardDataRegistry dataRegistry,
            IIpfsImageQueue imageQueue, WorkRepository workRepository, WorkSearchService workSearchService)
        {
            var services = new List<IStandardDataProcessingService<Model.Work>>();
            services.Add(workRepository);
            services.Add(workSearchService);
            services.Add(new WorkIpfsImagesStandardDataProcessingService(imageQueue));
            return new WorkDataLogProcessor(web3, dataRegistry, services);
        }
    }


}