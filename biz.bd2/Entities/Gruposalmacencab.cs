﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd2.Entities
{
    public partial class Gruposalmacencab
    {
        public Gruposalmacencab()
        {
            Gruposalmacenlins = new HashSet<Gruposalmacenlin>();
        }

        public int Idgrupo { get; set; }
        public string Descripcion { get; set; }
        public int? Codvisible { get; set; }

        public virtual ICollection<Gruposalmacenlin> Gruposalmacenlins { get; set; }
    }
}