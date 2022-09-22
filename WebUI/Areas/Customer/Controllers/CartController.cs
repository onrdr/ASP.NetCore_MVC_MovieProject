using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models;
using Models.ViewModel;
using System.Security.Claims;

namespace WebUI.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public int OrderTotal { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCartRepository.GetAll(c => c.ApplicationUserId == claim.Value,
                includeProperties: "Product"),
            };
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price,
                    cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.CartTotal += cart.Price * cart.Count;
            }
            return View(ShoppingCartVM);
        }

        public IActionResult Summary()
        {
            return View();
        }

        public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(c => c.Id == cartId);
            var newCount = _unitOfWork.ShoppingCartRepository.IncrementCount(cart, 1);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(c => c.Id == cartId);
            var newCount = _unitOfWork.ShoppingCartRepository.DecrementCount(cart, 1);

            if (newCount <= 0) 
                Remove(cartId);

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(c => c.Id == cartId);
            _unitOfWork.ShoppingCartRepository.Remove(cart);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
        private double GetPriceBasedOnQuantity(double quantity, double price, double price50, double price100)
        {
            if (quantity <= 50) 
                return price;
            if (quantity > 100) 
                return price100;
            return price50;
        }
    }
}
