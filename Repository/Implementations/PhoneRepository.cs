using Database.Data;
using Database.Models;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class PhoneRepository : GenericRepository<Phone>, IPhoneRepository
    {
        public PhoneRepository(AppDbContext context) : base(context)
        {
        }
    }
}
