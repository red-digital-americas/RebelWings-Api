using biz.bd2.Entities;
using biz.bd2.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.bd2.Repository.Sucursal
{
    public interface ISucursalRepository : IGenericRepository<RemFront>
    {
        Boolean getSucursalById(int sucursalId);
        List<RemFront> getSucursales();
        List<biz.bd2.Models.TransferDto> GetBranchList();
    }
}
