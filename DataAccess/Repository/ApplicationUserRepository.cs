
using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;

namespace DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly SaleDbContext _db;

        public ApplicationUserRepository(SaleDbContext db) : base(db)
        {
            _db = db;
        }  
    }
}
