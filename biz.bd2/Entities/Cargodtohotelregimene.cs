﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd2.Entities
{
    public partial class Cargodtohotelregimene
    {
        public int Codarticulo { get; set; }
        public int Codregimen { get; set; }

        public virtual Cargodtohotel CodarticuloNavigation { get; set; }
        public virtual Articulosregimene CodregimenNavigation { get; set; }
    }
}