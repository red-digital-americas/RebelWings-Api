﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.fortia.Entities
{
    public partial class RhServiciosReq
    {
        public RhServiciosReq()
        {
            RhSolReqServs = new HashSet<RhSolReqServ>();
        }

        public int ClaServicio { get; set; }
        public string NomServicio { get; set; }
        public int? ClaUsuario { get; set; }
        public int? ClaSistema { get; set; }
        public int? ClaProceso { get; set; }
        public DateTime? FechaUltCambio { get; set; }

        public virtual ICollection<RhSolReqServ> RhSolReqServs { get; set; }
    }
}