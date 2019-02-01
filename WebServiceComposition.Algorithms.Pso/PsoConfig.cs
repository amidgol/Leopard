using System;
using System.Collections.Generic;
using System.Text;
using WebServiceComposition.Domain.Models;

namespace WebServiceComposition.Algorithms.Pso
{
    public class PsoConfig:Config
    {
        public double C1 { get; set; } = 0.6;
        public double C2 { get; set; } = 0.4;
    }
}
