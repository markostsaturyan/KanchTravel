using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.DataManagement.Exceptions
{
    public class UserManagementException:Exception
    {
        public int ExceptionCode{ get; set; }
    }
}
