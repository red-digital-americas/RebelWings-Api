﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd1.Entities
{
    public partial class FactConcepto
    {
        public decimal IdConcepto { get; set; }
        public string IdFactura { get; set; }
        public string Codigo { get; set; }
        public string Unidades { get; set; }
        public string DescripcionConcepto { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public decimal? ImporteNeto { get; set; }
        public decimal? DescuentoPorc { get; set; }
        public decimal? Impuesto { get; set; }
    }
}