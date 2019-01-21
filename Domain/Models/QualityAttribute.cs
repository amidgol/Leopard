using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;

namespace Domain.Models
{
    public class QualityAttribute
    {
        public QualityAttributeType Type { get; set; }
        public string Title { get; set; }
        public Type Scale { get; set; } = typeof(double);
    }
}
