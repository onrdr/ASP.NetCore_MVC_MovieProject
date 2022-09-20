
using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;

namespace DataAccess.Repository
{ 
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly SaleDbContext _db;

        public CompanyRepository(SaleDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company company)
        {
            _db.Companies.Update(company);
        }
    }
}
