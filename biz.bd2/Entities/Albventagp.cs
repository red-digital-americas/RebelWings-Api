﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd2.Entities
{
    public partial class Albventagp
    {
        public string Numserie { get; set; }
        public int Numalbaran { get; set; }
        public string N { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? Hora { get; set; }

        public virtual Albventacab NNavigation { get; set; }
    }
}