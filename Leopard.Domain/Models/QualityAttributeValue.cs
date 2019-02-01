namespace Leopard.Domain.Models
{
    public class QualityAttributeValue
    {
        public QualityAttribute QualityAttribute { get; set; }
        public object Value { get; set; }


        public override string ToString()
        {
            return $"{QualityAttribute.Title}: {Value}";
        }
    }
}