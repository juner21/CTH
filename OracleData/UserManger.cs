using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.ComponentModel.Composition;
using InterfaceContract;

namespace OracleData
{
    [Export(typeof(IUserData))]
    public class UserManger //: IUserData
    {
        public User Get()
        {

            return new User
            {
                Id = "132131231",
                UserName = "Oracle",
                Pwd = "hanfei"
            };
        }


    }
}
