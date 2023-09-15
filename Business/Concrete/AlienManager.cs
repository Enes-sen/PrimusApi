using Business.Abstract;
using Business.Contants;
using Business.Utilities.Result;
using Core1.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class AlienManager : IAlienService
    {
        private readonly IAlienDal _alienDal;
        public AlienManager(IAlienDal alienDal)
        {
            _alienDal = alienDal;
        }
        public IResult Add(Alien Alien)
        {
            _alienDal.Add(Alien);

            return new SuccessResult(Messages.AlienAdded);

        }

        public IResult Delete(Alien alien)
        {
            _alienDal.Delete(alien);
            return new SuccessResult(Messages.AlienDeleted);
        }

        public IDataResult<List<Alien>> GetList()
        {
            return new SuccessDataResult<List<Alien>>(_alienDal.GetList().ToList());
        }
    }
}
