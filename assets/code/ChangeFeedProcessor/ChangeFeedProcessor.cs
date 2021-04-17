using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EPH.Functions
{
    public static class ChangeFeedProcessor
    {
        [FunctionName("ChangeFeedProcessor")]
        public static void Run([CosmosDBTrigger(
            databaseName: "orders",
            collectionName: "orders",
            ConnectionStringSetting = "CosmosDBConnection",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input,
            [Sql("Orders", ConnectionStringSetting = "SqlConnectionString")] ICollector<SqlOrder> ordersOut,
            ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                var orders = input.Select(item => JsonConvert.DeserializeObject<Order>(item.ToString())).ToList();

                foreach (var order in orders)
                {
                    ordersOut.Add(new SqlOrder{
                        id = order.id,
                        orderNumber = order.orderNumber,
                        orderStatus = order.orderStatus,
                        hubId = order.hubId,
                        customerName = order.customer.deliveryAddress.name,
                        street = order.customer.deliveryAddress.street,
                        city = order.customer.deliveryAddress.city,
                        zipCode = order.customer.deliveryAddress.zipCode,
                        articleCount = order.articles.Count
                    });
                }
            }
        }
    }
}