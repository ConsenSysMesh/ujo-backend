using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            
        }
    }

    public class ArtistRepositoryTest
    {
        [Fact]
        public async void ShouldCreateAndFindAnArtitstUsingARepository()
        {
            var artistName = "ujoArtistRepo";
            var address = "XXX" + artistName;
            var bio = "Hi i am an artist";
            var artist = new Artist() { Name = artistName, Address = address, Bio = bio }; 

            using (var context = new UjoContext())
            {
                var unitOfWork = new UnitOfWork(context);
                var artistRepository = unitOfWork.RepositoryAsync<Artist>();
                artistRepository.Insert(artist);
                await unitOfWork.SaveChangesAsync();
            }

            using (var context = new UjoContext())
            {
                var unitOfWork = new UnitOfWork(context);
                var artistRepository = unitOfWork.RepositoryAsync<Artist>();
                var artistFound = await artistRepository.FindAsync(address);
                Assert.Equal(artist.Name, artistFound.Name);
                Assert.Equal(artist.Address, artistFound.Address);
                Assert.Equal(artist.Bio, artistFound.Bio);
            }

        }

        [Fact]
        public async void ShouldUpsertAnArtist()
        {
            var artistName = "ujoArtistRepoUpsert";
            var address = "XXX" + artistName;
            var bio = "Hi i am an artist";
            var artist = new Artist() { Name = artistName, Address = address, Bio = bio };

            using (var context = new UjoContext())
            {
                var unitOfWork = new UnitOfWork(context);
                var artistRepository = unitOfWork.RepositoryAsync<Artist>();
                artistRepository.Insert(artist);
                await unitOfWork.SaveChangesAsync();
            }

            using (var context = new UjoContext())
            {
                var unitOfWork = new UnitOfWork(context);
                var artistRepository = unitOfWork.RepositoryAsync<Artist>();
                var artistFound = await artistRepository.FindAsync(address);
                Assert.Equal(artist.Name, artistFound.Name);
                Assert.Equal(artist.Address, artistFound.Address);
                Assert.Equal(artist.Bio, artistFound.Bio);
                artistFound.Bio = "Hi i am an artist and also a fan";
                artistRepository.Update(artistFound);
                await unitOfWork.SaveChangesAsync();
            }


            using (var context = new UjoContext())
            {
                var unitOfWork = new UnitOfWork(context);
                var artistRepository = unitOfWork.RepositoryAsync<Artist>();
                var artistFound = await artistRepository.FindAsync(address);
                Assert.Equal("Hi i am an artist and also a fan", artistFound.Bio);
            }
        }
    }
}
