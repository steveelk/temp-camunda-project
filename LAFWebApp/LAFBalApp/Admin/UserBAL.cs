using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAFDalApp.Admin
{
    public class UserBAL
    {
        private IUserDAL _dataAccess = UserAccessFactory.GetInstance();

        private static UserBAL _instance = null;
        private static object _locker = new object();

        public static UserBAL Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new UserBAL();
                        }
                    }
                }
                return _instance;
            }
        }

        private UserBAL()
        {
        }

        public User getUser(long id)
        {
            //_dataAccess.getUser(id);
            User user = new User();
            user.Id = 1;
            user.UserName = "asd";
            user.UserRole = "admin";

            return user;
        }

        public List<string> getPermissions()
        {
            List<string> permissions = new List<string>();
            permissions.Add("CanDownload");
            permissions.Add("CanAttach");

            return permissions;
        }
    }
}
