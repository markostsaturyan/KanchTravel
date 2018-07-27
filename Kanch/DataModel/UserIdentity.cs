using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Kanch.DataModel
{
    public class UserIdentity
    {
        public int Id { get; set; }
        public string AuthenticationMethod { get; set; }
        public List<Claim> Claims { get; set; }
    }
}
