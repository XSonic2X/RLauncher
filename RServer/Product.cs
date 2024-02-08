using BD;
using System.Data.SqlClient;

namespace RServer
{
    public class Product: Interface1
    {
        public Product() { }
        public string Name { get; set; } = "No_Name";
        public decimal Price { get; set; } = 0m;
        public void Initialize(SqlDataReader reader)
        {
            Name = (string)reader["Name"];
            Price = (decimal)reader["Price"];
        }
        public bool IfName(string name) => Name == name;
        public override string ToString()
        {
            return Name;
        }
    }
}
