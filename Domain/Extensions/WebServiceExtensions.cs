using System.Collections.Generic;
using System.Linq;
using WebServiceComposition.Domain.Models;

namespace WebServiceComposition.Domain.Extensions
{
    public static class WebServiceExtensions
    {
        public static double CalculateCost(this WebService webService,
            List<QualityAttributeWeight> weights)
        {
            double cost = 0;

            foreach (QualityAttributeValue attributeValue in webService.QualityAttributeValues)
            {
                double weight = weights.First(x => x.QualityAttribute.Title
                    .Equals(attributeValue.QualityAttribute.Title)).Weight;

                cost += weight * attributeValue.GetNormalizedValue();
            }

            return cost;
        }
    }
}
