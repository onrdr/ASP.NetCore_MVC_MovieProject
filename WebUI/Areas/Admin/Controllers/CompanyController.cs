using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.ViewModel;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork; 
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            Company company = new();

            if (id == null || id < 1)
                return View(company);

            company = _unitOfWork.CompanyRepository.GetFirstOrDefault(p => p.Id == id);
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (!ModelState.IsValid)
                return View(company);

            if (company.Id == 0)
            {
                _unitOfWork.CompanyRepository.Add(company);
                TempData["success"] = "Product created successfully";
            }
            else
            {
                _unitOfWork.CompanyRepository.Update(company);
                TempData["success"] = "Product updated successfully";
            }
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
        #endregion 

        #region API CALLS : https://localhost:44304/admin/product/getall
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.CompanyRepository.GetAll();
            return Json(new { data = companyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var company = _unitOfWork.CompanyRepository.GetFirstOrDefault(c => c.Id == id);
            if (company == null)
                return Json(new { success = false, message = "Error while deleting" });
             
            _unitOfWork.CompanyRepository.Remove(company);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted successfully" });
        }
        #endregion 

    }
}
