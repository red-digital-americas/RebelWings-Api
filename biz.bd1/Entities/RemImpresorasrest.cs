﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd1.Entities
{
    public partial class RemImpresorasrest
    {
        public int Idfront { get; set; }
        public string Nombre { get; set; }
        public string Modelo { get; set; }
        public short? Gruposecuencias { get; set; }

        public virtual RemFront IdfrontNavigation { get; set; }
    }
}