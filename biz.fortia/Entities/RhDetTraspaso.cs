﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.fortia.Entities
{
    public partial class RhDetTraspaso
    {
        public int FolTraspasoDet { get; set; }
        public int FolTraspaso { get; set; }
        public int ClaArticulo { get; set; }
        public double Cantidad { get; set; }
        public DateTime FechaUltCambio { get; set; }

        public virtual RhEncTraspaso FolTraspasoNavigation { get; set; }
    }
}