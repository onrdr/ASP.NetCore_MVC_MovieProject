
namespace DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        public IShoppingCartRepository ShoppingCartRepository { get; }
        public IApplicationUserRepository ApplicationUserRepository { get; }
        public IOrderDetailRepository OrderDetailRepository { get; }
        public IOrderHeaderRepository OrderHeaderRepository { get; }

        void Save();
    }
}
