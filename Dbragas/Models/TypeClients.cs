﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBragas.Models
{
    public partial class TypeClients
    {
        public TypeClients()
        {
            Clients = new HashSet<Clients>();
        }

        [Key]
        public Guid? Id { get; set; }
        [StringLength(120)]
        [Unicode(false)]
        public string? Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("TypeClient")]
        public virtual ICollection<Clients> Clients { get; set; }
    }
}