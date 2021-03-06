﻿using Divergent.Sales.Data.Context;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace Divergent.Sales.API.Host.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        [HttpGet, Route("{orderNumber}")]
        public dynamic Get(int orderNumber)
        {
            using (var sales = new SalesContext())
            {
                return sales.Orders
                    .Include(order => order.Items)
                    .Where(order => order.OrderNumber == orderNumber)
                    .Select(order => new
                    {
                        OrderNumber = order.OrderNumber,
                        ItemsCount = order.Items.Count,
                    })
                    .SingleOrDefault();
            }
        }

        [HttpGet]
        public IEnumerable<dynamic> Get(int pageIndex, int pageSize)
        {
            using (var sales = new SalesContext())
            {
                return sales.Orders
                    .Include(order => order.Items)
                    .OrderBy(order => order.OrderNumber) //required by SQLite EF
                    .Skip(pageSize * pageIndex)
                    .Take(pageSize)
                    .Select(order => new
                    {
                        OrderNumber = order.OrderNumber,
                        ItemsCount = order.Items.Count,
                    })
                    .ToList();
            }
        }
    }
}
