namespace WebServiceComposition.Domain.Models
{
    public class QualityAttributeWeight
    {
        public QualityAttribute QualityAttribute { get; set; }
        public double Weight { get; set; }

        public override string ToString()
        {
            return $"{QualityAttribute.Title}, Weight: {Weight}";
        }
    }
}