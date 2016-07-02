using Newtonsoft.Json.Linq;

namespace Ethereum.BlockchainStore.Services
{
    public class VmStackErrorChecker
    {
        public bool HasError(JObject stack)
        {
            return !string.IsNullOrEmpty(GetError(stack));
        }

        public string GetError(JObject stack)
        {
            var structsLogs = (JArray) stack["structLogs"];
            var lastCall = structsLogs[structsLogs.Count - 1];
            return lastCall["error"].Value<string>();
        }
    }
}