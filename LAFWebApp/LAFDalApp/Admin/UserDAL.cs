using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAFDalApp.Admin
{
    public class UserDAL : SqlDAL, IUserDAL
    {
        public UserDAL()
           : base()
        {
        }

        public User getUser(long id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                User user = null;

                try
                {
                    conn.Open();
                    const string query = "SELCET * FROM Users WHERE ID=@ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.Add(CreateParam("@ID", id));
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    if (dataReader.Read())
                    {
                        user = new User();
                       
                    }
                    dataReader.Close();

                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                return user;
            }
        }
    }
}
