using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public interface ITimeProvider
    {
        DateTimeOffset Now { get; }
    }
}
