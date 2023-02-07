using biz.bd2.Repository.Stock;
using dal.bd2.DBContext;
using dal.bd2.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using biz.bd2.Models;
using Microsoft.EntityFrameworkCore;

namespace dal.bd2.Repository.Stock
{
    public class StockRepository : GenericRepository<biz.bd2.Entities.Stock>, IStockRepository
    {
        public StockRepository(BD2Context context) : base(context)
        {

        }
        public List<biz.bd2.Models.StockDto> GetStock(int id_sucursal)
        {
            List<biz.bd2.Models.StockDto> _stock = new List<biz.bd2.Models.StockDto>();
            List<biz.bd2.Models.StockDto> _stock2 = new List<biz.bd2.Models.StockDto>();
            var timeNow = DateTime.Now;
            var serie = _context.RemCajasfronts.FirstOrDefault(x => x.Idfront == id_sucursal).Codalmventas;
            if (serie != null)
            {
                _stock = _context.Stocks
                    .Join(_context.Articuloscamposlibres,
                    art => art.Codarticulo,
                    stk => stk.Codarticulo,
                    (art, stk) => new biz.bd2.Models.StockDto()
                    {
                      Codalmacen = art.Codalmacen,
                      Codarticulo = stk.Codarticulo,
                      Regulariza = stk.Regulariza,
                      Unidadessat = stk.Unidadessat,
                      Unidadmedida = stk.UnidadMedida,
                    })
                    .Join(_context.Articulos1,
                    art => art.Codarticulo,
                    stk => stk.Codarticulo,
                    (art, stk) => new biz.bd2.Models.StockDto()
                    {
                      Codalmacen = art.Codalmacen,
                      Descripcion = stk.Descripcion,
                      Codarticulo = art.Codarticulo,
                      Regulariza = art.Regulariza,
                      Unidadessat = art.Unidadessat,
                      Unidadmedida = art.Unidadmedida,
                    })
                    .Where(s => s.Codalmacen == serie && s.Regulariza == "T").ToList();

            }
            if (timeNow.Hour < 3) {

                _stock = _stock.Where(s => !_context.Moviments.Where(es => es.Fecha == DateTime.Now.Date.AddDays(-1) && es.Codarticulo == s.Codarticulo && es.Codalmacenorigen == s.Codalmacen && es.Codalmacendestino == "" && es.Hora.Value.Hour > 3).Any()).ToList();
                _stock2 = _stock.Where(s => !_context.Moviments.Where(es => es.Fecha == DateTime.Now.Date && es.Codarticulo == s.Codarticulo && es.Codalmacenorigen == s.Codalmacen && es.Codalmacendestino == "").Any()).ToList();
                if(_stock.LongCount() > 0){

                    if(_stock2.LongCount() > 0) {

                         return _stock;

                    }
                    else{
                         return _stock2;

                    }
                    
                }
                else {

                         return _stock;
           
                }
            }
            else {

               _stock = _stock.Where(s => !_context.Moviments.Where(es => es.Fecha == DateTime.Now.Date && es.Codarticulo == s.Codarticulo && es.Codalmacenorigen == s.Codalmacen && es.Codalmacendestino == "" && es.Hora.Value.Hour > 3).Any()).ToList();
               return _stock;
            }

             

             
        }
        public decimal StockValidate(int id_sucursal, int codarticulo)
        {
            decimal _stock = 0;
            var serie = _context.RemCajasfronts.FirstOrDefault(x => x.Idfront == id_sucursal).Codalmventas;
            if (serie != null)
            {
                _stock = (decimal)_context.Stocks
                    .Join(_context.Articuloscamposlibres,
                    art => art.Codarticulo,
                    stk => stk.Codarticulo,
                    (art, stk) => new
                    {
                      Codalmacen = art.Codalmacen,
                      Codarticulo = stk.Codarticulo,
                      Regulariza = stk.Regulariza,
                      Unidadessat = stk.Unidadessat,
                      Unidadmedida = stk.UnidadMedida,
                      art.Stock1

                    })
                    .Join(_context.Articulos1,
                    art => art.Codarticulo,
                    stk => stk.Codarticulo,
                    (art, stk) => new
                    {
                      Codalmacen = art.Codalmacen,
                      Descripcion = stk.Descripcion,
                      Codarticulo = art.Codarticulo,
                      Regulariza = art.Regulariza,
                      Unidadessat = art.Unidadessat,
                      Unidadmedida = art.Unidadmedida,
                      art.Stock1
                    })
                    .SingleOrDefault(s => s.Codalmacen == serie && s.Codarticulo == codarticulo && s.Regulariza == "T").Stock1.Value;

            }

            return _stock;
        }
        public biz.bd2.Models.StockDto UpdateStock(int codArticulo, string codAlmacen, double cantidad)
        {
            biz.bd2.Models.StockDto _stock = new biz.bd2.Models.StockDto();

            //FECHA DE INVENTARIOS
            var tablaInv = DateTime.Now.Date.AddDays(-1);
            if (_context.Inventarios.FirstOrDefault(x => x.Codalmacen == codAlmacen && x.Fecha == DateTime.Now.Date) != null) {
              tablaInv  = _context.Inventarios.FirstOrDefault(x => x.Codalmacen == codAlmacen && x.Fecha == DateTime.Now.Date).Fecha;
            }
            else { tablaInv = DateTime.Now.Date.AddDays(-1); }

            var __stock = _context.Stocks.FirstOrDefault(x => x.Codarticulo == codArticulo && x.Codalmacen == codAlmacen);
            double _stockAnterior = __stock.Stock1.Value;
            if (__stock != null)
            {
                __stock.Stock1 = cantidad;

                _stock.Codalmacen = codAlmacen;
                _stock.Descripcion = _stock.Descripcion;
                _stock.Codarticulo = codArticulo;
                _stock.Regulariza = "T";
                _stock.Unidadessat = "";
                _stock.Stock1 = cantidad;

                _context.Stocks.Update(__stock);
              if (tablaInv != DateTime.Now.Date) {
                biz.bd2.Entities.Inventario _inventario = new biz.bd2.Entities.Inventario();
                _inventario.Fecha = DateTime.Now.Date;
                _inventario.Codalmacen = codAlmacen;
                _inventario.Tipovaloracion = -3;
                _inventario.Serie = "";
                _inventario.Numero = 0;
                _inventario.Codvendedor = -1;
                _inventario.Completo = "F";
                _inventario.Metodo = 1;
                _inventario.Inicial = "F";
                _inventario.Bloqueado = "F";
                _inventario.Tipovaloraciondmn = null;
                _inventario.Estado = 0;
                _inventario.Escierre = false;
                _inventario.EnlaceEjercicio = null;
                _inventario.EnlaceEmpresa = null;
                _inventario.EnlaceUsuario = null;
                _inventario.EnlaceAsiento = null;

                _context.Inventarios.Add(_inventario);

              }

                biz.bd2.Entities.Moviment _moviment = new biz.bd2.Entities.Moviment();
                _moviment.Codalmacenorigen = codAlmacen;
                _moviment.Codalmacendestino = "";
                _moviment.Numserie = "";
                _moviment.Codarticulo = codArticulo;
                _moviment.Talla = ".";
                _moviment.Color = ".";
                _moviment.Precio = _context.Articuloscamposlibres.FirstOrDefault(x => x.Codarticulo == codArticulo)?.Precioproveedor;
                _moviment.Fecha = DateTime.Now.Date;
                _moviment.Hora = Convert.ToDateTime("1899-12-30 " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ".000");
                _moviment.Codprocli = 0;
                _moviment.Tipo = "REG";
                _moviment.Unidades = cantidad;
                _moviment.Seriedoc = "";
                _moviment.Numdoc = 0;
                _moviment.Seriecompra = "";
                _moviment.Numfaccompra = -1;
                _moviment.Caja = "";
                _moviment.Stock = _stockAnterior;
                _moviment.Pvp = 0;
                _moviment.Codmonedapvp = 1;
                _moviment.Calcmovpost = "F";
                _moviment.Udmedida2 = 0;
                _moviment.Zona = "";
                _moviment.Pvpdmn = null;
                _moviment.Preciodmn = null;
                _moviment.Stock2 = 0;

                _context.Moviments.Add(_moviment);

                _context.SaveChanges();
                return _stock;
            }
            else
            {
                return _stock;
            }
        }

        public List<Mermas> GetMermas(int branch, DateTime initDate, DateTime endDate)
        {
            var mermas =
                from moviment in _context.Moviments
                join articulo1 in _context.Articulos1 on moviment.Codarticulo equals articulo1.Codarticulo
                join rem in _context.RemCajasfronts on new
                {
                    Codalmmermas = EF.Functions.Collate(moviment.Codalmacendestino, "Modern_Spanish_CI_AS"),
                    Ventas = EF.Functions.Collate(moviment.Codalmacenorigen, "Modern_Spanish_CI_AS")
                } equals new
                {
                    Codalmmermas = rem.Codalmmermas,
                    Ventas = rem.Codalmventas
                }
                join remFront in _context.RemFronts on rem.Idfront equals remFront.Idfront
                where (moviment.Fecha >= initDate && moviment.Fecha <= endDate) && rem.Idfront == branch
                orderby moviment.Fecha descending 
                select new Mermas()
                {
                    Description = articulo1.Descripcion,
                    Price = moviment.Precio.Value,
                    Unity = moviment.Unidades.Value,
                    UnitMeasure = articulo1.Unidadmedida
                };
            return mermas.ToList();
        }
    }
}
