using Leopard.Domain.Models;

namespace Leopard.Algorithms.Pso
{
    public class PsoConfig:Config
    {
        public double Omega { get; set; } = 0.3;
        public double C1 { get; set; } = 0.3;
        public double C2 { get; set; } = 0.4;
    }
}
