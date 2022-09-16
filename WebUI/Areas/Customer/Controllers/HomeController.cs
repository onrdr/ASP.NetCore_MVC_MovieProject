using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Diagnostics;

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
        // "images\\products\\b7b79960-9fda-4da4-8b7e-3c06b4c5266a.jpg"
        public IActionResult Details(int? id)
        {
            ShoppingCart cart = new()
            {
                Count = 1,
                Product = _unitOfWork.ProductRepository.GetFirstOrDefault(p => p.Id == id, includeProperties: "Category")
            };
            return View(cart);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}