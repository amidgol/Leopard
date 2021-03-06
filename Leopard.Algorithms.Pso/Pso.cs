﻿using System;
using System.Collections.Generic;
using System.Linq;
using Leopard.Algorithms.Pso.Extensions;
using Leopard.Domain;
using Leopard.Domain.Extensions;
using Leopard.Domain.Models;

namespace Leopard.Algorithms.Pso
{
    public class Pso : IAlgorithm
    {
        public CompositionPlan Execute(CompositionRequest input, Action<string> display)
        {
            display("PSO started...\n");

            List<CompositionPlan> particles = input.CreateInitialPopulation().ToList();

            particles.ForEach(p => p.PBest = p);

            particles.ForEach(p => p.Cost = p.CalculateCost(input.Config.QualityAttributeWeights));

            CompositionPlan gBest = particles.GetGlobalBest();

            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(input.Config.OutputFile))
            {
                file.Flush();

                PsoConfig psoConfig = (PsoConfig) input.Config;

                for (int i = 0; i < psoConfig.MaxIteration; i++)
                {
                    particles.ForEach(p =>
                    {
                        p.Move(gBest, psoConfig, input.TaskCandidateServices.ToList());
                        gBest = particles.GetGlobalBest();
                    });

                    display($"iteration: {i}, Cost: {gBest.Cost}");
                    file.WriteLine($"{i},{gBest.Cost}");
                }
            }

            display($"PSO Best Solution: {gBest}");

            return gBest;
        }
    }
}
