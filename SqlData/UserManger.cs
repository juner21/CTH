using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using InterfaceContract;
using System.ComponentModel.Composition;

namespace SqlData
{

    [Export(typeof(IUserData))]
    public class UserManger : IUserData
    {
        public User Get()
        {


            return new User
            {
                Id = "132131231",
                UserName = "Sql Server!!",
                Pwd = "hanfei"
            };
        }

        public bool Login(string userName, string pwd)
        {
            throw new NotImplementedException();
        }
    }
}
