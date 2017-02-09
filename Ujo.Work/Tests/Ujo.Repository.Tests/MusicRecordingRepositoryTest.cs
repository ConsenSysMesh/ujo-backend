using System.Linq;
using Ujo.Model;
using Ujo.Repository.Infrastructure;
using Xunit;

namespace Ujo.Repository.Tests
{
    public class MusicRecordingRepositoryTest
    {
        [Fact]
        public async void ShouldBeAbleToUpsertMusicRecordingsIncludingOtherArtists()
        {
            var address = "XXXX";
            var musicRecording = new MusicRecording();
            musicRecording.Address = address;
            musicRecording.Name = "my great track";
            musicRecording.ObjectState = ObjectState.Added;
            musicRecording.OtherArtists.Add(new CreativeWorkArtist() {ContributionType = "Featured", CreativeWorkAddress = address, NonRegisteredArtistName = "JB" , Role = "Tambourine", ObjectState = ObjectState.Added});

            using (var context = new UjoContext())
            using (var unitOfWork = new UnitOfWork(context)) { 
                var musicRecordingService = new MusicRecordingService(unitOfWork);
                await musicRecordingService.UpsertAsync(musicRecording);
            }

            using (var context = new UjoContext())
            using (var unitOfWork = new UnitOfWork(context))
            {
                var musicRecordingService = new MusicRecordingService(unitOfWork);
                var recordingOutput = await musicRecordingService.FindAsync(address);
                Assert.Equal(musicRecording.Name, recordingOutput.Name);
                Assert.Equal(musicRecording.OtherArtists.ToList()[0].NonRegisteredArtistName, recordingOutput.OtherArtists.ToList()[0].NonRegisteredArtistName);
            }

           
            var musicRecording2 = new MusicRecording();
            musicRecording2.Address = address;
            musicRecording2.Name = "my great track 2";
            musicRecording2.ObjectState = ObjectState.Added;
            musicRecording2.OtherArtists.Add(new CreativeWorkArtist() { ContributionType = "Featured", CreativeWorkAddress = address, NonRegisteredArtistName = "JB2", Role = "Tambourine", ObjectState = ObjectState.Added });

            using (var context = new UjoContext())
            using (var unitOfWork = new UnitOfWork(context))
            {
                var musicRecordingService = new MusicRecordingService(unitOfWork);
                await musicRecordingService.UpsertAsync(musicRecording2);
            }

            using (var context = new UjoContext())
            using (var unitOfWork = new UnitOfWork(context))
            {
                var musicRecordingService = new MusicRecordingService(unitOfWork);
                var recordingOutput = await musicRecordingService.FindAsync(address);
                Assert.Equal(musicRecording2.Name, recordingOutput.Name);
                Assert.Equal(1, recordingOutput.OtherArtists.Count);
                Assert.Equal(musicRecording2.OtherArtists.ToList()[0].NonRegisteredArtistName, recordingOutput.OtherArtists.ToList()[0].NonRegisteredArtistName);
            }
        }
    }
}