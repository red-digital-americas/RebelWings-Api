﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd1.Entities
{
    public partial class Tipostarjetacondicionesrtl
    {
        public int Idtipotarjeta { get; set; }
        public int Idfront { get; set; }
        public int Dia { get; set; }
        public int? Idtarifav { get; set; }
        public double? Dto { get; set; }

        public virtual Tipostarjetum IdtipotarjetaNavigation { get; set; }
    }
}