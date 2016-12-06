using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CongoData.DataAccess.Abstract {
    public interface ICongoRepository {
        Account GetAccount(int id);
    }
}
