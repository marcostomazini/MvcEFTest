using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcEFTest.Entities;

namespace MvcEFTest.Controllers
{
    public class ManufacturersController : Controller
    {
        private readonly MvcEFTestContext _db = new MvcEFTestContext();
        private bool _disposed;

        // GET: Manufacturers
        public async Task<ActionResult> Index()
        {
            return View(await _db.Manufacturers.ToListAsync());
        }

        // GET: Manufacturers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Manufacturer manufacturer = await _db.Manufacturers.FindAsync(id);
            if (manufacturer == null)
            {
                return HttpNotFound();
            }

            return View(manufacturer);
        }

        // GET: Manufacturers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manufacturers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,HeadquartersLocation")] Manufacturer manufacturer)
        {
            if (!ModelState.IsValid)
            {
                return View(manufacturer);
            }

            _db.Manufacturers.Add(manufacturer);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Manufacturers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Manufacturer manufacturer = await _db.Manufacturers.FindAsync(id);
            if (manufacturer == null)
            {
                return HttpNotFound();
            }

            return View(manufacturer);
        }

        // POST: Manufacturers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,HeadquartersLocation")] Manufacturer manufacturer)
        {
            if (!ModelState.IsValid)
            {
                return View(manufacturer);
            }

            _db.Entry(manufacturer).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Manufacturers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Manufacturer manufacturer = await _db.Manufacturers.FindAsync(id);
            if (manufacturer == null)
            {
                return HttpNotFound();
            }

            return View(manufacturer);
        }

        // POST: Manufacturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Manufacturer manufacturer = await _db.Manufacturers.FindAsync(id);
            _db.Manufacturers.Remove(manufacturer);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _db.Dispose();
            }

            _disposed = true;
            base.Dispose(disposing);
        }
    }
}
