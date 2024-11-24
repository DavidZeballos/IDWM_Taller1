using IDWM_TallerAPI.Src.Models;

namespace IDWM_TallerAPI.Src.Interfaces.Service
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}