﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd1.Entities
{
    public partial class Contactoscliente
    {
        public int Codcliente { get; set; }
        public string Cargo { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string EMail { get; set; }
        public int? Id { get; set; }
        public string Dptoedi { get; set; }
        public bool Facturacion { get; set; }
        public bool Tesoreria { get; set; }
        public string Mobil { get; set; }

        public virtual Cliente CodclienteNavigation { get; set; }
    }
}