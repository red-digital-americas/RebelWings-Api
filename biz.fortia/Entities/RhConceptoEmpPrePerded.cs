﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.fortia.Entities
{
    public partial class RhConceptoEmpPrePerded
    {
        public int ClaEmpresa { get; set; }
        public int ClaConcepto { get; set; }
        public int ClaPerded { get; set; }
        public DateTime? FechaUltCambio { get; set; }

        public virtual RhConcepto Cla { get; set; }
    }
}