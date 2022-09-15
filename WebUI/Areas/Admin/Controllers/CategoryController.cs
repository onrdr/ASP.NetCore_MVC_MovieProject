using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc; 
using Models;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Index GET
        public IActionResult Index()
        {
            var categoryList = _unitOfWork.CategoryRepository.GetAll().OrderBy(c => c.Name);
            return View(categoryList);
        }
        #endregion

        #region Create GET - POST
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
                ModelState.AddModelError("DisplayOrder", "Name and Display Order cannot be the same");

            if (!ModelState.IsValid)
                return View(obj);

            _unitOfWork.CategoryRepository.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit GET - POST
        public IActionResult Edit(int? id)
        {
            if (id == null || id < 1)
                return NotFound();

            var category = _unitOfWork.CategoryRepository.GetFirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
                ModelState.AddModelError("CustomError", "Name and Display Order cannot be the same");

            if (!ModelState.IsValid)
                return View(obj);

            _unitOfWork.CategoryRepository.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete GET - POST
        public IActionResult Delete(int? id)
        {
            if (id == null || id < 1)
                return NotFound();

            var category = _unitOfWork.CategoryRepository.GetFirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var category = _unitOfWork.CategoryRepository.GetFirstOrDefault(c => c.Id == id);
            if (category == null)
                return NotFound();

            _unitOfWork.CategoryRepository.Remove(category);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
        #endregion

    }
}
