﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd2.Entities
{
    public partial class Serviciosauditorium
    {
        public int Id { get; set; }
        public string Serie { get; set; }
        public int Numero { get; set; }
        public double Idservicio { get; set; }
        public DateTime Dia { get; set; }
        public DateTime Hora { get; set; }
        public int Codempleado { get; set; }
        public int Estado { get; set; }
        public string Observaciones { get; set; }
    }
}