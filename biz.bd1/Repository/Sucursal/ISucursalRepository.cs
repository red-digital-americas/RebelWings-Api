using biz.bd1.Entities;
using biz.bd1.Models;
using biz.bd1.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.bd1.Repository.Sucursal
{
    public interface ISucursalRepository : IGenericRepository<RemFront>
    {
        Boolean getSucursalById(int sucursalId);
        List<RemFront> getSucursales();
        List<TransferDto> GetBranchList();
    }
}
