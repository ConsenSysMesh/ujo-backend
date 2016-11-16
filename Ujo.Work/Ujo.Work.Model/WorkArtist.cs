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
                this.Name = string.Empty; 
            }
            else
            {
                this.Name = nameOrAddress;
                this.Address = string.Empty;
            }
            this.Role = role;
            this.Index = index;   
        }

        private bool IsAddress(string nameOrAddress)
        {
            return nameOrAddress.StartsWith("0x");
        }

    }
}