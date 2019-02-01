using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServiceComposition.Algorithms.Pso.Extensions;
using WebServiceComposition.Domain;
using WebServiceComposition.Domain.Extensions;
using WebServiceComposition.Domain.Models;

namespace WebServiceComposition.Algorithms.Pso
{
    public class Pso : IAlgorithm
    {
        public CompositionPlan Execute(CompositionRequest input)
        {
            Console.ResetColor();

            List<CompositionPlan> particles = input.CreateInitialPopulation().ToList();

            particles.ForEach(p => p.PBest = p);

            particles.ForEach(p => p.Cost = p.CalculateCost(input.Config.QualityAttributeWeights));

            CompositionPlan gBest = particles.GetGlobalBest();

            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(input.Config.OutputFile))
            {
                file.Flush();
                for (int i = 0; i < 1000; i++)
                {
                    particles.ForEach(p =>
                    {
                        p.Move(gBest, (PsoConfig)input.Config);
                        gBest = particles.GetGlobalBest();
                    });

                    Console.WriteLine($"iteration: {i}, Cost: {gBest.Cost}");
                    file.WriteLine($"{i},{gBest.Cost}");
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Best Solution: {gBest}");

            return gBest;
        }
    }
}
