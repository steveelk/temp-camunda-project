using Microsoft.Extensions.Configuration;
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

        private int _minSaltSize = 8;

        private int _maxSaltSize = 16;

        private int _saltDigestSize = 32;

        private int _saltIterCount = 75000;
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

        public void SetPassword(string password)
        {
            string salt = PBKDF2SHA256.GenerateSalt(_minSaltSize, _maxSaltSize);
            string newPass = PBKDF2SHA256.PBKDF2SHA256GetString(_saltDigestSize, password, salt, _saltIterCount);
        }

        private bool ComparePBKDF2SHA256(string password, string saltedPassword, string salt)
        {
            string newSaltedPassword = PBKDF2SHA256.PBKDF2SHA256GetString(_saltDigestSize, password, salt,
                        _saltIterCount);

            if (newSaltedPassword == saltedPassword)
            {
                return true;
            }
            return false;
        }
    }
}
