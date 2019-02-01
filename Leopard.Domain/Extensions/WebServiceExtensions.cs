using System.Collections.Generic;
using System.Linq;
using Leopard.Domain.Enums;
using Leopard.Domain.Models;

namespace Leopard.Domain.Extensions
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

                if (attributeValue.QualityAttribute.Type
                    .Equals(QualityAttributeType.CostOriented))
                {
                    cost += weight * attributeValue.GetNormalizedValue();
                }
                else
                {
                    cost += weight * (1 - attributeValue.GetNormalizedValue());
                }
            }

            return cost;
        }
    }
}
