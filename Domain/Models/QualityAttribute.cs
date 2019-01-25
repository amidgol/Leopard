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
        public string Unit { get; set; }
        public double MinPossibleValue { get; set; }
        public double MaxPossibleValue { get; set; }
    }
}
