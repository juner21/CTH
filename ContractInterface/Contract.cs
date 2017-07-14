using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

namespace ContractInterface
{
    public class Contract
    {
        [Import]
        public IUserData UserData { get; set; }
    }
}
