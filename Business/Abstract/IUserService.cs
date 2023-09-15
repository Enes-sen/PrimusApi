using Entities.Concrete;

namespace Business.Abstract
{
    public interface IUserService
    {
        void Add(User user);
        User GetByEmail(string userName);

    }
}
