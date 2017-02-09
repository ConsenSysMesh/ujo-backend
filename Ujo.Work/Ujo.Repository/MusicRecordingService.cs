using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CCC.Contracts.StandardData.Processing;
using CCC.Contracts.StandardData.Services.Model;
using Nethereum.Web3;
using Ujo.Model;
using Ujo.Repository.Infrastructure;

namespace Ujo.Repository
{
    public class MusicRecordingService:IStandardDataProcessingService<Ujo.Model.MusicRecording>
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly UjoContext _ujoContext;

        public MusicRecordingService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task UpsertAsync(MusicRecording work)
        {
            var originalWork = await FindAsync(work.Address);
            
            //This should be somewhere else when initialising the generic (Sql) service

            Mapper.Initialize(cfg => {
                cfg.CreateMap<MusicRecording, MusicRecording>();
                
            });

            if (originalWork != null)
            {
                //Because we don't have ids that relates to OtherArtists we are just dumping the original ones and adding new ones
                foreach (var otherArtist in originalWork.OtherArtists)
                {
                    otherArtist.ObjectState = ObjectState.Deleted;
                    work.OtherArtists.Add(otherArtist);
                }
                //Then we do a map to reflect the changes
                //We may consider a DTO then map and update the graph
                Mapper.Map(work, originalWork);
                originalWork.ObjectState = ObjectState.Modified;
                _unitOfWork.RepositoryAsync<MusicRecording>().InsertOrUpdateGraph(originalWork);
            }
            else
            {
                _unitOfWork.RepositoryAsync<MusicRecording>().InsertOrUpdateGraph(work);
            }
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task<MusicRecording> FindAsync(string address)
        {
            var repository = _unitOfWork.RepositoryAsync<MusicRecording>();
            var query = repository.Query(x => x.Address == address).Include(x => x.OtherArtists);
            var found = await query.SelectAsync();      
            return found.SingleOrDefault();
        }

        public async Task<bool> ExistsAsync(string contractAddress)
        {
            return (await FindAsync(contractAddress)) != null;
        }

        public async Task RemovedAsync(string contractAddress)
        {
            await _unitOfWork.RepositoryAsync<MusicRecording>().DeleteAsync(contractAddress);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DataChangedAsync(MusicRecording model, EventLog<DataChangedEvent> dataEventLog)
        {
            await UpsertAsync(model);
        }
    }   
}
