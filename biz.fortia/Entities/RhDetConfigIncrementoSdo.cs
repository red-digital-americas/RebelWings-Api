﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.fortia.Entities
{
    public partial class RhDetConfigIncrementoSdo
    {
        public int FolAuto { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public int? TipoIncremento { get; set; }
        public int? TipoSueldoIncr { get; set; }
        public double? Monto { get; set; }
        public int? TipoMov { get; set; }
        public int? Estatus { get; set; }
        public int? NumNomina { get; set; }
        public int? ClaPeriodo { get; set; }
        public int? ClaEmpresa { get; set; }
        public DateTime? FechaUltCambio { get; set; }

        public virtual RhEncConfigIncrementoSdo FolAutoNavigation { get; set; }
    }
}