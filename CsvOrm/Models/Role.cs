using CsvOrm.Attributes;
using System.Collections.Generic;

namespace CsvOrm.Models
{
    [CsvTable("roles.csv")]
    public sealed class Role
    {
        [CsvPrimaryKey]
        [CsvColumn("id")]
        public int Id { get; set; }

        [CsvColumn("name")]
        [NotNull]
        public string Name { get; set; }

        [CsvForeignCollection(typeof(User), "RoleId")]
        public List<User> Users { get; set; }
    }
}