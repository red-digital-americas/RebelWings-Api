using biz.bd2.Entities;
using biz.bd2.Repository.Sucursal;
using dal.bd2.DBContext;
using dal.bd2.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.bd2.Repository.Sucursal
{
    public class SucursalRepository : GenericRepository<RemFront>, ISucursalRepository
    {
        public SucursalRepository(BD2Context context) : base(context)
        {

        }

        public Boolean getSucursalById(int sucursalId)
        {
            return _context.RemFronts.Any(x => x.Idfront == sucursalId) ? true : false;
        }

        public List<RemFront> getSucursales()
        {
            return _context.RemFronts.ToList();
        }
        public List<biz.bd2.Models.TransferDto> GetBranchList()
        {
            var branchList = _context.RemFronts.Select(s => new biz.bd2.Models.TransferDto()
            {
                Name = s.Titulo,
                Description = s.Direccion,
                BranchId = s.Idfront,
            }).ToList();
            return branchList;
        }

    }
}
