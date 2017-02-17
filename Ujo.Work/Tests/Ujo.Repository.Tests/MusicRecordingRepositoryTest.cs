using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ujo.Messaging;
using Ujo.Model;
using Ujo.Repository.Infrastructure;
using Xunit;

namespace Ujo.Repository.Tests
{

    public class MusicRecordingMappingTests
    {
        [Fact]
        public void ShouldBeAbleToMapDTOToModel()
        {
            MappingBootstrapper.Initialise();
            Mapper.AssertConfigurationIsValid();

            var address = "XXXX";
            var musicRecording = new MusicRecordingDTO();
            musicRecording.Address = address;
            musicRecording.Name = "my great track";
            var musicRecordingModel = new MusicRecording();
            Mapper.Map(musicRecording, musicRecordingModel);
            Assert.Equal(musicRecordingModel.Address, address);
        }

        [Fact]
        public void ShouldBeAbleToMapOtherArtitsDTOToModel()
        {
            MappingBootstrapper.Initialise();
            Mapper.AssertConfigurationIsValid();

            var address = "XXXX";
            var otherArtitsAddress = "ArtistXXXXX";
            var musicRecording = new MusicRecordingDTO();
            musicRecording.Address = address;
            musicRecording.OtherArtists.Add(new CreativeWorkArtistDTO() {ArtistAddres = otherArtitsAddress});
            var musicRecordingModel = new MusicRecording();
            Mapper.Map(musicRecording, musicRecordingModel);
            Assert.Equal(musicRecordingModel.Address, address);
            Assert.Equal(musicRecordingModel.OtherArtists.First().ArtistAddres, otherArtitsAddress);
        }
    }


    public class MusicRecordingServiceTest
    {
        [Fact]
        public async Task ShouldBeAbleToUpsertMusicRecordingsIncludingOtherArtists()
        {
            MappingBootstrapper.Initialise();

            var address = "XXXX";
            var musicRecording = new MusicRecordingDTO();
            musicRecording.Address = address;
            musicRecording.Name = "my great track";
            //musicRecording.ObjectState = ObjectState.Added;
            musicRecording.OtherArtists.Add(new CreativeWorkArtistDTO() {ContributionType = "Featured", CreativeWorkAddress = address, NonRegisteredArtistName = "JB" , Role = "Tambourine"});

            using (var context = new UjoContext())
            {
                var unitOfWork = new UnitOfWork(context);
                var musicRecordingService = new MusicRecordingService(unitOfWork);
                await musicRecordingService.UpsertAsync(musicRecording).ConfigureAwait(false);
            }

            using (var context = new UjoContext())
            using (var unitOfWork = new UnitOfWork(context))
            {
                var musicRecordingService = new MusicRecordingService(unitOfWork);
                var recordingOutput = await musicRecordingService.FindAsync(address).ConfigureAwait(false);
                Assert.Equal(musicRecording.Name, recordingOutput.Name);
                Assert.Equal(musicRecording.OtherArtists.ToList()[0].NonRegisteredArtistName, recordingOutput.OtherArtists.ToList()[0].NonRegisteredArtistName);
            }

           
            var musicRecording2 = new MusicRecordingDTO();
            musicRecording2.Address = address;
            musicRecording2.Name = "my great track 2";
           // musicRecording2.ObjectState = ObjectState.Added;
            musicRecording2.OtherArtists.Add(new CreativeWorkArtistDTO() { ContributionType = "Featured", CreativeWorkAddress = address, NonRegisteredArtistName = "JB2", Role = "Tambourine"});

            using (var context = new UjoContext())
            using (var unitOfWork = new UnitOfWork(context))
            {
                var musicRecordingService = new MusicRecordingService(unitOfWork);
                await musicRecordingService.UpsertAsync(musicRecording2).ConfigureAwait(false);
            }

            using (var context = new UjoContext())
            using (var unitOfWork = new UnitOfWork(context))
            {
                var musicRecordingService = new MusicRecordingService(unitOfWork);
                var recordingOutput = await musicRecordingService.FindAsync(address).ConfigureAwait(false);
                Assert.Equal(musicRecording2.Name, recordingOutput.Name);
                Assert.Equal(1, recordingOutput.OtherArtists.Count);
                Assert.Equal(musicRecording2.OtherArtists.ToList()[0].NonRegisteredArtistName, recordingOutput.OtherArtists.ToList()[0].NonRegisteredArtistName);
            }
        }
    }
}