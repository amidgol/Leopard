using System.Collections.Generic;

namespace WebServiceComposition.Domain.Models
{
    public class WebService: BaseEntity
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public IEnumerable<QualityAttributeValue> QualityAttributeValues { get; set; }

        public double Cost { get; set; }

        public override string ToString()
        {
            return $"{Title}, Cost: {Cost}";
        }
    }
}
