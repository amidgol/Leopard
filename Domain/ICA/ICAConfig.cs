using System;
using System.Collections.Generic;
using System.Text;
using Domain.Models;

namespace Domain.ICA
{
    public class IcaConfig
    {
        public int InitialPopulationCount { get; set; } = 200;
        public int InitialEmpiresCount { get; set; } = 5;
        public int InitialColoniesCount { get; set; } = 195;

        public List<QualityAttributeWeight> QualityAttributeWeights { get; set; }
    }
}
