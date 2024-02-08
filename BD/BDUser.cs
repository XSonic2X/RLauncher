using System;
using System.Data.SqlClient;

namespace BD
{
    public class BDUser: IDisposable
    {
        public BDUser() 
        { 
        }
        public BDUser(byte[] bytes)
        {
            SetBDUser(SystemCustom.ReadXml<BDUser>(bytes));
        }
        private void SetBDUser(BDUser dUser)
        {
            Name = dUser.Name;
            Login = dUser.Login;
            Password = dUser.Password;
            Key = dUser.Key;
        }
        public BDUser(SqlDataReader reader)
        {
            if (reader.HasRows)
            {
                Name = (string)reader["Name"];
                Login = (string)reader["Login"];
                Password = (string)reader["Pass"];
                Key = Login+SystemCustom.Generation((49 - Login.Length));
            }
            else
            { 
                Key = "";
            }
            reader.Close();
            reader.Dispose();
        }
        public string Name = "No_Name";
        public string Login = "";
        public string Password = "";
        public string Key = "";
        public void SetKey(string Key)
        { 
            this.Key = Key;
            Password = "";
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
