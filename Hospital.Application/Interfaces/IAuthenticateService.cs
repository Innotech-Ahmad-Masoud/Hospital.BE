using Hospital.Application.Models.Authenticate;
using Hospital.Application.Results;
using Hospital.Application.Results.Authenticate;

namespace Hospital.Application.Interfaces
{
    public interface IAuthenticateService
    {
        Task<Response<LoginResult>> Login(LoginModel model);

        Task<Response<RegisterResult>> Register(RegisterModel model);
    }
}
