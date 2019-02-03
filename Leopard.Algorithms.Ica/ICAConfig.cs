using Leopard.Domain.Models;

namespace Leopard.Algorithms.Ica
{
    public class IcaConfig:Config
    {
        public int InitialEmpiresCount { get; set; } = 10;
        public double Zeta { get; set; } = 0.1;

        public double Alpha { get; set; } = 0.7;
        public double Beta { get; set; } = 0.1;
        public double Gamma { get; set; } = 0.2;
    }
}
