﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd2.Entities
{
    public partial class Hestadosdefecto
    {
        public int Codigo { get; set; }
        public string Idestado { get; set; }
        public bool? Poner { get; set; }
        public int Idhotel { get; set; }
        public byte[] Version { get; set; }

        public virtual Hestadoshabitacione IdestadoNavigation { get; set; }
    }
}