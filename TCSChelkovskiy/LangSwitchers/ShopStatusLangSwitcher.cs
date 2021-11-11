using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCSchelkovskiyAPI.Models;

namespace TCSChelkovskiy.LangSwitchers
{
    public class ShopStatusLangSwitcher
    {
        public string GetStatusWord(ShopModel shop)
        {
            if (shop.Status == null) { shop.Status = ""; }
            switch (shop.Status)
            {
                case "no":
                    return "";
                case "new":
                    return "Новый";
                case "soon":
                    return "Скоро открытие";
                case "reconstruction":
                    return "На реконструкции";
                default:
                    return "";
            }
        }
    }
}
