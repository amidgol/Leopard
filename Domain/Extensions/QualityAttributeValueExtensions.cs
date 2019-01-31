using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Models;

namespace Domain.Extensions
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
