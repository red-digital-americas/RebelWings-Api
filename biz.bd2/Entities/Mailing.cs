﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd2.Entities
{
    public partial class Mailing
    {
        public Mailing()
        {
            Mailingbitmaps = new HashSet<Mailingbitmap>();
        }

        public int Idinforme { get; set; }
        public string Cuerpo { get; set; }
        public string Cabecera { get; set; }
        public string Pie { get; set; }
        public int? Alturacabecera { get; set; }
        public int? Alturacuerpo { get; set; }
        public int? Alturapie { get; set; }
        public string Descripcion { get; set; }
        public double? MargenAr { get; set; }
        public double? MargenAb { get; set; }
        public double? MargenIzq { get; set; }
        public double? MargenDer { get; set; }
        public int? Tipopapel { get; set; }
        public double? Anchopapel { get; set; }
        public double? Altopapel { get; set; }
        public string Orientacion { get; set; }
        public byte[] Version { get; set; }

        public virtual ICollection<Mailingbitmap> Mailingbitmaps { get; set; }
    }
}