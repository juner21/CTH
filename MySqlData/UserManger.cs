using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using InterfaceContract;
using System.ComponentModel.Composition;

namespace MySqlData
{

    [Export(typeof(IUserData))]
    public class UserManger// : IUserData
    {
        public User Get()
        {


            return new User
            {
                UserId = "132131231",
                UserName = "MySql",
                Pwd = "hanfei"
            };
        }


    }
}
