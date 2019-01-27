using System.Collections.Generic;
using System.Linq;
using Domain.Algorithms.Ica.Extensions;
using Domain.Extensions;
using Domain.ICA;
using Domain.Models;

namespace Domain.Algorithms.Ica
{
    public class Ica : IAlgorithm<CompositionRequest, IcaConfig, CompositionPlan>
    {
        private readonly IcaConfig _icaConfig;

        public Ica(IcaConfig icaConfig)
        {
            _icaConfig = icaConfig;
        }
        public CompositionPlan Execute(CompositionRequest input, IcaConfig config)
        {
            List<CompositionPlan> countries = input.CreateInitialCountries().ToList();

            List<Empire<CompositionPlan>> empires = CreateInitialEmpires(countries).ToList();

            while (empires.Count>1)
            {
                foreach (Empire<CompositionPlan> empire in empires)
                {
                    empire.Assimilate(config.QualityAttributeWeights)
                        .UpdateAfterAssimilation()
                        .CalculateCost(_icaConfig.Zeta);
                }

                empires.NormalizePowers().Compete().EliminatePowerlessEmpires();
            }

            return empires.First().Imperialist;
        }

        private IEnumerable<Empire<CompositionPlan>> CreateInitialEmpires(List<CompositionPlan> countries)
        {
            foreach (var country in countries)
            {
                country.Cost = country.CalculateCost(_icaConfig.QualityAttributeWeights);
                country.Power = 1 - country.Cost;
            }

            countries = countries.OrderBy(x => x.Cost).ToList();

            IEnumerable<CompositionPlan> initialImperialists = countries
                .Take(_icaConfig.InitialEmpiresCount).ToList();

            double sum = 0;
            foreach (CompositionPlan imperialist in initialImperialists)
            {
                sum += imperialist.Cost;
            }

            List<Empire<CompositionPlan>> empires = new List<Empire<CompositionPlan>>
                (initialImperialists.Count());

            int skipCount = _icaConfig.InitialEmpiresCount;

            foreach (CompositionPlan imperialist in initialImperialists)
            {
                Empire<CompositionPlan> empire = new Empire<CompositionPlan>();

                imperialist.NormalizedPower = imperialist.Power / sum;

                int coloniesCount = (int)(imperialist.NormalizedPower * _icaConfig.InitialColoniesCount);

                empire.Imperialist = imperialist;
                empire.Colonies = countries.Skip(skipCount).Take(coloniesCount).ToList();

                empires.Add(empire);

                skipCount+=coloniesCount;
            }

            return empires;
        }

    }
}
