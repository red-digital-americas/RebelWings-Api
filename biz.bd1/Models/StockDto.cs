using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.bd1.Models
{
    public class StockDto
    {
        public string Codalmacen { get; set; }
        public string Descripcion { get; set; }
        public int Codarticulo { get; set; }
        public string Regulariza { get; set; }
        public string Unidadessat { get; set; }
        public string Unidadmedida { get; set; }
        public double? Stock1 { get; set; }
        public DateTime? Ultfecha { get; set; }
    }
}
