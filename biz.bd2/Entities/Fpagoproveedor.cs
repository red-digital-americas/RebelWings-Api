﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd2.Entities
{
    public partial class Fpagoproveedor
    {
        public int Codproveedor { get; set; }
        public string Tipo { get; set; }
        public string Codformapago { get; set; }
        public int Coddtopp { get; set; }
        public double? Dtopp { get; set; }
        public double? Cantidad { get; set; }
        public string Serie { get; set; }

        public virtual Formaspago CodformapagoNavigation { get; set; }
        public virtual Proveedore CodproveedorNavigation { get; set; }
    }
}