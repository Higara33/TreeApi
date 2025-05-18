using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public class TimeProvider : ITimeProvider
    {
        public DateTimeOffset Now => DateTimeOffset.Now;
    }
}
