using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace LAFDalApp
{
    public abstract class SqlDAL
    {
        public IConfigurationRoot GetAppSettings()
        {
            string applicationExeDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            var builder = new ConfigurationBuilder()
            .SetBasePath(applicationExeDirectory)
            .AddJsonFile("appsettings.json");

            return builder.Build();
        }

        protected string _connectionString;

        public SqlDAL()
        {
            var appSettingsJson = GetAppSettings();
            var connectionString = appSettingsJson["ConnectionStrings:DefaultConnection"];
            _connectionString = connectionString;
        }

        //public SqlDAL(string connectionString)
        //{
        //    _connectionString = connectionString;
        //}

        protected void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected byte GetByte(object obj, byte defaultValue)
        {
            if (obj != null && obj.ToString() != string.Empty)
            {
                return byte.Parse(obj.ToString());
            }
            return defaultValue;
        }

        protected Int16 GetInt16(object obj, Int16 defaultValue)
        {
            if (obj != null && obj.ToString() != string.Empty)
            {
                return Int16.Parse(obj.ToString());
            }
            return defaultValue;
        }

        protected Int32 GetInt32(object obj, Int32 defaultValue)
        {
            if (obj != null && obj.ToString() != string.Empty)
            {
                return Int32.Parse(obj.ToString());
            }
            return defaultValue;
        }

        protected bool TryGetInt32(object obj, out Int32 value)
        {
            value = -1;
            if (obj != null && obj.ToString() != string.Empty)
            {
                value = Int32.Parse(obj.ToString());
                return true;
            }
            return false;
        }

        protected Int32 GetInt32(object obj)
        {
            if (obj != null && obj.ToString() != string.Empty)
            {
                return Int32.Parse(obj.ToString());
            }
            return 0;
        }

        protected Int64 GetInt64(object obj)
        {
            if (obj != null && obj.ToString() != string.Empty)
            {
                return Int64.Parse(obj.ToString());
            }
            return 0;
        }

        protected Int64 GetInt64(object obj, Int64 defaultValue)
        {
            if (obj != null && obj.ToString() != string.Empty)
            {
                return Int64.Parse(obj.ToString());
            }
            return defaultValue;
        }

        protected bool GetBool(object obj)
        {
            if (obj != null && obj.ToString() != string.Empty)
            {
                return bool.Parse(obj.ToString());
            }
            return false;
        }

        protected bool TryGetTimeSpan(object obj, out TimeSpan timeSpan)
        {
            if (obj != null && obj.ToString().Trim() != string.Empty)
            {
                timeSpan = TimeSpan.Parse(obj.ToString());
                return true;
            }
            else
            {
                timeSpan = TimeSpan.MinValue;
                return false;
            }
        }

        protected DateTime GetDateTime(object obj)
        {
            if (obj != null && obj.ToString() != string.Empty)
            {
                return (DateTime)obj;
            }
            return DateTime.MinValue;
        }

        protected DateTime GetDateTimeUTC(object obj)
        {
            if (obj != null && obj.ToString() != string.Empty)
            {
                DateTime dt = (DateTime)obj;
                dt = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
                return dt;
            }
            return DateTime.MinValue;
        }


        protected string GetString(object obj)
        {
            string str = obj.ToString();
            if (str == string.Empty)
                return null;
            return str;
        }

        protected byte[] GetBinary(object obj)
        {
            return GetBinary(obj, null);
        }

        protected byte[] GetBinary(object obj, byte[] defaultValue)
        {
            if (obj != null && obj.ToString() != string.Empty)
            {
                return (byte[])obj;
            }
            return defaultValue;
        }

        protected SqlParameter CreateParam(string paramName, long value)
        {
            SqlParameter param = new SqlParameter(paramName, SqlDbType.BigInt);
            param.Value = value;
            return param;
        }

        protected SqlParameter CreateParam(string paramName, object value, SqlDbType dbType, int size)
        {
            SqlParameter param = new SqlParameter(paramName, dbType, size);
            param.Value = value;
            return param;
        }

        protected SqlParameter CreateParam(string paramName, object value, SqlDbType dbType)
        {
            SqlParameter param = new SqlParameter(paramName, dbType);
            param.Value = value;
            return param;
        }

        protected SqlParameter CreateOutParam(string paramName, SqlDbType dbType, int size)
        {
            SqlParameter paramOut = new SqlParameter(paramName, dbType, size);
            paramOut.Direction = System.Data.ParameterDirection.Output;
            return paramOut;
        }

        protected SqlParameter CreateOutParam(string paramName, SqlDbType dbType)
        {
            SqlParameter paramOut = new SqlParameter(paramName, dbType);
            paramOut.Direction = System.Data.ParameterDirection.Output;
            return paramOut;
        }
    }
}