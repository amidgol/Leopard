using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class BaseEntity
    {
        public double Id { get; set; }
        public DateTime CreationDateTime { get; set; } = DateTime.Now;
    }
}
