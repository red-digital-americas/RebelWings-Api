﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd1.Entities
{
    public partial class Promocionesconseguida
    {
        public int Idtarjeta { get; set; }
        public int Idfront { get; set; }
        public int Idlinea { get; set; }
        public string Mostrar { get; set; }

        public virtual Tarjeta IdtarjetaNavigation { get; set; }
    }
}