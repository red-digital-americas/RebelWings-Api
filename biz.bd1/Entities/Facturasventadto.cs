﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd1.Entities
{
    public partial class Facturasventadto
    {
        public string Numserie { get; set; }
        public int Numero { get; set; }
        public string N { get; set; }
        public int Linea { get; set; }
        public int? Numlindoc { get; set; }
        public int? Coddto { get; set; }
        public string Tipo { get; set; }
        public int? Secuencia { get; set; }
        public double? Base { get; set; }
        public double? Dtocargo { get; set; }
        public double? Importe { get; set; }
        public double? Udsdto { get; set; }
        public double? Importeunitariodesc { get; set; }
        public int? Tipoimpuesto { get; set; }
        public double? Iva { get; set; }
        public double? Req { get; set; }
        public int? Tipodto { get; set; }

        public virtual Facturasventum NNavigation { get; set; }
    }
}