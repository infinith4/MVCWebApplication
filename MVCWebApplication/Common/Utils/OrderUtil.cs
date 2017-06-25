using MVCWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCWebApplication.Common.Utils
{
    public class OrderUtil
    {
        public static List<SelectListItem> MakeProductDropdownList(List<OrderModel.Product> productList)
        {
            var productItems = new List<SelectListItem>() {
                new SelectListItem() { Value = "-1", Text = "商品を選択して下さい", Selected = true }
            };
            foreach (var item in productList)
            {
                productItems.Add(new SelectListItem() { Value = item.ProductId.ToString(), Text = string.Format("商品名: {0}, 価格: {1}", item.ProductName, String.Format("{0:#,0}円", item.Price)) , Selected = false });
            }

            return productItems;
        }

        /// <summary>
        /// 商品IDから商品名を取得する
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private static string SelectProductName(int productId)
        {
            string productName = string.Empty;
            using (var _db = new LearnAppsEntities())
            {
                var mst_product = _db.MST_Product.SingleOrDefault(c => c.ProductId == productId);
                if (mst_product != null)
                {
                    productName = mst_product.ProductName;
                }
            }
            return productName;
        }
    }
}