using System.Threading.Tasks;

namespace CCC.Contracts
{
    public interface IByteCodeMatcher
    {
        Task<bool> IsMatchAsync(string address);
    }
}