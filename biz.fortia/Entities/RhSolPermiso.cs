﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.fortia.Entities
{
    public partial class RhSolPermiso
    {
        public int ClaEmpresa { get; set; }
        public int ClaTrab { get; set; }
        public int FolAuto { get; set; }
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaFinal { get; set; }
        public double? Dias { get; set; }
        public double? Horas { get; set; }
        public string Observaciones { get; set; }
        public byte? Estatus { get; set; }
        public DateTime? FechaSolicita { get; set; }
        public DateTime? FechaAutoriza { get; set; }
        public int? ClaUsuario { get; set; }
        public DateTime? FechaUltCambio { get; set; }
        public string Motivo { get; set; }
        public int? ClaFalta { get; set; }
    }
}