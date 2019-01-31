using System.Collections.Generic;
using System.Linq;
using WebServiceComposition.Domain.Models;

namespace WebServiceComposition.Domain.Extensions
{
    public static class QualityAttributeExtensions
    {
        public static QualityAttribute CalculateMinAndMax(this QualityAttribute qualityAttribute,
            List<WebService> webServices)
        {
            double max = 0;
            double min = (double)webServices.First()
                .QualityAttributeValues.First(q => q.QualityAttribute.Equals(qualityAttribute)).Value;

            foreach (WebService webService in webServices)
            {
                foreach (QualityAttributeValue qualityAttributeValue in webService.QualityAttributeValues)
                {
                    if (qualityAttributeValue.QualityAttribute.Equals(qualityAttribute))
                    {
                        if ((double)qualityAttributeValue.Value > max)
                            max = (double)qualityAttributeValue.Value;

                        if ((double)qualityAttributeValue.Value < min)
                            min = (double)qualityAttributeValue.Value;
                    }
                }
            }

            qualityAttribute.MaxPossibleValue = max;
            qualityAttribute.MinPossibleValue = min;

            return qualityAttribute;
        }
    }
}
