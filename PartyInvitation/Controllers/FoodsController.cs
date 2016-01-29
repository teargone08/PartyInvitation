using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PartyInvitation.Models;

namespace PartyInvitation.Controllers
{
    public class FoodsController : Controller
    {
        private PartyEntities db = new PartyEntities();

        // GET: Foods
        public ActionResult Index()
        {
            var foods = db.Foods.Include(f => f.FoodType);
            return View(foods.ToList());
        }

        // GET: Foods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Foods.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // GET: Foods/Create
        public ActionResult Create()
        {
            ViewBag.foodTypeId = new SelectList(db.FoodTypes, "FoodTypeId", "FoodTypeName");
            return View();
        }

        // POST: Foods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "foodId,foodTypeId,foodName,ImageFileName")] Food food, HttpPostedFileBase uploadedFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.foodTypeId = new SelectList(db.FoodTypes, "FoodTypeId", "FoodTypeName", food.foodTypeId);
                return View(food);
            }
            else
            {
                db.Foods.Add(food);              
                string fileName = string.Empty;
                if (uploadedFile != null)
                {
                    fileName = System.IO.Path.GetFileName(uploadedFile.FileName);
                    food.ImageFileName = fileName;
                    string imageFilePathOnServer = System.IO.Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                    uploadedFile.SaveAs(imageFilePathOnServer);
                }
                this.db.Foods.Attach(food);
                this.db.Entry(food).State = System.Data.Entity.EntityState.Modified;
                this.db.SaveChanges();
                return RedirectToAction("Index");                
            }

        }


        // GET: Foods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Foods.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            ViewBag.foodTypeId = new SelectList(db.FoodTypes, "FoodTypeId", "FoodTypeName", food.foodTypeId);
            return View(food);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "foodId,foodTypeId,foodName,ImageFileName")] Food food)
        {
            if (ModelState.IsValid)
            {
                db.Entry(food).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.foodTypeId = new SelectList(db.FoodTypes, "FoodTypeId", "FoodTypeName", food.foodTypeId);
            return View(food);
        }

        // GET: Foods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Foods.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Food food = db.Foods.Find(id);
            db.Foods.Remove(food);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
