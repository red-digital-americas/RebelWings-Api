﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd2.Entities
{
    public partial class Asignaciondptovendedore
    {
        public int Coddpto { get; set; }
        public int Codvendedor { get; set; }

        public virtual Dptovendedore CoddptoNavigation { get; set; }
        public virtual Vendedore CodvendedorNavigation { get; set; }
    }
}