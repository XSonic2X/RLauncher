using BD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace RServer
{
    public class SQL
    {
        public SQL(string ConString)
        {
            Connection = new SqlConnection(ConString);
        }
        private SqlConnection Connection { get; set; }
        public void Open()
        {
            Connection.Open();
        }
        public void Close()
        {
            Connection.Close();
        }
        public SqlDataReader ProcedureInvokeSDR(string ProcedureName, SqlParameter[] Parameters)
        {
            
            SqlDataReader reader;
            try
            {
                using (SqlCommand sq = new SqlCommand("", Connection))
                {
                    sq.CommandType = CommandType.StoredProcedure;
                    sq.CommandText = ProcedureName;
                    for (int i = 0; i < Parameters.Length; i++)
                    {
                        sq.Parameters.Add(Parameters[i]);
                    }
                    reader = sq.ExecuteReader();
                    R = reader.Read();
                }
                return reader;
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
            return null;
        }
        bool R = false;
        public void ProcedureInvoke(string ProcedureName, SqlParameter[] Parameters)
        {
            using (SqlDataReader sdr = ProcedureInvokeSDR(ProcedureName, Parameters)) { sdr.Close(); }
        }
        public SqlParameter[] SqlParameters(string[] txt) // Создание параметров
        {
            SqlParameter[] sqlParameters = new SqlParameter[txt.Length];
            for (int i = 0; i < txt.Length; i++)
            {
                sqlParameters[i] = new SqlParameter();
                sqlParameters[i].ParameterName = txt[i];
            }
            return sqlParameters;
        }
        public T[] AssemblingTable<T>(string ProcedureName, SqlParameter[] Parameters) where T : Interface1, new()
        {
            List<T> values = new List<T>();
            using (SqlDataReader sdr = ProcedureInvokeSDR(ProcedureName, Parameters)) 
            {
                T t;
                if (R)
                {
                    do
                    {
                        t = new T();
                        t.Initialize(sdr);
                        values.Add(t);
                    }
                    while (sdr.Read());
                }
                sdr.Close();
            }
            return values.ToArray();
        }
        public T[] AssemblingTable<T>(string ProcedureName) where T: Interface1, new()
        {
            List<T> values = new List<T>();
            using (SqlCommand sq = new SqlCommand(ProcedureName, Connection))
            {
                sq.CommandType = CommandType.StoredProcedure;
                sq.CommandText = ProcedureName;
                using (SqlDataReader reader = sq.ExecuteReader())
                {
                    T t;
                    while (reader.Read())
                    {
                        t = new T();
                        t.Initialize(reader);
                        values.Add(t);
                    }
                    reader.Close();
                }
            }
            return values.ToArray();
        }
    }
}
