using System;
using Leopard.Domain.Enums;

namespace Leopard.Domain.Models
{
    public class QualityAttribute
    {
        public QualityAttributeType Type { get; set; }
        public string Title { get; set; }
        public Type Scale { get; set; } = typeof(double);
        public string Unit { get; set; }
        public double MinPossibleValue { get; set; }
        public double MaxPossibleValue { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
