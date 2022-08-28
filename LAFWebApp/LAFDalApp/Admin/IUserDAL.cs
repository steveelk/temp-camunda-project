using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAFDalApp.Admin
{
    public interface IUserDAL
    {
        User getUser(long id);
    }

    public class UserAccessFactory
    {

        private static IUserDAL _instance;

        private static object _locker = new object();

        public static IUserDAL GetInstance()
        {
            if (_instance == null)
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        _instance = new UserDAL();
                    }
                }
            }
            return _instance;
        }
    }
}
