﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace biz.bd1.Entities
{
    public partial class Tiposterminal
    {
        public Tiposterminal()
        {
            Favoritostiposterminals = new HashSet<Favoritostiposterminal>();
        }

        public int Idtipoterminal { get; set; }
        public string Descripcion { get; set; }
        public byte[] Version { get; set; }

        public virtual ICollection<Favoritostiposterminal> Favoritostiposterminals { get; set; }
    }
}