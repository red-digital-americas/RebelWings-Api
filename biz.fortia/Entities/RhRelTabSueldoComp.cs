﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.fortia.Entities
{
    public partial class RhRelTabSueldoComp
    {
        public int ClaSueComp { get; set; }
        public int ClaEmpresa { get; set; }
        public DateTime? FechaUltCambio { get; set; }

        public virtual RhTabSueldoComp ClaSueCompNavigation { get; set; }
    }
}