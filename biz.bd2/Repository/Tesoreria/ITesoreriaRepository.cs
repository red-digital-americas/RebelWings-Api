using biz.bd2.Entities;
using biz.bd2.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.bd2.Repository.Tesoreria
{
    public interface ITesoreriaRepository : IGenericRepository<Tesorerium>
    {
        double GetVolado(int id_sucursal, DateTime hora);
    }
}
