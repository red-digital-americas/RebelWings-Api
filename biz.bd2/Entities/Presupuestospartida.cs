﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd2.Entities
{
    public partial class Presupuestospartida
    {
        public string Numserie { get; set; }
        public int Numpresupuesto { get; set; }
        public string N { get; set; }
        public int Version { get; set; }
        public int Idpartida { get; set; }
        public string Area { get; set; }
        public string Descripcion { get; set; }
        public double? Total { get; set; }

        public virtual Presupuestoscab Presupuestoscab { get; set; }
    }
}