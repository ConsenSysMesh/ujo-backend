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
using Ujo.Messaging;
using Ujo.Model;
using Ujo.Repository.Infrastructure;

namespace Ujo.Repository
{
    public class MusicRecordingService:IStandardDataProcessingService<MusicRecordingDTO>
    {
        private UnitOfWork _unitOfWork;
        private UjoContext _ujoContext;

        public MusicRecordingService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task UpsertAsync(MusicRecordingDTO work)
        {
            var originalWork = await FindAsync(work.Address).ConfigureAwait(false);
            
            if (originalWork != null)
            {
                //Because we don't have ids that relates to OtherArtists we are just dumping the original ones and adding new ones
                foreach (var otherArtist in originalWork.OtherArtists)
                {
                    otherArtist.ObjectState = ObjectState.Deleted;
                    //work.OtherArtists.Add(otherArtist);
                }
                //Then we do a map to reflect the changes
                //We may consider a DTO then map and update the graph
                Mapper.Map(work, originalWork);
                originalWork.ObjectState = ObjectState.Modified;
                _unitOfWork.RepositoryAsync<MusicRecording>().InsertOrUpdateGraph(originalWork);
            }
            else
            {
                var newWork = new MusicRecording();
                Mapper.Map(work, newWork);
                _unitOfWork.RepositoryAsync<MusicRecording>().InsertOrUpdateGraph(newWork);
            }
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }


        public async Task<MusicRecording> FindAsync(string address)
        {
            var repository = _unitOfWork.RepositoryAsync<MusicRecording>();
            var query = repository.Query(x => x.Address == address).Include(x => x.OtherArtists);
            var found = await query.SelectAsync().ConfigureAwait(false);      
            return found.SingleOrDefault();
        }

        public async Task<bool> ExistsAsync(string contractAddress)
        {
            return (await FindAsync(contractAddress).ConfigureAwait(false)) != null;
        }

        public async Task RemovedAsync(string contractAddress)
        {
            await _unitOfWork.RepositoryAsync<MusicRecording>().DeleteAsync(contractAddress).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DataChangedAsync(MusicRecordingDTO model, EventLog<DataChangedEvent> dataEventLog)
        {
            await UpsertAsync(model).ConfigureAwait(false);
        }
    }   
}
