using OctopusCodesMultiVendor.Models;
using System;
using System.Linq;

namespace OctopusCodesMultiVendor.Helpers
{
    public class VendorHelper
    {
        private static OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        public static bool checkExpires(int vendorId)
        {
            try
            {
                var lastMemberShip = ocmde.MemberShipVendors.Where(m => m.VendorId == vendorId).OrderByDescending(m => m.Id).First();
                return lastMemberShip.EndDate >= DateTime.Now;
            }
            catch
            {
                return false;
            }
        }
    }
}