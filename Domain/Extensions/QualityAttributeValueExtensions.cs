using WebServiceComposition.Domain.Models;

namespace WebServiceComposition.Domain.Extensions
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
