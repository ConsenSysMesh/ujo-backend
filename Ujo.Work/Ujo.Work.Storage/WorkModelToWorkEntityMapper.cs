using Newtonsoft.Json;

namespace Ujo.Work.Storage
{
    public class WorkModelToWorkEntityMapper
    {
        public WorkEntity MapFromWorkModel(WorkEntity workEntity, Model.Work work)
        {
            workEntity.Address = work.Address ?? "";
            workEntity.Name = work.Name ?? "";
            workEntity.Creator = work.Creator ?? "";
            workEntity.WorkFileIpfsHash = work.WorkFileIpfsHash ?? "";
            workEntity.CoverFileIpfsHash = work.CoverImageIpfsHash ?? "";
            workEntity.DateCreated = work.DateCreated ?? "";
            workEntity.DateModified = work.DateModified ?? "";
            workEntity.Image = work.Image ?? "";
            workEntity.Audio = work.Audio ?? "";
            workEntity.Genre = work.Genre ?? "";
            workEntity.Keywords = work.Keywords ?? "";
            workEntity.ByArtistAddress = work.ByArtistAddress ?? "";
            workEntity.ByArtistName = work.ByArtistName ?? "";
            workEntity.FeaturedArtists = JsonConvert.SerializeObject(work.FeaturedArtists.ToArray());
            workEntity.ContributingArtists = JsonConvert.SerializeObject(work.ContributingArtists.ToArray());
            workEntity.PerformingArtists = JsonConvert.SerializeObject(work.PerformingArtists.ToArray());
            workEntity.Label = work.Label ?? "";
            workEntity.Description = work.Description ?? "";
            workEntity.Publisher = work.Publisher ?? "";
            workEntity.HasPartOf = work.HasPartOf;
            workEntity.IsPartOf = work.IsPartOf;
            workEntity.IsFamilyFriendly = work.IsFamilyFriendly;
            workEntity.License = work.License ?? "";
            workEntity.IswcCode = work.IswcCode ?? "";

            return workEntity;
        }
    }
}