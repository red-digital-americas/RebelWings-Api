﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd1.Entities
{
    public partial class LastsalesAlbventalin
    {
        public string Numserie { get; set; }
        public int Numalbaran { get; set; }
        public string N { get; set; }
        public int Numlin { get; set; }
        public int? Codarticulo { get; set; }
        public string Referencia { get; set; }
        public string Descripcion { get; set; }
        public string Color { get; set; }
        public string Talla { get; set; }
        public double? Unid1 { get; set; }
        public double? Unid2 { get; set; }
        public double? Unid3 { get; set; }
        public double? Unid4 { get; set; }
        public double? Unidadestotal { get; set; }
        public double? Precio { get; set; }
        public double? Dto { get; set; }
        public double? Total { get; set; }
        public double? Udsexpansion { get; set; }
        public string Expandida { get; set; }
        public double? Totalexpansion { get; set; }
        public double? Iva { get; set; }
        public double? Req { get; set; }
        public double? Precioiva { get; set; }
        public int? Codformato { get; set; }

        public virtual LastsalesAlbventacab NNavigation { get; set; }
    }
}