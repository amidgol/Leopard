using System.Collections.Generic;
using WebServiceComposition.Domain;

namespace WebServiceComposition.Algorithms.Ica.Models
{
    public class Empire<TCountry> where TCountry:class
    {
        public TCountry Imperialist { get; set; }
        public List<TCountry> Colonies { get; set; }
        public IFitnessCalculator<TCountry> FitnessCalculator { get; set; }
        public double TotalCost { get; set; }
        public double NormalizedPower { get; set; }

        public override string ToString()
        {
            return $"Colonies: {Colonies.Count}, Power:{NormalizedPower}";
        }
    }
}
