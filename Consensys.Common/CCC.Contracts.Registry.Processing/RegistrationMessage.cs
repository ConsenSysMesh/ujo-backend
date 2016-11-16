namespace CCC.Contracts.Registry.Processing
{
    public class RegistrationMessage
    {
        /// <summary>
        /// Registered if true, Unregistered if false
        /// </summary>
        public bool Registered { get; set; }
        public string Address { get; set; }
    }
}