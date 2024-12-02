using CsvOrm.Attributes;
using System.Collections.Generic;

namespace CsvOrm.Models
{
    [CsvTable("users.csv")]
    public sealed class User
    {
        [CsvPrimaryKey]
        [CsvColumn("id")]
        public int Id { get; set; }

        [CsvColumn("username")]
        [CsvIndex]
        [Unique]
        [NotNull]
        public string Username { get; set; }

        [CsvColumn("email")]
        [Unique]
        [NotNull]
        public string Email { get; set; }

        [CsvForeignKey(typeof(Role))]
        [CsvColumn("role_id")]
        public int RoleId { get; set; }

        [CsvForeignEntity]
        public Role Role { get; set; }

        [CsvForeignCollection(typeof(Order), "UserId")]
        public List<Order> Orders { get; set; }
    }
}