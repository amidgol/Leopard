using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class WebService: BaseEntity
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public IEnumerable<QualityAttributeValue> QualityAttributeValues { get; set; }

        public double Cost { get; set; }

    }
}
