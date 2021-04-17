using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace EPH.Functions
{
    public static class OrderSystem
    {
        private static List<string> _orderStatusses = new List<string> { "REQUESTED", "ACCEPTED", "PICKING", "DELIVERING", "DELIVERED" };

        [FunctionName("OrderSystem")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "orders",
                collectionName: "orders",
                ConnectionStringSetting = "CosmosDBConnection")]
                IAsyncCollector<Order> orders,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            int amount = Int32.Parse(req.Query["amount"]);

            for (int i = 0; i < amount; i++)
            {
                // Create order
                var lookupIndex = new Random().Next(0, 47);
                
                var order = new Order
                {
                    orderNumber = Guid.NewGuid().ToString(),
                    orderStatus = _orderStatusses[new Random().Next(0, 4)],
                    hubId = new Random().Next(10000, 10010),
                    customer = new Customer
                    {
                        customerId = new Random().Next(50000, 50100),
                        deliveryAddress = new DeliveryAddress
                        {
                            name = Lookups.names[lookupIndex],
                            street = Lookups.streets[lookupIndex],
                            city = "Berlin",
                            zipCode = Lookups.zipCodes[lookupIndex],
                        }
                    },
                    articles = new List<Article>()
                };

                // Add articles
                for (int j = 0; j < new Random().Next(1, 6); j++)
                {
                    var articleIndex = new Random().Next(0, 42);
                    order.articles.Add(new Article
                    {
                        article = Lookups.articles[articleIndex],
                        quantity = new Random().Next(1, 10),
                        price = Lookups.prices[articleIndex],
                    });
                }

                log.LogInformation($"Order = {order.orderNumber}");
                await orders.AddAsync(order);
            }

            return new OkResult();
        }
    }
}
