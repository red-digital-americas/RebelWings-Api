﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd2.Entities
{
    public partial class Articuloscommerce
    {
        public int Codarticulo { get; set; }
        public int Codidioma { get; set; }
        public byte[] Desccorta { get; set; }
        public byte[] Desclarga { get; set; }
        public string Desccortahtml { get; set; }
        public string Desclargahtml { get; set; }

        public virtual Articulo1 CodarticuloNavigation { get; set; }
    }
}