using System.Data.Common;
using System.Data;
using System.Diagnostics;
using DotNetCore_CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.Models;
using BusinessLogicLayer;

namespace DotNetCore_CRUD.Controllers
{
    public class HomeController : Controller
    {
        private readonly Repository _repo;

        public HomeController(Repository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            ViewBag.message = TempData["message"];
            return View();
        }

        [HttpPost]
        public IActionResult Index(FormModel model)
        {
            if (model != null)
            {
                string message = _repo.InsertData(model);
                TempData["message"] = message;
            }
            else
            {
                TempData["message"] = "Try again.";
            }
            return RedirectToAction("Index");
        }

        public IActionResult About(int Id)
        {
            if (Id != 0)
            {
                string message = _repo.DeleteData(Id);
                TempData["message"] = message;
                return RedirectToAction("About");
            }

            var formList = _repo.GetAllFormData();
            ViewBag.message = TempData["message"];
            return View(formList);
        }

        public IActionResult EditPage(int Id)
        {
            if (Id > 0)
            {
                var formModel = _repo.GetFormModelById(Id);
                return View(formModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult UpdateForm(FormModel model)
        {
            if (model != null)
            {
                string message = _repo.UpdateData(model);
                TempData["message"] = message;
            }
            else
            {
                TempData["message"] = "Try again.";
            }
            return RedirectToAction("About");
        }
    }
}
