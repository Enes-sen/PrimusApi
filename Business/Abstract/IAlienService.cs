using Business.Utilities.Result;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IAlienService
    {
        IDataResult<List<Alien>> GetList();
        IResult Add(Alien alien);
        IResult Delete(Alien alien);
    }
}
