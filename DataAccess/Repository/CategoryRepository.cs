
using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;

namespace DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly SaleDbContext _db;

        public CategoryRepository(SaleDbContext db) : base(db)
        {
            _db = db;
        } 

        public void Update(Category category)
        {
            _db.Categories.Update(category);
        }
    }
}
