using System.Linq;
using Newtonsoft.Json;
using Ujo.Messaging;

namespace Ujo.Work.Storage
{
    public class WorkModelToWorkEntityMapper
    {
        public WorkEntity MapFromWorkModel(WorkEntity workEntity, MusicRecordingDTO work)
        {
            workEntity.Address = work.Address ?? "";
            workEntity.Name = work.Name ?? "";
            workEntity.Creator = work.Creator ?? "";
            workEntity.DateCreated = work.DateCreated ?? "";
            workEntity.DateModified = work.DateModified ?? "";
            workEntity.Image = work.Image ?? "";
            workEntity.Audio = work.Audio ?? "";
            workEntity.Genre = work.Genre ?? "";
            workEntity.Keywords = work.Keywords ?? "";
            workEntity.ByArtistAddress = work.ByArtistAddress ?? "";
            workEntity.ByArtistName = work.ArtistName ?? "";
            workEntity.FeaturedArtists = JsonConvert.SerializeObject(work.GetFeaturedArtists().ToArray());
            workEntity.ContributingArtists = JsonConvert.SerializeObject(work.GetContributingArtists().ToArray());
            workEntity.PerformingArtists = JsonConvert.SerializeObject(work.GetPerformingArtists().ToArray());
            workEntity.Label = work.Label ?? "";
            workEntity.Description = work.Description ?? "";
            workEntity.Publisher = work.Publisher ?? "";
            workEntity.HasPartOf = work.HasPart ?? false;
            workEntity.IsPartOf = work.IsPartOf ?? false;
            workEntity.IsFamilyFriendly = work.IsFamilyFriendly ?? "";
            workEntity.License = work.License ?? "";
            workEntity.IswcCode = work.IswcCode ?? "";

            return workEntity;
        }
    }
}