using System;
using System.Collections.Generic;
using System.Linq;
using Leopard.Algorithms.Dpso.Extensions;
using Leopard.Domain;
using Leopard.Domain.Extensions;
using Leopard.Domain.Models;

namespace Leopard.Algorithms.Dpso
{
    public class Dpso:IAlgorithm
    {
        public CompositionPlan Execute(CompositionRequest input, Action<string> display)
        {
            display("DPSO started...\n");

            List<CompositionPlan> particles = input.CreateInitialPopulation().ToList();

            particles.ForEach(p => p.PBest = p);

            particles.ForEach(p => p.Cost = p.CalculateCost(input.Config.QualityAttributeWeights));

            CompositionPlan gBest = particles.GetGlobalBest();

            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(input.Config.OutputFile))
            {
                file.Flush();

                DpsoConfig psoConfig = (DpsoConfig)input.Config;
                double c1_primary = psoConfig.C1;
                double c2_primary = psoConfig.C2;

                for (int i = 0; i < psoConfig.MaxIteration; i++)
                {
                    psoConfig.C1 = ((psoConfig.MaxIteration - i) * c1_primary + (i * c2_primary)) / psoConfig.MaxIteration;

                    psoConfig.C2 = ((psoConfig.MaxIteration - i) * c2_primary + (i * c1_primary)) / psoConfig.MaxIteration;

                    particles.ForEach(p =>
                    {
                        p.Move(gBest, psoConfig, input.TaskCandidateServices.ToList());
                        gBest = particles.GetGlobalBest();
                    });

                    display($"iteration: {i}, Cost: {gBest.Cost}");
                    file.WriteLine($"{i},{gBest.Cost}");
                }
            }

            display($"DPSO Best Solution: {gBest}");

            return gBest;
        }
    }
}
