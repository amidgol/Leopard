using Leopard.Domain.Models;

namespace Leopard.Domain.Extensions
{
    public static class QualityAttributeValueExtensions
    {
        public static double GetNormalizedValue(this QualityAttributeValue qualityAttributeValue)
        {
            double normal = ((double)qualityAttributeValue.Value - qualityAttributeValue.QualityAttribute.MinPossibleValue) /
                    (qualityAttributeValue.QualityAttribute.MaxPossibleValue - 
                     qualityAttributeValue.QualityAttribute.MinPossibleValue);

            return normal;
        }
   
    }
}
