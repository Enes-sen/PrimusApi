using Business.Utilities.Result;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> Register(UserRegisterDto userRegisterDto);
        IDataResult<User> Login(UserLoginDto userLoginDto);
        IResult UserExists(string userName);
    }
}
