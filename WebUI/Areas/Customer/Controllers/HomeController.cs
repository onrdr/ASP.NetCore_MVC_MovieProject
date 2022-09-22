using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Diagnostics;
using System.Security.Claims;

namespace WebUI.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category");
            return View(productList);
        }

        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Count = 1,
                ProductId = productId,
                Product = _unitOfWork.ProductRepository.GetFirstOrDefault(p => p.Id == productId, includeProperties: "Category")
            };
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            var cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(
                c => c.ApplicationUserId == claim.Value &&
                c.ProductId == shoppingCart.ProductId);

            if (cart != null)
                _unitOfWork.ShoppingCartRepository.IncrementCount(cart, shoppingCart.Count);
            else
                _unitOfWork.ShoppingCartRepository.Add(shoppingCart);

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}