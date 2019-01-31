using System.Collections.Generic;
using Domain.Models;

namespace Domain.Algorithms.Ica
{
    public class IcaConfig:Config
    {
        public int InitialEmpiresCount { get; set; } = 10;
        public double Zeta { get; set; } = 0.1;
    }
}
