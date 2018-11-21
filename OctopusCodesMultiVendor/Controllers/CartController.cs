using Microsoft.AspNetCore.Mvc;
using OctopusCodesMultiVendor.Helpers;
using OctopusCodesMultiVendor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace OctopusCodesMultiVendor.Controllers
{
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly IStringLocalizer<SharedResource> sharedLocalizer;

        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        public CartController(IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.sharedLocalizer = sharedLocalizer;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            try
            {
                var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                ViewBag.countItems = cart == null ? 0 : cart.Count;
                ViewBag.cart = cart;
                ViewBag.total = cart != null ? cart.Sum(i => i.Price * i.Quantity) : 0;
                ViewBag.username_customer = HttpContext.Session.GetString("username_customer");
                return View("Index");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = Exists(id, cart);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index", "cart");
        }

        [Route("update")]
        [HttpPost]
        public IActionResult Update(IFormCollection fc)
        {
            int productId = Convert.ToInt32(fc["productId"]);
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = Exists(productId, cart);
            cart[index].Quantity = Convert.ToInt32(fc["quantity"]);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        [Route("buy/{id}")]
        public IActionResult Buy(int id)
        {
            try
            {
                var product = ocmde.Products.Find(id);
                var mainPhoto = product.Photos.SingleOrDefault(p => p.Main && p.Status);
                var photo = mainPhoto == null ? "no-image.png" : mainPhoto.Name;
                if (!VendorHelper.checkExpires(product.VendorId))
                {
                    return RedirectToAction("Expires", "Product");
                }
                else
                {
                    var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                    if (cart == null)
                    {
                        List<Item> _cart = new List<Item>();
                        _cart.Add(new Item()
                        {
                            Id = product.Id,
                            Name = product.Name,
                            VendorId = product.VendorId,
                            VendorName = product.Vendor.Name,
                            Photo = photo,
                            Price = product.Price,
                            Quantity = 1
                        });
                        SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", _cart);
                    }
                    else
                    {
                        int index = Exists(id, cart);
                        if (index == -1)
                        {
                            cart.Add(new Item()
                            {
                                Id = product.Id,
                                Name = product.Name,
                                VendorId = product.VendorId,
                                VendorName = product.Vendor.Name,
                                Photo = photo,
                                Price = product.Price,
                                Quantity = 1
                            });
                        }
                        else
                        {
                            cart[index].Quantity++;
                        }
                        SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private int Exists(int productId, List<Item> cart)
        {
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Id == productId)
                {
                    return i;
                }
            }
            return -1;
        }

        [Route("save")]
        public IActionResult Save()
        {
            try
            {
                if (HttpContext.Session.GetString("username_customer") == null)
                {
                    return RedirectToAction("Index", "Login", new { Area = "CustomerPanel" });
                }
                else
                {
                    var customer = ocmde.AccountCustomer.SingleOrDefault(a => a.Email.Equals(HttpContext.Session.GetString("email_customer")));
                    var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                    var vendorIds = cart.Select(i => i.VendorId).Distinct().ToList();
                    vendorIds.ForEach(id =>
                    {
                        var currentVendor = ocmde.Vendors.Find(id);

                        // Create new order
                        Orders order = new Orders()
                        {
                            CustomerId = customer.Id,
                            DateCreation = DateTime.Now,
                            Name = sharedLocalizer["New_Order_for_Vendor"] + " " + currentVendor.Name,
                            OrderStatusId = 1,
                            VendorId = id
                        };
                        ocmde.Orderss.Add(order);
                        ocmde.SaveChanges();

                        // Create order details
                        cart.Where(i => i.VendorId == id).ToList().ForEach(i =>
                    {
                        OrdersDetail ordersDetail = new OrdersDetail()
                        {
                            OrderId = order.Id,
                            Price = i.Price,
                            Quantity = i.Quantity,
                            ProductId = i.Id
                        };
                        ocmde.OrdersDetails.Add(ordersDetail);
                        ocmde.SaveChanges();
                    });

                    });

                    // Remove Cart
                    HttpContext.Session.Remove("Cart");

                    return RedirectToAction("Index", "Orders", new { Area = "CustomerPanel" });
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}