using ProductProject.Service.UserService.Dto;

namespace ProductProject.Service.UserService;

public interface IUserService
{
    Task Register(RegisterDto dto);
    Task<TokenDto> Login(LoginDto dto);
}