﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd2.Entities
{
    public partial class Tiposretencion
    {
        public Tiposretencion()
        {
            Proveedoresretencionesivas = new HashSet<Proveedoresretencionesiva>();
            Tiposretencionlins = new HashSet<Tiposretencionlin>();
        }

        public int Tipo { get; set; }
        public string Descripcion { get; set; }
        public string Cuenta { get; set; }
        public int? Tipofacturacion { get; set; }
        public int? Tiporetencion { get; set; }
        public double? Porcbase { get; set; }
        public double? Sustraendo { get; set; }
        public int? Idclave { get; set; }
        public string Claveecu { get; set; }

        public virtual Tiposretencionclave IdclaveNavigation { get; set; }
        public virtual ICollection<Proveedoresretencionesiva> Proveedoresretencionesivas { get; set; }
        public virtual ICollection<Tiposretencionlin> Tiposretencionlins { get; set; }
    }
}