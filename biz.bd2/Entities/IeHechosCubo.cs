﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd2.Entities
{
    public partial class IeHechosCubo
    {
        public int IdHecho { get; set; }
        public int IdCubo { get; set; }

        public virtual IeCubo IdCuboNavigation { get; set; }
        public virtual IeHecho IdHechoNavigation { get; set; }
    }
}