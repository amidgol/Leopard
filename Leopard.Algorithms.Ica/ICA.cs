using System;
using System.Collections.Generic;
using System.Linq;
using Leopard.Algorithms.Ica.Extensions;
using Leopard.Algorithms.Ica.Models;
using Leopard.Domain;
using Leopard.Domain.Extensions;
using Leopard.Domain.Models;

namespace Leopard.Algorithms.Ica
{
    public class Ica : IAlgorithm
    {
        public CompositionPlan Execute(CompositionRequest input, Action<string> display)
        {
            display("ICA started...\n");

            List<CompositionPlan> countries = input.CreateInitialPopulation().ToList();

            List<Empire<CompositionPlan>> empires = CreateInitialEmpires(countries, ((IcaConfig)(input.Config))).ToList();

            double iteration = 1;

            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(input.Config.OutputFile))
            {
                file.Flush();
                while (empires.Count > 1 && iteration < input.Config.MaxIteration)
                {
                    foreach (Empire<CompositionPlan> empire in empires)
                    {
                        empire.Assimilate(input.Config.QualityAttributeWeights, input)
                            .UpdateAfterAssimilation()
                            .CalculateCost(((IcaConfig)(input.Config)).Zeta);
                    }

                    empires.NormalizePowers().Compete();

                    countries.ForEach(c=>c.Revolution(countries, ((IcaConfig)(input.Config)).RevolutionRate));
                    
                    empires.EliminatePowerlessEmpires();

                    string output =
                        $"iteration {iteration}," +
                        $" empires: {empires.Count}," +
                       // $" best solution:{empires.First().Imperialist}," +
                        $" best cost:{empires.First().Imperialist.Cost}";

                    file.WriteLine($"{iteration},{empires.First().Imperialist.Cost}");

                    display(output);
                    iteration++;
                }
            }

            display($"ICA Best Cost: {empires.First().Imperialist.Cost}");

            return empires.First().Imperialist;
        }

        private IEnumerable<Empire<CompositionPlan>> CreateInitialEmpires(List<CompositionPlan> countries, IcaConfig config)
        {
            foreach (CompositionPlan country in countries)
            {
                country.Cost = country.CalculateCost(config.QualityAttributeWeights);
                country.Power = 1 - country.Cost;
            }

            countries = countries.OrderBy(x => x.Cost).ToList();

            IEnumerable<CompositionPlan> initialImperialists = countries
                .Take(config.InitialEmpiresCount).ToList();

            double sum = 0;
            foreach (CompositionPlan imperialist in initialImperialists)
            {
                sum += imperialist.Power;
            }

            List<Empire<CompositionPlan>> empires = new List<Empire<CompositionPlan>>
                (initialImperialists.Count());

            int skipCount = config.InitialEmpiresCount;

            foreach (CompositionPlan imperialist in initialImperialists)
            {
                Empire<CompositionPlan> empire = new Empire<CompositionPlan>();

                imperialist.NormalizedPower = imperialist.Power / sum;

                int coloniesCount = (int)(imperialist.NormalizedPower *
                                          (config.CandidatesPerTask - config.InitialEmpiresCount));

                empire.Imperialist = imperialist;
                empire.Colonies = countries.Skip(skipCount).Take(coloniesCount).ToList();

                empires.Add(empire);

                skipCount += coloniesCount;
            }

            List<CompositionPlan> remainedCountries = countries.Skip(skipCount)
                .Take(countries.Count - skipCount).ToList();

            empires.First().Colonies.AddRange(remainedCountries);
            return empires;
        }

    }
}
