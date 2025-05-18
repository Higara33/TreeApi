using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public record RemeberMeRequest(string Code);
    public record TokenResponse(string AccessToken, string RefreshToken, DateTime RefreshTokenExpires);
}
