using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Payments.StripeFpx.Models
{
    public record PaymentInfoModel : BaseNopModel
    {
        public PaymentInfoModel()
        {
            CreditCardTypes = new List<SelectListItem>();
            PaymentOptions = new List<SelectListItem>();
            EWallet =  new List<SelectListItem>();
   
        }

        [NopResourceDisplayName("Payment.SelectCreditCard")]
        public string CreditCardType { get; set; }

        [NopResourceDisplayName("Payment.SelectCreditCard")]
        public IList<SelectListItem> CreditCardTypes { get; set; }


        [NopResourceDisplayName("Payment.PaymentOptions")]
        public IList<SelectListItem> PaymentOptions { get; set; }

        [NopResourceDisplayName("Payment.EWallet")]
        public IList<SelectListItem> EWallet { get; set; }

   
    }
}