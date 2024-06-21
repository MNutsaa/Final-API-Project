using Microsoft.AspNetCore.Identity;

namespace Forum.Contracts
{
    public interface IJwtGenerator
    {
        string GenerateToken(IdentityUser applicationUser, IEnumerable<string> roles);
    }
}
