﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd1.Entities
{
    public partial class Procesosespeciale
    {
        public Procesosespeciale()
        {
            Procesosespecialesexecs = new HashSet<Procesosespecialesexec>();
            Procesosespecialessqls = new HashSet<Procesosespecialessql>();
            Procesosespecialesusus = new HashSet<Procesosespecialesusu>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Procesosespecialesexec> Procesosespecialesexecs { get; set; }
        public virtual ICollection<Procesosespecialessql> Procesosespecialessqls { get; set; }
        public virtual ICollection<Procesosespecialesusu> Procesosespecialesusus { get; set; }
    }
}