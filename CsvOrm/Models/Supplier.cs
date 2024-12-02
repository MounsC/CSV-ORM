using CsvOrm.Attributes;
using System.Collections.Generic;

namespace CsvOrm.Models
{
    [CsvTable("suppliers.csv")]
    public sealed class Supplier
    {
        [CsvPrimaryKey]
        [CsvColumn("id")]
        public int Id { get; set; }

        [CsvColumn("name")]
        [NotNull]
        public string Name { get; set; }

        [CsvColumn("contact_email")]
        [NotNull]
        public string ContactEmail { get; set; }

        [CsvForeignCollection(typeof(Product), "SupplierId")]
        public List<Product> Products { get; set; }
    }
}