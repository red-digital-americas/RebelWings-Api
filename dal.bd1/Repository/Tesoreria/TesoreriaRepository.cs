using biz.bd1.Entities;
using biz.bd1.Repository.Tesoreria;
using dal.bd1.DBContext;
using dal.bd1.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.bd1.Repository.Tesoreria
{
    public class TesoreriaRepository : GenericRepository<Tesorerium>, ITesoreriaRepository
    {
        public TesoreriaRepository(BD1Context context) : base(context)
        {

        }
        public double GetVolado(int id_sucursal, DateTime hora)
        {
            double tesoresia = 0;
            double tesoresia1 = 0;
            
            var serieT = _context.RemCajasfronts.FirstOrDefault(x => x.Idfront == id_sucursal).Serietiquets;
            var serieF = _context.RemCajasfronts.FirstOrDefault(x => x.Idfront == id_sucursal).Seriefacturas;
          
            if (serieT != null || serieF != null)
            {
                if(hora == Convert.ToDateTime("01/01/1900").Date)
                {
                    tesoresia = _context.Tesoreria
                   .Where(s => s.Fechadocumento.Value.Date == DateTime.Now.Date && s.Origen == "C" && s.Tipodocumento == "F"
                   && s.Codtipopago == "1" && s.Serie == serieT /*&& s.Fechamodificado.Value.Date < DateTime.Now.Date*/
                   ).Sum(x => x.Importe).Value;

                   tesoresia1 = _context.Tesoreria
                   .Where(s => s.Fechadocumento.Value.Date == DateTime.Now.Date && s.Origen == "C" && s.Tipodocumento == "F"
                   && s.Codtipopago == "1" && s.Serie == serieF /*&& s.Fechamodificado.Value.Date < DateTime.Now.Date*/
                   ).Sum(x => x.Importe).Value;
                }
                else
                {
                    tesoresia = _context.Tesoreria
                   .Where(s => s.Fechadocumento.Value.Date == DateTime.Now.Date && s.Origen == "C" && s.Tipodocumento == "F"
                   && s.Codtipopago == "1" && s.Fechamodificado.Value > hora && s.Serie == serieT).Sum(x => x.Importe).Value;

                    tesoresia1 = _context.Tesoreria
                   .Where(s => s.Fechadocumento.Value.Date == DateTime.Now.Date && s.Origen == "C" && s.Tipodocumento == "F"
                   && s.Codtipopago == "1" && s.Fechamodificado.Value > hora && s.Serie == serieF).Sum(x => x.Importe).Value;
                }

            }
          
            return tesoresia + tesoresia1;
        }
    }
}
