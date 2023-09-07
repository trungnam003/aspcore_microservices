using Shared.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Identity
{
    public interface ITokenService
    {
        TokenResponse GetToken(TokenRequest request);
    }
}
