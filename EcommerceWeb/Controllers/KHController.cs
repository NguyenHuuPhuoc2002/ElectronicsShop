using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Controllers
{
    public class KHController : Controller
    {
        // GET: KHController
        public ActionResult Index()
        {
            return View();
        }

        // GET: KHController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: KHController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KHController/Create
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

        // GET: KHController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: KHController/Edit/5
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

        // GET: KHController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: KHController/Delete/5
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
