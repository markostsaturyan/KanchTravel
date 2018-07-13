using Authentication.DataManagement.BusinessLogicLayer.BusiessLayerDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.DataManagement.BusinessLogicLayer
{
    public interface IUserRepository
    {
        User FindUserAsync(string userName);
        User FindUserAsync(long id);
    }
}
