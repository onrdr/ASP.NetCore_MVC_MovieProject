using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.ViewModel;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        #region Index GET
        public IActionResult Index()
        {
            return View();
        }
        #endregion 

        #region Upsert GET - POST
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = GetSelectedItems()
            };

            if (id == null || id < 1)
            {
                //ViewBag.CategoryList = categoryList;
                //ViewData["Category"] = categoryList;
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.ProductRepository.GetFirstOrDefault(p => p.Id == id);
                return View(productVM);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                UploadFileIfNotNull(productVm, file);

                ProceedToAddOrUpdate(productVm);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }

            productVm.CategoryList = GetSelectedItems();
            return View(productVm);
        }
        #endregion 

        #region API CALLS : https://localhost:44304/admin/product/getall
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category");
            return Json(new { data = productList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var category = _unitOfWork.ProductRepository.GetFirstOrDefault(c => c.Id == id);
            if (category == null)
                return Json(new { success = false, message = "Error while deleting" });

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, category.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
                System.IO.File.Delete(oldImagePath);

            _unitOfWork.ProductRepository.Remove(category);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted successfully" });
        }
        #endregion

        #region Functions 
        private IEnumerable<SelectListItem> GetSelectedItems()
        {
            return _unitOfWork.CategoryRepository.GetAll().Select(category => new SelectListItem
            {
                Text = category.Name,
                Value = category.Id.ToString(),
            });
        }
        private void UploadFileIfNotNull(ProductVM productVm, IFormFile? file)
        {
            if (file != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\products");
                var extension = Path.GetExtension(file.FileName);

                DeleteFileIfNotNull(productVm, wwwRootPath);

                using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                productVm.Product.ImageUrl = @"images\products\" + fileName + extension;
            }
        }
        private void DeleteFileIfNotNull(ProductVM productVm, string wwwRootPath)
        {
            if (productVm.Product.ImageUrl != null)
            {
                var oldImagePath = Path.Combine(wwwRootPath, productVm.Product.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                    System.IO.File.Delete(oldImagePath);
            }
        }
        private void ProceedToAddOrUpdate(ProductVM productVm)
        {
            if (productVm.Product.Id == 0)
            {
                _unitOfWork.ProductRepository.Add(productVm.Product);
                return;
            }
            _unitOfWork.ProductRepository.Update(productVm.Product);
        }
        #endregion

    }
}
