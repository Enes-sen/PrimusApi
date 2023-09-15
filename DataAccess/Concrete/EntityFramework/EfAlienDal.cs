using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfAlienDal : EfEntityRepositoryBase<Alien, ApiContext>, IAlienDal
    {

    }
}
