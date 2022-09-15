
using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;

namespace DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly SaleDbContext _db;
        public ProductRepository(SaleDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            /*
            _db.Products.Update(product);
             * This is one way to update. But in this way it updates all the columns in the table 
               even if some of the columns are not changed. We can see the solution below.
               It s a bit longer solution, but you dont want your image to be uploaded 
               everytime you update database with the way above.
             */

            var productAcquired = _db.Products.FirstOrDefault(p => p.Id == product.Id);
            if (productAcquired!= null)
            {
                productAcquired.Title = product.Title;  
                productAcquired.ISBN = product.ISBN;
                productAcquired.Price = product.Price;
                productAcquired.Price50 = product.Price50;
                productAcquired.ListPrice = product.ListPrice;
                productAcquired.Price100 = product.Price100;
                productAcquired.Description = product.Description;
                productAcquired.CategoryId = product.CategoryId;
                productAcquired.Author = product.Author;

                if (productAcquired.ImageUrl != null)
                    productAcquired.ImageUrl = product.ImageUrl;
            }
        }
    }
}
