﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd2.Entities
{
    public partial class Temporalestyc
    {
        public int Codgrupo { get; set; }
        public int Codtabla { get; set; }
        public int Identificador { get; set; }
        public short? Tipo { get; set; }
        public int? Tipoinforme { get; set; }
        public short? Tipotallascomp { get; set; }
        public short? Numtallasparam { get; set; }
        public string Parametro { get; set; }
        public short? Tipoparamtot { get; set; }
        public string Valor { get; set; }
        public int? Codtitulo { get; set; }

        public virtual Icgnombresinforme Cod { get; set; }
    }
}