using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.IntegrationTesting;
using Nethereum.Web3;
using Ujo.ContractRegistry.Tests;
using Ujo.IpfsImage.Services.Tests;
using Xunit;
using Ujo.Work.Service.Tests;

namespace Ujo.IntegrationTesting
{
    public class MordenIntegrationTests
    {
        private string[] works = new[]
        {
            "Jeff Mills  - The Bells (original mix).mp3",
            "Speed J - Evolution.mp3",
            "Lil Louis - French Kiss.mp3",
            "Laurent Garnier - Wake Up.mp3",
            "Carl Craig - Wrap Me In It-s Arms (Instrumental).mp3",
            "Sterac - Asphyx.mp3",
            "Vainqueur - Lyot (Original Mix).mp3",
            "Joey Beltram - Energy Flash.mp3",
            "Plastikman - spastik.mp3",
            "Choice - Acid Eiffel.mp3",
            "black dog - cost 2.mp3",
            "3MB Feat. Magic Juan Atkins - Die Kosmischen Kuriere (Original Mix).mp3",
            "3 PHASE feat Dr MOTTE - Der Klang der Familie (original mix).mp3",
            "Suburban Knight - Midnight Sunshine.mp3",
            "Plastikman - Plasticine.mp3",
            "Model 500 - No UFOs (instrumental).mp3",
            "Rhythim Is Rhythim - Strings of Life.mp3",
            "Rythim Is Rythim - Nude Photo.mp3",
            "Robert Hood - Minus Alive.mp3",
            "Lil Louis - Blackout.mp3",
            "DBX - Losing Control (Carl Craig Remix).mp3",
            "Galaxy 2 Galaxy - Hi-Tech Jazz (The Science).mp3",
            "The Aztec Mystic - Knights of the Jaguar.mp3",
            "AFX - VBS.Redlof.B.mp3",
            "Underground Resistance - Punisher.mp3"
        };

        [Fact]
        public async Task ShouldUnregisterWorks()
        {
            var workRegistryHelper = new WorkRegistryMordenTests();
            await workRegistryHelper.UnRegisterContract("0x21e41690ea026721e22f1519d34eee60e6f043b2");
        }
        

        [Fact]
        public async Task ShouldProcessWorks()
        {
            foreach (var work in works)
            {
               var address =  await ProcessWork(work);
                Debug.WriteLine(address);
            }
        }

        [Fact]
        public async Task ShouldUpdateWork()
        {
            var address = "0xadc7691c3ef7ebf80cd43de666867c5b9ed5dac4";
            var work = await UploadFile("3 PHASE feat Dr MOTTE - Der Klang der Familie (original mix).mp3");
            var workHelper = new WorkMordenTests();
            await workHelper.ShouldUpdateWorkFileInContract(address, work);
        }

        [Fact]
        public async Task<string> ProcessWorkAllFields()
        {

            var workHash = await UploadFile("summerdnb.mp3");
            //all jpg
            var imageHash = await UploadFile("The Aztec Mystic - Knights of the Jaguar.jpg");
            var workHelper = new WorkMordenTests();
            var workContract =  await workHelper.DeployContractToModernAllFieldsAsync(workHash, "Summer dub", imageHash, "Super Simon", "Dub");
            await RegisterWork(workContract);
            return workContract;
        }

        [Fact]
        public async Task<string> ProcessWorkAllFieldsArtistAddress()
        {

            var workHash = await UploadFile("summerdnb.mp3");
            //all jpg
            var imageHash = await UploadFile("The Aztec Mystic - Knights of the Jaguar.jpg");
            var workHelper = new WorkMordenTests();
            var workContract = await workHelper.DeployContractToModernAllFieldsAsync(workHash, "Summer dub", imageHash, "0x1234567890", "Dub");
            await RegisterWork(workContract);
            return workContract;
        }

        public async Task<string> ProcessWork(string work)
        {
            var workHash = "workhash"; //await UploadFile(work);
            //all jpg
            var imageHash = await UploadFile(Path.GetFileNameWithoutExtension(work) + ".jpg");
            var artistWorkArray = Path.GetFileNameWithoutExtension(work).Split('-');
            var artist = artistWorkArray[0].Trim();
            var workName = artistWorkArray[1].Trim();
            var workContract = await DeployWorkToMorden(imageHash, workHash, artist, workName);
            await RegisterWork(workContract);
            return workContract;
        }

        public async Task RegisterWork(string address)
        {
            var workRegistryHelper = new WorkRegistryMordenTests();
            await workRegistryHelper.RegisterDeployedContract(address);
        }

        public async Task<string> DeployWorkToMorden(string imageHash, string workHash, string artist, string workName)
        {
            var workHelper = new WorkMordenTests();
            return await workHelper.DeployContractToModernAsync(workHash, workName, imageHash, artist, "Techno");
        }

        public async Task<string> UploadFile(string fileName)
        {
            var ipfsTestHelper = new IpfsImageServiceTests();
            var node = await ipfsTestHelper.UploadCurrentDirectoryFileToInfura(fileName);
            return node.Hash.ToString();
        }
    }
}