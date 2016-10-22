using System.Collections.Generic;

namespace Ujo.Work.Model
{
    public class WorkArtist
    {
        public int Index { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        public WorkArtist(int index, string name, string address, string role)
        {
            this.Index = index;
            this.Address = address;
            this.Role = Role;
            this.Name = name;
        }

        public WorkArtist(int index, string nameOrAddress, string role)
        {
            if (IsAddress(nameOrAddress))
            {
                this.Address = nameOrAddress;
            }
            else
            {
                this.Name = nameOrAddress;
            }
            this.Role = role;
            this.Index = index;   
        }

        private bool IsAddress(string nameOrAddress)
        {
            return nameOrAddress.StartsWith("0x");
        }

    }

    public class Work
    {
        public Work()
        {
            this.FeaturedArtists = new List<WorkArtist>();
            this.PerformingArtists = new List<WorkArtist>();
            this.ContributingArtists = new List<WorkArtist>();
        }

        public string Address { get; set; }
        public string Name { get; set; }
        public string WorkFileIpfsHash { get; set; }
        public string CoverImageIpfsHash { get; set; }
        public string Genre { get; set; }
        public string Creator { get; set; }

        public string DateCreated { get; set; }
        public string DateModified { get; set; }
        public string Image { get; set; }
        public string Audio { get; set; }
       
        public string Keywords { get; set; }
        public string ByArtistAddress { get; set; }
        public string ByArtistName { get; set; }
        public List<WorkArtist> FeaturedArtists { get; set; }
        public List<WorkArtist> ContributingArtists { get; set; }
        public List<WorkArtist> PerformingArtists { get; set; }

        public string Label { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public bool HasPartOf { get; set; }
        public bool IsPartOf { get; set; }
        public string IsFamilyFriendly { get; set; }
        public string License { get; set; }
        public string IswcCode { get; set; }

        public void AddFeatureArtist(int index, string nameOrAddress, string role)
        {
            if(!string.IsNullOrEmpty(nameOrAddress))
            this.FeaturedArtists.Add(new WorkArtist(index, nameOrAddress, role));
        }

        public void AddContributingArtist(int index, string nameOrAddress, string role)
        {
            if (!string.IsNullOrEmpty(nameOrAddress))
                this.ContributingArtists.Add(new WorkArtist(index, nameOrAddress, role));
        }

        public void AddPerformingArtist(int index, string nameOrAddress, string role)
        {
            if (!string.IsNullOrEmpty(nameOrAddress))
                this.PerformingArtists.Add(new WorkArtist(index, nameOrAddress, role));
        }


    }
}