using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LifeLogger.DataAccess.DbInitializer
{
    public interface IDbInitializer
    {
        void Initialize();
    }
}