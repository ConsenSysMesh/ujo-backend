using System;
using System.Text;
using System.Threading.Tasks;

namespace Ujo.WorkRegistry.Service.Tests.xUnitPriority
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestPriorityAttribute : Attribute
    {
        public TestPriorityAttribute(int priority)
        {
            Priority = priority;
        }

        public int Priority { get; private set; }
    }
}
