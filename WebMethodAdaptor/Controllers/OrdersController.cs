using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMethodAdaptor.Models;

namespace WebMethodAdaptor.Controllers
{
    [ApiController]
    public class OrdersController : ControllerBase
    {

        // method to retrieve data
        [HttpGet]
        [Route("api/[controller]")]
        public List<OrdersDetails> GetOrderData()
        {
            // Retrieve all records and convert to list
            var data = OrdersDetails.GetAllRecords().ToList();
            return data;
        }

        // method to handle incoming data manager requests
        [HttpPost]
        [Route("api/[controller]")]
        public object Post()
        {
            // Retrieve data source and convert to queryable
            IQueryable<OrdersDetails> DataSource = GetOrderData().AsQueryable();

            // Get total record count
            int totalRecordsCount = DataSource.Count();

            // Return result and total record count
            return new { result = DataSource, count = totalRecordsCount };
        }

        /// <summary>
        /// Inserts a new data item into the data collection.
        /// </summary>
        /// <param name="newRecord">It contains the new record detail which is need to be inserted.</param>
        /// <returns>Returns void</returns>
        [HttpPost]
        [Route("api/[controller]/Insert")]
        public void Insert([FromBody] CRUDModel<OrdersDetails> newRecord)
        {
            // Check if new record is not null
            if (newRecord.value != null)
            {
                // Insert new record
                OrdersDetails.GetAllRecords().Insert(0, newRecord.value);
            }
        }

        /// <summary>
        /// Update a existing data item from the data collection.
        /// </summary>
        /// <param name="updatedRecord">It contains the updated record detail which is need to be updated.</param>
        /// <returns>Returns void</returns>
        [HttpPost]
        [Route("api/[controller]/Update")]
        public void Update([FromBody] CRUDModel<OrdersDetails> updatedRecord)
        {
            // Retrieve updated order
            var updatedOrder = updatedRecord.value;
            if (updatedOrder != null)
            {
                // Find existing record
                var data = OrdersDetails.GetAllRecords().FirstOrDefault(or => or.OrderID == updatedOrder.OrderID);
                if (data != null)
                {
                    // Update existing record
                    data.OrderID = updatedOrder.OrderID;
                    data.CustomerID = updatedOrder.CustomerID;
                    data.ShipCity = updatedOrder.ShipCity;
                    data.ShipCountry = updatedOrder.ShipCountry;
                    data.EmployeeID = updatedOrder.EmployeeID;
                    data.Freight = updatedOrder.Freight;
                    data.Verified = updatedOrder.Verified;
                    data.OrderDate = updatedOrder.OrderDate;
                    data.ShipName = updatedOrder.ShipName;
                    data.ShipCountry = updatedOrder.ShipCountry;
                    data.ShippedDate = updatedOrder.ShippedDate;
                    data.ShipAddress = updatedOrder.ShipAddress;
                }
            }
        }

        /// <summary>
        /// Remove a specific data item from the data collection.
        /// </summary>
        /// <param name="deletedRecord">It contains the specific record detail which is need to be removed.</param>
        /// <return>Returns void</return>
        [HttpPost]
        [Route("api/[controller]/Remove")]
        public void Remove([FromBody] CRUDModel<OrdersDetails> deletedRecord)
        {
            int orderId = int.Parse(deletedRecord.key.ToString()); // get key value from the deletedRecord
            var data = OrdersDetails.GetAllRecords().FirstOrDefault(orderData => orderData.OrderID == orderId);
            if (data != null)
            {
                // Remove the record from the data collection
                OrdersDetails.GetAllRecords().Remove(data);
            }
        }

        [HttpPost]
        [Route("api/[controller]/CrudUpdate")]
        public void CrudUpdate([FromBody] CRUDModel<OrdersDetails> request)
        {
            // perform update operation
            if (request.action == "update")
            {
                var orderValue = request.value;
                OrdersDetails existingRecord = OrdersDetails.GetAllRecords().Where(or => or.OrderID == orderValue.OrderID).FirstOrDefault();
                existingRecord.OrderID = orderValue.OrderID;
                existingRecord.CustomerID = orderValue.CustomerID;
                existingRecord.Freight = orderValue.Freight;
                existingRecord.EmployeeID = orderValue.EmployeeID;
                existingRecord.ShipAddress = orderValue.ShipAddress;
                existingRecord.ShippedDate = orderValue.ShippedDate;
                existingRecord.ShipCountry = orderValue.ShipCountry;
                existingRecord.ShipName = orderValue.ShipName;
                existingRecord.ShipCity = orderValue.ShipCity;
                existingRecord.Verified = orderValue.Verified;
                existingRecord.OrderDate = orderValue.OrderDate;
            }
            // perform insert operation
            else if (request.action == "insert")
            {
                OrdersDetails.GetAllRecords().Insert(0, request.value);
            }
            // perform remove operation
            else if (request.action == "remove")
            {
                OrdersDetails.GetAllRecords().Remove(OrdersDetails.GetAllRecords().Where(or => or.OrderID == int.Parse(request.key.ToString())).FirstOrDefault());
            }
        }

        public class CRUDModel<T> where T : class
        {
            public string? action { get; set; }
            public string? keyColumn { get; set; }
            public object? key { get; set; }
            public T? value { get; set; }
            public List<T>? added { get; set; }
            public List<T>? changed { get; set; }
            public List<T>? deleted { get; set; }
            public IDictionary<string, object>? @params { get; set; }
        }
    }
}