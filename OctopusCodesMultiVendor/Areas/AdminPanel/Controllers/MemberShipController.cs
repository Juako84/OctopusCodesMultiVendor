using OctopusCodesMultiVendor.Models;
using System;
using System.Linq;
using OctopusCodesMultiVendor.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace OctopusCodesMultiVendor.Areas.AdminPanel.Controllers
{
    [Area("adminpanel")]
    [Route("adminpanel/membership")]
    public class MemberShipController : Controller
    {
        private readonly IStringLocalizer<MemberShipController> localizer;

        private readonly IStringLocalizer<SharedResource> sharedLocalizer;

        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        public MemberShipController(IStringLocalizer<MemberShipController> localizer, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;
        }

        [Route("index")]
        public IActionResult Index()
        {
            try
            {
                ViewBag.memberships = ocmde.MemberShips.ToList();
                return View();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("add")]
        [HttpGet]
        public IActionResult Add()
        {
            try
            {
                return View("Add", new MemberShip());
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("add")]
        [HttpPost]
        public IActionResult Add(MemberShip memberShip)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ocmde.MemberShips.Add(memberShip);
                    ocmde.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View("Add", memberShip);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id == MemberShipHelper.TrialPackage)
                {
                    TempData["error"] = sharedLocalizer["Can_not_delete_Trial_Package"];
                    return RedirectToAction("Index");
                }
                else
                {
                    var membership = ocmde.MemberShips.SingleOrDefault(m => m.Id == id);
                    ocmde.MemberShips.Remove(membership);
                    ocmde.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("status/{id}")]
        public IActionResult Status(int id)
        {
            try
            {
                var membership = ocmde.MemberShips.SingleOrDefault(m => m.Id == id);
                membership.Status = !membership.Status;
                ocmde.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("edit/{id}")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var membership = ocmde.MemberShips.SingleOrDefault(m => m.Id == id);
                return View("Edit", membership);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("edit")]
        [HttpPost]
        public IActionResult Edit(MemberShip memberShip)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentMemberShip = ocmde.MemberShips.Find(memberShip.Id
    );
                    currentMemberShip.Name = memberShip.Name;
                    currentMemberShip.Price = memberShip.Price;
                    currentMemberShip.Status = memberShip.Status;
                    currentMemberShip.Description = memberShip.Description;
                    currentMemberShip.Month = memberShip.Month;
                    ocmde.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View("Edit", memberShip);
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}