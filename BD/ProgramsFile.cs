using System.Data.SqlClient;

namespace BD
{
    public class ProgramsFile : Interface1
    {
        public ProgramsFile() 
        { 

        }
        public ProgramsFile(string txt)
        { 
            path = txt;
            string[] strings = path.Split(new char[] { '\\' });
            name = strings[(strings.Length-1)];
        }
        public string name;
        public string path;
        public string Description = "Реализация добавление описание на стороне сервера еще не придумано";
        public decimal Price = 0;

        public void Initialize(SqlDataReader reader)
        {
            name = (string)reader["Name"];
            Price = (decimal)reader["Price"];
        }
        public override string ToString()
        {
            return name;
        }
    }
}
