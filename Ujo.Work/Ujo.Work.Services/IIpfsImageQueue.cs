namespace Ujo.Work.Services
{
    public interface IIpfsImageQueue
    {
        void Add(string ipfsInageHash);
    }
}