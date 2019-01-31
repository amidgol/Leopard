using System;

namespace WebServiceComposition.Domain.Models
{
    public class BaseEntity
    {
        public double Id { get; set; }
        public DateTime CreationDateTime { get; set; } = DateTime.Now;
    }
}
