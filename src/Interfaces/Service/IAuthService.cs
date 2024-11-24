using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.DTOs;

namespace IDWM_TallerAPI.Src.Interfaces.Service
{
    public interface IAuthService
    {
        Task<string> RegisterUser(RegisterUserDto registerUserDto);

        Task<string> LoginUser(LoginDto loginDto);

    }
}