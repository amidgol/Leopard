using System;
using System.Collections.Generic;
using System.Linq;
using WebServiceComposition.Algorithms.Ica.Extensions;
using WebServiceComposition.Algorithms.Ica.Models;
using WebServiceComposition.Domain;
using WebServiceComposition.Domain.Extensions;
using WebServiceComposition.Domain.Models;

namespace WebServiceComposition.Algorithms.Ica
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
            List<CompositionPlan> countries = input.CreateInitialPopulation().ToList();

            List<Empire<CompositionPlan>> empires = CreateInitialEmpires(countries).ToList();

            double iteration = 1;

            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\Users\Amid\Desktop\ica.txt"))
            {
                file.Flush();
                while (empires.Count > 1 && iteration < 1000)
                {
                    foreach (Empire<CompositionPlan> empire in empires)
                    {
                        empire.Assimilate(config.QualityAttributeWeights, input)
                            .UpdateAfterAssimilation()
                            .CalculateCost(_icaConfig.Zeta);
                    }

                    empires.NormalizePowers().Compete();

                    countries.ForEach(c=>c.Revolution(countries, _icaConfig.RevolutionRate));
                    
                    empires.EliminatePowerlessEmpires();

                    string output =
                        $"iteration {iteration}," +
                        $" empires: {empires.Count}," +
                       // $" best solution:{empires.First().Imperialist}," +
                        $" best cost:{empires.First().Imperialist.Cost}";

                    file.WriteLine($"{iteration},{empires.First().Imperialist.Cost}");

                    Console.WriteLine(output);
                    iteration++;
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Cost: {empires.First().Imperialist.Cost}");

            return empires.First().Imperialist;
        }

        private IEnumerable<Empire<CompositionPlan>> CreateInitialEmpires(List<CompositionPlan> countries)
        {
            foreach (CompositionPlan country in countries)
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
                sum += imperialist.Power;
            }

            List<Empire<CompositionPlan>> empires = new List<Empire<CompositionPlan>>
                (initialImperialists.Count());

            int skipCount = _icaConfig.InitialEmpiresCount;

            foreach (CompositionPlan imperialist in initialImperialists)
            {
                Empire<CompositionPlan> empire = new Empire<CompositionPlan>();

                imperialist.NormalizedPower = imperialist.Power / sum;

                int coloniesCount = (int)(imperialist.NormalizedPower *
                                          (_icaConfig.CandidatesPerTask - _icaConfig.InitialEmpiresCount));

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
