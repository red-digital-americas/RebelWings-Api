﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd1.Entities
{
    public partial class ComOperacione
    {
        public ComOperacione()
        {
            ComTramas = new HashSet<ComTrama>();
        }

        public int Idoperacion { get; set; }
        public string Nombreoperacion { get; set; }
        public string Inputoutput { get; set; }

        public virtual ICollection<ComTrama> ComTramas { get; set; }
    }
}