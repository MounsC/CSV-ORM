using CsvOrm.Attributes;
using System.Collections.Generic;

namespace CsvOrm.Models
{
    [CsvTable("products.csv")]
    public sealed class Product
    {
        [CsvPrimaryKey]
        [CsvColumn("id")]
        public int Id { get; set; }

        [CsvColumn("name")]
        [NotNull]
        public string Name { get; set; }

        [CsvColumn("price")]
        [NotNull]
        public decimal Price { get; set; }

        [CsvForeignKey(typeof(Category))]
        [CsvColumn("category_id")]
        public int CategoryId { get; set; }

        [CsvForeignEntity]
        public Category Category { get; set; }

        [CsvForeignKey(typeof(Supplier))]
        [CsvColumn("supplier_id")]
        public int SupplierId { get; set; }

        [CsvForeignEntity]
        public Supplier Supplier { get; set; }

        [CsvForeignCollection(typeof(OrderItem), "ProductId")]
        public List<OrderItem> OrderItems { get; set; }
    }
}