using biz.bd2.Entities;
using biz.bd2.Models;
using biz.bd2.Repository.Albaran;
using biz.bd2.Repository.Generic;
using dal.bd2.DBContext;
using dal.bd2.Repository.Generic;

namespace dal.bd2.Repository.Albaran;

public class AlbaranRepository : GenericRepository<Albcompracab>, IAlbaranRepository
{
    public AlbaranRepository(BD2Context context) : base(context)
    {
    }

    public List<Albaranes> getAlbaranesList(string branchName, int pageNumber, int pageSize)
    {
        DateTime today = DateTime.Now.AddDays(-1);
        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        var albaranesList = (from a in _context.Albcompracabs
            join c in _context.Clientes on a.Codcliente equals c.Codcliente
            join al in _context.Albcompralins on a.Numserie equals al.Numserie
            where a.Norecibido.Equals("T") && c.Nombrecomercial.Contains(branchName)
            select new Albaranes()
            {
                NumSerie = a.Numserie,
                NombreCliente = c.Nombrecliente,
                NombreComercial = c.Nombrecomercial,
                Descripcion = al.Descripcion,
                NumAlbaran = a.Numalbaran,
                N = a.N,
                AlbaranDate = a.Fechaalbaran.Value,
                AlbaranTime = a.Hora.Value.TimeOfDay,
                Id = 0,
                Status = ""
            }).Skip(pageNumber - 1).Take(pageSize).ToList();
        return albaranesList;
    }
}