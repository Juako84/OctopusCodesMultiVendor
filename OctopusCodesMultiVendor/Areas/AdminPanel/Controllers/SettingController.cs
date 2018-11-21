using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OctopusCodesMultiVendor.Models;
using System;
using System.IO;
using System.Linq;

namespace OctopusCodesMultiVendor.Areas.AdminPanel.Controllers
{
    [Area("adminpanel")]
    [Route("adminpanel/settings")]
    public class SettingController : Controller
    {
        private readonly IStringLocalizer<SettingController> localizer;

        private readonly IStringLocalizer<SharedResource> sharedLocalizer;

        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        public SettingController(IStringLocalizer<SettingController> localizer, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;
        }

        [Route("group/{id}")]
        public IActionResult Group(string id)
        {
            try
            {
                ViewBag.settings = ocmde.Settings.Where(g => g.Group.Equals(id)).ToList();
                return View("Index");
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
                var setting = ocmde.Settings.SingleOrDefault(p => p.Id == id);
                return View("Edit", setting);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("edit/{id?}")]
        [HttpPost]
        public IActionResult Edit(int id, Setting setting, IFormFile logo)
        {
            try
            {
                if (logo != null && logo.Length > 0 && !logo.ContentType.Contains("image"))
                {
                    ViewBag.errorPhoto = sharedLocalizer["Photo_Invalid"];
                    return View("Edit", ocmde.Settings.Find(setting.Id));
                }

                if (ModelState.IsValid)
                {
                    var currentSetting = ocmde.Settings.Find(setting.Id);
                    if (logo != null && logo.FileName.Length != 0)
                    {
                        var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetFileName(logo.FileName);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/user/images", fileName);
                        var stream = new FileStream(path, FileMode.Create);
                        logo.CopyTo(stream);
                        currentSetting.Value = fileName;
                    }
                    else
                    {
                        currentSetting.Value = setting.Value;
                    }
                    ocmde.SaveChanges();
                    return RedirectToAction("Group", "Setting", new { id = currentSetting.Group });
                }
                return View("Edit", setting);
            }
            catch (Exception e)
            {
                throw;
            }
        }



    }
}