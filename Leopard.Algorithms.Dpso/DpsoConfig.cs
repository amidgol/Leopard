using System;
using System.Collections.Generic;
using System.Text;
using Leopard.Domain.Models;

namespace Leopard.Algorithms.Dpso
{
    public class DpsoConfig:Config
    {
        public double Omega { get; set; } = 0.3;
        public double C1 { get; set; } = 0.3;
        public double C2 { get; set; } = 0.4;
    }
}
