using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ICA
{
    public class Empire<TCountry> where TCountry:class
    {
        public TCountry Imperialist { get; set; }
        public List<TCountry> Colonies { get; set; }
        public IFitnessCalculator<TCountry> FitnessCalculator { get; set; }
        public double TotalCost { get; set; }
        public double NormalizedPower { get; set; }
    }
}
