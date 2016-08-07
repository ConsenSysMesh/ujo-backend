using System.Collections.Generic;
using Nethereum.RPC.Eth.Filters;

namespace Ujo.WorkRegistry.Service
{
    public class EventLogBlockNumberTransactionIndexComparer : IComparer<object>
    {
        //TODO: Move to Nethereum Web3
        //Workaround nethereum needs eventlog to implement IEventLog with Log so we can do as IEventLog
        public int Compare(object x, object y)
        {
            dynamic dynx = x;
            var xLog = dynx.Log as FilterLog;

            dynamic dyny = y;
            var yLog = dyny.Log as FilterLog;

            return new FilterLogBlockNumberTransactionIndexComparer().Compare(xLog, yLog);

        }
    }
}