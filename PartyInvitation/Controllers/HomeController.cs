using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PartyInvitation.Models;

namespace PartyInvitation.Controllers
{
    public class HomeController : Controller
    {
        private PartyEntities dbContext = new PartyEntities();
        public ActionResult Index()
        {
            int hour = DateTime.Now.Hour;
            ViewBag.Greeting = hour < 12 ? "Good Morning" : "Good Afternoon";
            return View();
        }
        //public ActionResult About()
        //{
        //    return View();
        //}
        [HttpGet]
        public ViewResult RsvpForm()
        {
            return View();
        }
        [HttpPost]
        public ViewResult RsvpForm(GuestResponse guestResponse)
        {

            if (ModelState.IsValid)
            {
                dbContext.GuestResponses.Add(guestResponse);
                dbContext.SaveChanges();
                // TODO: Email response to the party organizer
                return View("Thanks", guestResponse);
            }
            else
            {
                // there is a validation error
                return View();
            }
        }


        [HttpGet]
        public ActionResult ViewAllGuests()
        {
            List<GuestResponse> allGuests = this.dbContext.GuestResponses.ToList();
            return View(allGuests);
        }
        [HttpGet]
        public ActionResult UpdateRsvpForm(string id)
        {
            GuestResponse guest = this.dbContext.GuestResponses.FirstOrDefault(p => p.Id.ToString() == id);
            if (guest == null)
            {
                return RedirectToAction("ViewAllGuests");
            }
            return View(guest);

        }
        [HttpPost]
        public ActionResult UpdateRsvpForm(GuestResponse updateGuestResponse)
        {

            this.dbContext.GuestResponses.Attach(updateGuestResponse);
            this.dbContext.Entry(updateGuestResponse).State = System.Data.Entity.EntityState.Modified;
            this.dbContext.SaveChanges();
            return RedirectToAction("ViewAllGuests");
        }

        //public ActionResult ViewFood(ViewFoodViewModel vm)
        //{
        //    if (vm.SelectedPageCapacity == 0)
        //    {
        //        vm.SelectedPageCapacity = vm.AvailablePageCapacities[0];
        //    }
        //    if (vm.SelectedPageNumber == 0)
        //    {
        //        vm.SelectedPageNumber = 1;
        //    }

        //    int maxPageNumber = (int)Math.Ceiling(dbContext.SelectValidFoodAmount().First().Value / (double)vm.SelectedPageCapacity);
        //    for (int i = 1; i <= maxPageNumber; i++)
        //    {
        //        vm.AvailablePageNumbers.Add(i);
        //    }
        //    if (vm.SelectedPageNumber > maxPageNumber)
        //    {
        //        vm.SelectedPageNumber = 1;
        //    }

        //    //List<SelectValidProducts_Result> products = dbContext.SelectValidProducts(50, 1).ToList();
        //    //ViewProductsViewModel vm = new ViewProductsViewModel();
        //    //vm.Products = dbContext.SelectValidProducts(20, 1).ToList();
        //    vm.Foods = dbContext.SelectValidFood(vm.SelectedPageCapacity, vm.SelectedPageNumber).ToList();
        //    return View(vm);

        //}
        [HttpGet]
        public ActionResult CreateNewFood()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateNewFood(Food newFood)
        {
            this.dbContext.Foods.Add(newFood);
            this.dbContext.SaveChanges();
            return RedirectToAction("ViewFood");
        }
        [HttpGet]
        public ActionResult UpdateFood(string id)
        {
            Food food = this.dbContext.Foods.FirstOrDefault(p => p.foodId.ToString() == id);
            if (food == null)
            {
                return RedirectToAction("ViewFood");
            }
            return View(food);
        }
        [HttpPost]
        public ActionResult UpdateFood(Food updateFood, HttpPostedFileBase uploadedFile)
        {
            string fileName = string.Empty;
            if (uploadedFile != null)
            {
                fileName = System.IO.Path.GetFileName(uploadedFile.FileName);
                updateFood.ImageFileName = fileName;
                string imageFilePathOnServer = System.IO.Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                uploadedFile.SaveAs(imageFilePathOnServer);
            }
            this.dbContext.Foods.Attach(updateFood);
            this.dbContext.Entry(updateFood).State = System.Data.Entity.EntityState.Modified;
            this.dbContext.SaveChanges();
            return RedirectToAction("ViewFood");
        }

        [HttpGet]
        public ActionResult DeleteFood(string id)
        {
            Food food = dbContext.Foods.FirstOrDefault(p => p.foodId.ToString() == id);
            return View(food);
        }

        [HttpGet]
        public ActionResult ConfirmDeleteFood(string id)
        {
            Food food = dbContext.Foods.FirstOrDefault(p => p.foodId.ToString() == id);
            this.dbContext.Entry(food).State = System.Data.Entity.EntityState.Deleted;
            this.dbContext.SaveChanges();
            return RedirectToAction("ViewFood");
        }

       

        public ActionResult About()
        {
            ViewBag.Message = "The brief introduction of this application .";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Tiegang(Tony) Tong";

            return View();
        }
    }
}