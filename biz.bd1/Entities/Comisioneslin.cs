﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd1.Entities
{
    public partial class Comisioneslin
    {
        public int Cod { get; set; }
        public int Grupoarticulo { get; set; }
        public double Desde { get; set; }
        public double Hasta { get; set; }
        public double? Porcentaje { get; set; }

        public virtual Comisionescab CodNavigation { get; set; }
    }
}