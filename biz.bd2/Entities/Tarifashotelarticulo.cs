﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd2.Entities
{
    public partial class Tarifashotelarticulo
    {
        public int Codtarifa { get; set; }
        public int Codarticulo { get; set; }
        public string Tipo { get; set; }
        public short? Tipovaloracion { get; set; }
        public byte[] Version { get; set; }

        public virtual Tarifashotel CodtarifaNavigation { get; set; }
    }
}