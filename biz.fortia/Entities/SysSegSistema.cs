﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.fortia.Entities
{
    public partial class SysSegSistema
    {
        public SysSegSistema()
        {
            SysSegSistemaProcesos = new HashSet<SysSegSistemaProceso>();
        }

        public int ClaSistema { get; set; }
        public string NomSistema { get; set; }
        public int Orden { get; set; }
        public int ClaIcono { get; set; }

        public virtual ICollection<SysSegSistemaProceso> SysSegSistemaProcesos { get; set; }
    }
}