﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd2.Entities
{
    public partial class Seriescamposlibre
    {
        public string Serie { get; set; }
        public string Franquicia { get; set; }
        public string Consulta1 { get; set; }
        public string Consulta2 { get; set; }
        public int? Cliente { get; set; }
        public string Cuentacontable { get; set; }
        public string Seriefa { get; set; }
        public string Serienc { get; set; }
        public string Webpass { get; set; }

        public virtual Series SerieNavigation { get; set; }
    }
}