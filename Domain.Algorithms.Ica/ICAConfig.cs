using System.Collections.Generic;
using Domain.Models;

namespace Domain.Algorithms.Ica
{
    public class IcaConfig
    {
        public int NumberOfTasks { get; set; } = 2;
        public int CandidatesPerTask { get; set; } = 200;
        public int InitialEmpiresCount { get; set; } = 10;
        public int InitialColoniesCount { get; set; } = 190;
        public double Zeta { get; set; } = 0.1;
        public List<QualityAttributeWeight> QualityAttributeWeights { get; set; }
    }
}
