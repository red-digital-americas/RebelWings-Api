﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd1.Entities
{
    public partial class IeValoresFiltrosCuboSb
    {
        public int IdScoreboard { get; set; }
        public int IdGraficaSb { get; set; }
        public int IdFiltroCuboSb { get; set; }
        public int IdValorFiltroCuboSb { get; set; }
        public int IdValorCompuesto { get; set; }
        public int IdNivel { get; set; }
        public string Valor { get; set; }

        public virtual IeFiltrosCuboSb Id { get; set; }
    }
}