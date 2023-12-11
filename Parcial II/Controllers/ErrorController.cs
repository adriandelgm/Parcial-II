using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Parcial_II.Controllers
{
    public class ErrorController : Controller
    {
        // GET: ErrorController1cs
        public ActionResult Index(string message)
        {
            ViewBag.Message = message;
            return View("Error");
        }

        // GET: ErrorController1cs/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ErrorController1cs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ErrorController1cs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ErrorController1cs/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ErrorController1cs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ErrorController1cs/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ErrorController1cs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
