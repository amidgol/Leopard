using System;
using System.Collections.Generic;
using System.Linq;
using Leopard.Algorithms.Ica.Models;
using Leopard.Domain.Extensions;
using Leopard.Domain.Models;

namespace Leopard.Algorithms.Ica.Extensions
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
                }
            }

            empires.RemoveAll(x => x.Colonies.Count == 0);

            return empires;
        }

        public static List<Empire<CompositionPlan>> NormalizePowers(this List<Empire<CompositionPlan>> empires)
        {
            double maxCost = empires.OrderBy(e => e.TotalCost).Last().TotalCost;

            foreach (Empire<CompositionPlan> empire in empires)
            {
                empire.NormalizedPower = 1 - empire.TotalCost;
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
            double sumNormalizedPowers = empires.Where(e => e.Colonies.Any()).Select(e => e.NormalizedPower).Sum();

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
            List<QualityAttributeWeight> attributeWeights, CompositionRequest request)
        {
            foreach (CompositionPlan colony in empire.Colonies)
            {
                List<double> mask = colony.GetMask();

                for (int i = 0; i < colony.TaskServices.Count; i++)
                {
                    TaskService taskService = colony.TaskServices[i];

                    if (mask[i] < taskService.WebService.Cost &&
                        colony.TaskServices[i].WebService.Cost > empire.Imperialist.TaskServices[i].WebService.Cost)
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

        public static Empire<CompositionPlan> CustomAssimilate(this Empire<CompositionPlan> empire, CompositionPlan gBest,
            List<QualityAttributeWeight> attributeWeights, CompositionRequest request)
        {
            IcaConfig icaConfig = (IcaConfig) request.Config;

            foreach (CompositionPlan colony in empire.Colonies)
            {
                double revolutionRate = (double)1 / icaConfig.Tasks.Count;

                Random r = new Random();

                foreach (TaskService taskService in colony.TaskServices)
                {
                    double rand1 = (double)r.Next(0, 100) / 100;

                    if (rand1 < revolutionRate)
                    {
                        List<WebService> webServices = request.TaskCandidateServices
                            .First(t => t.Task.Equals(taskService.Task)).WebServices;

                        int randomIndex = r.Next(0, webServices.Count - 1);

                        taskService.WebService = webServices[randomIndex];
                    }
                    else
                    if (rand1 < icaConfig.Alpha)
                    {
                        if (taskService.WebService.Cost > empire.Imperialist.TaskServices
                                .First(t => t.Task.Equals(taskService.Task)).WebService.Cost)
                        {
                            taskService.WebService = empire.Imperialist.TaskServices
                                .First(t => t.Task.Equals(taskService.Task)).WebService;
                        }
                    }
                    else if (rand1 < icaConfig.Alpha + icaConfig.Beta)
                    {
                        if (taskService.WebService.Cost > colony.PBest.TaskServices
                                .First(t => t.Task.Equals(taskService.Task)).WebService.Cost)
                        {
                            taskService.WebService = colony.PBest.TaskServices
                                .First(t => t.Task.Equals(taskService.Task)).WebService;
                        }
                    }
                    else
                    {
                        if (taskService.WebService.Cost > gBest.TaskServices
                                .First(t => t.Task.Equals(taskService.Task)).WebService.Cost)
                        {
                            taskService.WebService = gBest.TaskServices
                                .First(t => t.Task.Equals(taskService.Task)).WebService;
                        }
                    }

                    colony.UpdatePBest(icaConfig);
                }
            }

            return empire;
        }

        public static Empire<CompositionPlan> UpdateAfterAssimilation(this Empire<CompositionPlan> empire)
        {

            try
            {
                empire.Colonies.OrderBy(x => x.Cost);
                CompositionPlan bestColony = empire.Colonies.OrderBy(x => x.Cost).First();

                if (bestColony.Cost < empire.Imperialist.Cost)
                {
                    CompositionPlan formerImperialist = empire.Imperialist;
                    empire.Imperialist = bestColony;
                    empire.Colonies.Remove(bestColony);
                    empire.Colonies.Add(formerImperialist);
                }
            }
            catch (Exception)
            {

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
