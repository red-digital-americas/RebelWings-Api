﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.fortia.Entities
{
    public partial class RhSolProrrogaVacacione
    {
        public int FolProrroga { get; set; }
        public int ClaEmpresa { get; set; }
        public int ClaTrab { get; set; }
        public int DiasProrroga { get; set; }
        public int Antiguedad { get; set; }
        public string Observaciones { get; set; }
        public int Estatus { get; set; }
        public DateTime? FechaSolicita { get; set; }
        public DateTime? FechaAutoriza { get; set; }
        public int? ClaUsuarioAutoriza { get; set; }
        public DateTime FechaUltCambio { get; set; }
    }
}