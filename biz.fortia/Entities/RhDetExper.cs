﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.fortia.Entities
{
    public partial class RhDetExper
    {
        public int ClaEncExper { get; set; }
        public int ClaDetExper { get; set; }
        public string NomDetExper { get; set; }
        public DateTime? FechaUltCambio { get; set; }
        public string NomDetExperIng { get; set; }

        public virtual RhEncExper ClaEncExperNavigation { get; set; }
    }
}