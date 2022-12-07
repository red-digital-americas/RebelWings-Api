using biz.bd1.Entities;
using biz.bd1.Models;
using biz.bd1.Repository.Remision;
using dal.bd1.DBContext;
using dal.bd1.Repository.Generic;

namespace dal.bd1.Repository.Remision;

public class RemisionRepository : GenericRepository<Albventacab>, IRemisionRepository
{
    public RemisionRepository(BD1Context context) : base(context)
    {
    }

    public List<Remisiones> GetRemisionesByBranch(string branchName, DateTime startDateTime, DateTime endDateTime)
    {
        var albaranesList = (from a in _context.Albventacabs
            join c in _context.Clientes on a.Codcliente equals c.Codcliente
            // join al in _context.Albventalins on a.Numserie equals al.Numserie
            where a.Norecibido.Equals("T") && a.Fecha >= startDateTime && a.Fecha <= endDateTime &&
                  c.Nombrecomercial.Contains(branchName)
            select new Remisiones()
            {
                NumSerie = a.Numserie,
                NumAlbaran = a.Numalbaran,
                Fecha = a.Fecha,
                RemisionesDetails = (from al in _context.Albventalins
                    where al.Numserie == a.Numserie && al.Numalbaran == a.Numalbaran
                    select new RemisionesDetail()
                    {
                        Descripcion = al.Descripcion,
                        Dto = al.Dto,
                        Precio = al.Precio,
                        Referencia = al.Referencia,
                        Total = al.Total,
                        UnidadTotal = al.Unidadestotal
                    }).ToList()
            }).ToList();
        return albaranesList;
    }
}