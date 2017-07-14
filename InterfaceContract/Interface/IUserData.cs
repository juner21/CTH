﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceContract
{
    public interface IUserData
    {

        User Get();

        bool Login(string userName, string pwd);
    }
}
