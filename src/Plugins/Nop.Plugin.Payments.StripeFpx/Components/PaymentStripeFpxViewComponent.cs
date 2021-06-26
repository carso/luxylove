using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Payments.StripeFpx.Models;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Payments.StripeFpx.Components
{
    [ViewComponent(Name = "PaymentStripeFpx")]
    public class PaymentStripeFpxViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = new PaymentInfoModel()
            {
                CreditCardTypes = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Public Bank", Value = "bankin" }
                },
                PaymentOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Please Select", Value = "-1" },
                    new SelectListItem { Text = "Credit Card", Value = "card" },
                    new SelectListItem { Text = "FPX", Value = "fpx" }
            
                },
                EWallet = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Touch & Go", Value = "tng" }
                }

            };

                         


         

            return View("~/Plugins/Payments.StripeFpx/Views/PaymentInfo.cshtml", model);
        }
    }
}
