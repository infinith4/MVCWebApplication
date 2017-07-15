using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCWebApplication.Models;
using MVCWebApplication.Common;
using MVCWebApplication.Common.Utils;

namespace MVCWebApplication.Controllers
{
    public class OrderController : Controller
    {
        LearnAppsEntities _db = new LearnAppsEntities();
        string _sessionId = string.Empty;
        string _aspId = string.Empty;
        Log log = new Log();
        // 1. GET: Order
        public ActionResult Index()
        {
            //セッションID発行
            DateTime dt = DateTime.Now;
            System.Random r = new System.Random();
            string sessionId = dt.ToString("yyyyMMddhhmmssfff") + r.Next(10000).ToString();
            Session[Const.Session.SESSION_ID] = sessionId;
            string aspId = "0001";
            Session[Const.Session.ASP_ID] = aspId;
            return View();
        }
        // 2. 
        public ActionResult SelectProduct()
        {
            _sessionId = Session[Const.Session.SESSION_ID].ToString();
            _aspId = Session[Const.Session.ASP_ID].ToString();
            var orderProduct = from mst_Price in _db.MST_Price
                                join mst_Product in _db.MST_Product on mst_Price.ProductId equals mst_Product.ProductId
                                where mst_Price.ASP_ID == _aspId
                                orderby mst_Product.ProductId
                                select new OrderModel.Product
                                {
                                    ProductId = mst_Price.ProductId,
                                    ProductName = mst_Product.ProductName,
                                    Price = (decimal)mst_Price.Price
                                };
            List<OrderModel.Product> orderProductList = orderProduct.ToList();
            ViewData[Const.ViewData.PRODUCT_DROPDOWN_LIST] = OrderUtil.MakeProductDropdownList(orderProductList);

            //private Method へのアクセス
            //string productName = OrderUtil.SelectProductName(0);  //アクセスできない保護レベルになっている
            return View(new OrderModel.Product());
        }

        [HttpPost]
        public ActionResult SelectProduct(OrderModel.Product model)
        {
            try
            {
            	//強制的にExceptionを発生させる
            	//throw new Exception();
                
                _sessionId = Session[Const.Session.SESSION_ID].ToString();
                _aspId = Session[Const.Session.ASP_ID].ToString();
                if (model.ProductId != -1)
                {
                    var orderProduct = (from mst_Price in _db.MST_Price
                                        join mst_Product in _db.MST_Product on mst_Price.ProductId equals mst_Product.ProductId
                                        where mst_Price.ASP_ID == _aspId && mst_Price.ProductId == model.ProductId
                                        select new OrderModel.Product
                                        {
                                            ProductId = mst_Price.ProductId,
                                            ProductName = mst_Product.ProductName,
                                            Price = (decimal)mst_Price.Price
                                        }).Single();

                    OrderManage order = new OrderManage()
                    {
                        SessionId = _sessionId,
                        ProductId = orderProduct.ProductId,
                        Price = orderProduct.Price
                    };

                    _db.OrderManage.Add(order);
                    _db.SaveChanges();

                    return Redirect(Const.Redirect.ORDER_END);
                }
                else
                {
                    var orderProduct = from mst_Price in _db.MST_Price
                                       join mst_Product in _db.MST_Product on mst_Price.ProductId equals mst_Product.ProductId
                                       where mst_Price.ASP_ID == _aspId
                                       orderby mst_Product.ProductId
                                       select new OrderModel.Product
                                       {
                                           ProductId = mst_Price.ProductId,
                                           ProductName = mst_Product.ProductName
                                       };
                    List<OrderModel.Product> orderProductList = orderProduct.ToList();
                    ViewData[Const.ViewData.PRODUCT_DROPDOWN_LIST] = OrderUtil.MakeProductDropdownList(orderProductList);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                log.WriteError("SelectProduct[HttpPost]でエラーが発生", ex);
                throw;
            }
        }

        public ActionResult End()
        {
            Session.Remove(Const.Session.SESSION_ID);
            return View();
        }
    }
}