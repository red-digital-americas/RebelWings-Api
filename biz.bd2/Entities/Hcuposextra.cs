﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd2.Entities
{
    public partial class Hcuposextra
    {
        public int Idhotel { get; set; }
        public int Idcupo { get; set; }
        public int Codarticulo { get; set; }
        public int Posicion { get; set; }
        public byte[] Version { get; set; }

        public virtual Articulo1 CodarticuloNavigation { get; set; }
        public virtual Hcupo IdcupoNavigation { get; set; }
        public virtual Hotele IdhotelNavigation { get; set; }
    }
}