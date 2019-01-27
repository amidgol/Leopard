using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            double iteration = 1;

            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\Users\Amid\Desktop\out.txt"))
            {
                file.Flush();
                while (empires.Count > 1 && iteration < 10000)
                {
                    foreach (Empire<CompositionPlan> empire in empires)
                    {
                        empire.Assimilate(config.QualityAttributeWeights)
                            .UpdateAfterAssimilation()
                            .CalculateCost(_icaConfig.Zeta);
                    }

                    empires.NormalizePowers().Compete();

                    empires.EliminatePowerlessEmpires();

                    string output =
                        $"iteration {iteration}," +
                        $" empires: {empires.Count}," +
                        $" best solution:{empires.First().Imperialist}," +
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
                sum += imperialist.Power;
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

                skipCount += coloniesCount;
            }

            List<CompositionPlan> remainedCountries = countries.Skip(skipCount)
                .Take(countries.Count - skipCount).ToList();

            empires.First().Colonies.AddRange(remainedCountries);
            return empires;
        }

    }
}
