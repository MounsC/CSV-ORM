using CsvOrm.Attributes;

namespace CsvOrm.Models
{
    [CsvTable("order_items.csv")]
    public sealed class OrderItem
    {
        [CsvPrimaryKey]
        [CsvColumn("id")]
        public int Id { get; set; }

        [CsvForeignKey(typeof(Order))]
        [CsvColumn("order_id")]
        public int OrderId { get; set; }

        [CsvForeignEntity]
        public Order Order { get; set; }

        [CsvForeignKey(typeof(Product))]
        [CsvColumn("product_id")]
        public int ProductId { get; set; }

        [CsvForeignEntity]
        public Product Product { get; set; }

        [CsvColumn("quantity")]
        [NotNull]
        public int Quantity { get; set; }

        [CsvColumn("unit_price")]
        [NotNull]
        public decimal UnitPrice { get; set; }
    }
}