using System.Collections.Generic;
using System.Threading.Tasks;
using CCC.Contracts.StandardData.Processing;
using Nethereum.Web3;
using Ujo.Messaging;
using Ujo.Repository;
using Ujo.Work.Search.Service;
using Ujo.Work.Services.Ethereum;
using Ujo.Work.Storage;

namespace Ujo.Work.Services
{
    public class WorkDataLogProcessor : StandardDataLogProcessor<MusicRecordingDTO>
    {
        public WorkDataLogProcessor(Web3 web3, IStandardDataRegistry standardDataRegistry, IEnumerable<IStandardDataProcessingService<MusicRecordingDTO>> dataChangedServices) : base(web3, standardDataRegistry, dataChangedServices)
        {
        }

        public override Task<MusicRecordingDTO> GetObjectModelAsync(string contractAddress)
        {
            var workService = new WorkService(Web3, contractAddress);
            return workService.GetMusicRecordingAsync();
        }

        public static WorkDataLogProcessor Create(Web3 web3, 
            IStandardDataRegistry dataRegistry,
            IIpfsImageQueue imageQueue, 
            WorkRepository workRepository, 
            WorkSearchService workSearchService,
            MusicRecordingService musicRecordingService
            )
        {
            var services = new List<IStandardDataProcessingService<MusicRecordingDTO>>();
            services.Add(workRepository);
            services.Add(workSearchService);
            services.Add(new WorkIpfsImagesStandardDataProcessingService(imageQueue));
            services.Add(musicRecordingService);

            return new WorkDataLogProcessor(web3, dataRegistry, services);
        }
    }
}