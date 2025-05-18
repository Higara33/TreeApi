using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public record NodeDto(long Id, string Name, List<NodeDto> Children);
}
