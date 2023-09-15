using Business.Abstract;
using Business.Utilities.Result;
using Business.Contants;
using Core1.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        public AuthManager(IUserService userService)
        {
            _userService = userService;

        }
        public IDataResult<User> Login(UserLoginDto userLoginDto)
        {
            User UserCheck = _userService.GetByEmail(userLoginDto.UserName);
            return UserCheck == null
                ? new ErrorDataResult<User>(message: Messages.UserNotFound)
                : UserCheck.passwordhash == null || UserCheck.passwordsalt == null
                ? new ErrorDataResult<User>(UserCheck, message: Messages.PasswordHashSaltMissing)
                : !Verifyhash(password: userLoginDto.Password, passwordHash: UserCheck.passwordhash, passwordSalt: UserCheck.passwordsalt)
                ? new ErrorDataResult<User>(message: Messages.PasswordError)
                : new SuccessDataResult<User>(UserCheck, Messages.SuccessfullyLogged);

        }


        public IDataResult<User> Register(UserRegisterDto userRegisterDto)
        {

            CreatePasswordHash(userRegisterDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            User user = new()
            {
                username = userRegisterDto.UserName,
                passwordhash = passwordHash,
                passwordsalt = passwordSalt
            };
            _userService.Add(user);
            return new SuccessDataResult<User>(user, Messages.UserRegistered);

        }

        public IResult UserExists(string UserName)
        {
            return _userService.GetByEmail(UserName) != null ? new ErrorResult(Messages.UserExists) : new SuccessResult();
        }
        private static bool Verifyhash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using System.Security.Cryptography.HMACSHA512 hmac = new(passwordSalt);
            byte[] computedhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedhash.Length; i++)
            {
                if (computedhash[i] != passwordHash[i])
                {
                    return false;
                }

            }
            return true;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using System.Security.Cryptography.HMACSHA512 hmac = new();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
