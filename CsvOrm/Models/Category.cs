using CsvOrm.Attributes;
using System.Collections.Generic;

namespace CsvOrm.Models
{
    [CsvTable("categories.csv")]
    public sealed class Category
    {
        [CsvPrimaryKey]
        [CsvColumn("id")]
        public int Id { get; set; }

        [CsvColumn("name")]
        [NotNull]
        public string Name { get; set; }

        [CsvForeignCollection(typeof(Product), "CategoryId")]
        public List<Product> Products { get; set; }
    }
}