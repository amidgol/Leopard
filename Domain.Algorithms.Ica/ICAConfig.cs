using System.Collections.Generic;
using Domain.Models;

namespace Domain.Algorithms.Ica
{
    public class IcaConfig
    {
        public int InitialPopulationCount { get; set; } = 200;
        public int InitialEmpiresCount { get; set; } = 5;
        public int InitialColoniesCount { get; set; } = 195;
        public double Zeta { get; set; } = 0.1;
        public List<QualityAttributeWeight> QualityAttributeWeights { get; set; }
    }
}
