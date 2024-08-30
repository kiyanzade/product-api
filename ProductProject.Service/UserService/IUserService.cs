using Microsoft.AspNetCore.Identity;
using ProductProject.Service.UserService.Dto;

namespace ProductProject.Service.UserService;

public interface IUserService
{
    Task<IdentityResult> Register(RegisterDto dto);
    Task<TokenDto> Login(LoginDto dto);
}