﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd2.Entities
{
    public partial class Repartostockcab
    {
        public Repartostockcab()
        {
            Repartostocks = new HashSet<Repartostock>();
        }

        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public DateTime? Fecha { get; set; }
        public string Almorig { get; set; }

        public virtual ICollection<Repartostock> Repartostocks { get; set; }
    }
}