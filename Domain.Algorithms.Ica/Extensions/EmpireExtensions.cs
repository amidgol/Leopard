using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Extensions;
using Domain.ICA;
using Domain.Models;
namespace Domain.Algorithms.Ica.Extensions
{
    internal static class EmpireExtensions
    {

        public static List<Empire<CompositionPlan>> EliminatePowerlessEmpires(
            this List<Empire<CompositionPlan>> empires)
        {
            foreach (Empire<CompositionPlan> empire in empires)
            {
                if (empire.Colonies.Count == 0)
                {
                    empires.JoinRandomly(empire.Imperialist);
                    empires.Remove(empire);
                }
            }

            return empires;
        }

        public static List<Empire<CompositionPlan>> NormalizePowers(this List<Empire<CompositionPlan>> empires)
        {
            double maxCost = empires.OrderBy(e => e.TotalCost).Last().TotalCost;

            foreach (Empire<CompositionPlan> empire in empires)
            {
                empire.NormalizedPower = maxCost - empire.TotalCost;
            }

            return empires;
        }

        public static List<Empire<CompositionPlan>> Compete(this List<Empire<CompositionPlan>> empires)
        {
            empires = empires.OrderBy(e => e.TotalCost).ToList();

            CompositionPlan weakestColony = empires.Last().Colonies.OrderBy(x => x.Cost).Last();

            empires.Last().Colonies.Remove(weakestColony);

            empires.JoinRandomly(weakestColony);

            return empires;
        }

        private static List<Empire<CompositionPlan>> JoinRandomly(this List<Empire<CompositionPlan>> empires,
            CompositionPlan country)
        {
            double sumNormalizedPowers = empires.Select(e => e.NormalizedPower).Sum();

            double[] probabilities = new double[empires.Count - 1];
            Random random = new Random();
            for (int i = 0; i < probabilities.Length; i++)
            {
                double r = (double)random.Next(0, 100) / 100;
                probabilities[i] = (empires[i + 1].NormalizedPower / sumNormalizedPowers) - r;
            }

            double maxProbability = probabilities.Max();
            int maxIndex = probabilities.ToList().IndexOf(maxProbability);

            empires[maxIndex + 1].Colonies.Add(country);

            return empires;
        }

        public static Empire<CompositionPlan> Assimilate(this Empire<CompositionPlan> empire,
            List<QualityAttributeWeight> attributeWeights)
        {
            foreach (CompositionPlan colony in empire.Colonies)
            {
                List<double> mask = colony.GetMask();

                for (var i = 0; i < colony.TaskServices.Count; i++)
                {
                    TaskService taskService = colony.TaskServices[i];

                    if (mask[i] > taskService.WebService.Cost)
                    {
                        colony.TaskServices[i].WebService = empire.Imperialist.TaskServices[i]
                            .WebService;

                        //update colony's cost
                        colony.Cost = colony.CalculateCost(attributeWeights);
                    }
                }
            }

            return empire;
        }

        public static Empire<CompositionPlan> UpdateAfterAssimilation(this Empire<CompositionPlan> empire)
        {
            CompositionPlan bestColony = empire.Colonies.OrderBy(x => x.Cost).First();

            if (bestColony.Cost < empire.Imperialist.Cost)
            {
                CompositionPlan formerImperialist = empire.Imperialist;
                empire.Imperialist = bestColony;
                empire.Colonies[0] = formerImperialist;
            }

            return empire;
        }

        public static Empire<CompositionPlan> CalculateCost(this Empire<CompositionPlan> empire, double zeta)
        {
            double totalCost = empire.Imperialist.Cost + zeta * (empire.Colonies.Select(x => x.Cost).Average());

            empire.TotalCost = totalCost;

            return empire;
        }
    }
}
