using biz.bd1.Entities;
using biz.bd1.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.bd1.Repository.Tesoreria
{
    public interface ITesoreriaRepository : IGenericRepository<Tesorerium>
    {
        double GetVolado(int id_sucursal, DateTime hora);
    }
}
