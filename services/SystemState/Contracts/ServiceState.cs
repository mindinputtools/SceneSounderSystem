using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ServiceState
    {
        public string ServiceName { get; }
        public bool Up { get; }
    }
}
