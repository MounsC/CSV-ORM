using CsvOrm.Attributes;
using System;
using System.Collections.Generic;

namespace CsvOrm.Models
{
    [CsvTable("orders.csv")]
    public sealed class Order
    {
        [CsvPrimaryKey]
        [CsvColumn("id")]
        public int Id { get; set; }

        [CsvForeignKey(typeof(User))]
        [CsvColumn("user_id")]
        public int UserId { get; set; }

        [CsvForeignEntity]
        public User User { get; set; }

        [CsvColumn("order_date")]
        public DateTime OrderDate { get; set; }

        [CsvForeignCollection(typeof(OrderItem), "OrderId")]
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}