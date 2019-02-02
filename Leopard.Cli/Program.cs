using System;
using System.Collections.Generic;
using System.IO;
using Leopard.Algorithms.Dpso;
using Leopard.Algorithms.Ica;
using Leopard.Algorithms.Pso;
using Leopard.Domain;
using Leopard.Domain.Enums;
using Leopard.Domain.Models;
using Leopard.RequestGeneration.RequestGenerators;
using Microsoft.Extensions.Configuration;

namespace Leopard.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Action> actions = new List<Action>();
            void DisplayAction(string s) => Console.WriteLine(s);

            IcaConfig icaConfig = GetIcaConfig();
            PsoConfig psoConfig = GetPsoConfig();
            DpsoConfig dpsoConfig = GetDpsoConfig();

            IRequestGenerator requestGenerator = new QwsRequestGenerator();

            Ica ica = new Ica();
            CompositionRequest icaRequest = requestGenerator.Generate(icaConfig);
            actions.Add(() => ica.Execute(icaRequest, DisplayAction));

            Pso pso = new Pso();
            CompositionRequest psoRequest = requestGenerator.Generate(psoConfig);
            actions.Add(() => pso.Execute(psoRequest, DisplayAction));

            Dpso dpso = new Dpso();
            CompositionRequest dpsoRequest = requestGenerator.Generate(dpsoConfig);
            actions.Add(() => dpso.Execute(dpsoRequest, DisplayAction));

            for (var i = 0; i < actions.Count; i++)
            {
                Console.ForegroundColor = i % 2 == 0 ? ConsoleColor.Cyan : ConsoleColor.DarkYellow;

                var a = actions[i];
                a.Invoke();
            }

            Console.ReadLine();
        }

        private static IcaConfig GetIcaConfig()
        {
            IcaConfig icaConfig = new IcaConfig
            {
                InitialEmpiresCount = Convert.ToInt32(AppSettings("ICA")
                    .GetSection("InitialEmpiresCount").Value),
                Zeta = Convert.ToDouble(AppSettings("ICA").GetSection("Zeta").Value),
                RevolutionRate = Convert.ToDouble(AppSettings("ICA")
                    .GetSection("RevolutionRate").Value),
                OutputFile = AppSettings("ICA").GetSection("OutputFile").Value,

                Tasks = GetTasks(),
                DataSetFilePath = AppSettings("DataSetFilePath").Value,
                QualityAttributeWeights = GetQualityAttributeWeights(),
                CandidatesPerTask = Convert.ToInt32(AppSettings("CandidatesPerTask").Value),
                FileOffset = Convert.ToInt32(AppSettings("FileOffset").Value),
                MaxIteration = Convert.ToInt32(AppSettings("MaxIteration").Value)
            };

            return icaConfig;
        }

        private static PsoConfig GetPsoConfig()
        {
            PsoConfig psoConfig = new PsoConfig()
            {
                C1 = Convert.ToDouble(AppSettings("PSO").GetSection("C1").Value),
                C2 = Convert.ToDouble(AppSettings("PSO").GetSection("C2").Value),
                Omega = Convert.ToDouble(AppSettings("PSO").GetSection("Omega").Value),
                OutputFile = AppSettings("PSO").GetSection("OutputFile").Value,

                Tasks = GetTasks(),
                DataSetFilePath = AppSettings("DataSetFilePath").Value,
                QualityAttributeWeights = GetQualityAttributeWeights(),
                CandidatesPerTask = Convert.ToInt32(AppSettings("CandidatesPerTask").Value),
                FileOffset = Convert.ToInt32(AppSettings("FileOffset").Value),
                MaxIteration = Convert.ToInt32(AppSettings("MaxIteration").Value)
            };

            return psoConfig;
        }

        private static DpsoConfig GetDpsoConfig()
        {
            DpsoConfig dpsoConfig = new DpsoConfig()
            {
                C1 = Convert.ToDouble(AppSettings("DPSO").GetSection("C1").Value),
                C2 = Convert.ToDouble(AppSettings("DPSO").GetSection("C2").Value),
                Omega = Convert.ToDouble(AppSettings("DPSO").GetSection("Omega").Value),
                OutputFile = AppSettings("DPSO").GetSection("OutputFile").Value,

                Tasks = GetTasks(),
                DataSetFilePath = AppSettings("DataSetFilePath").Value,
                QualityAttributeWeights = GetQualityAttributeWeights(),
                CandidatesPerTask = Convert.ToInt32(AppSettings("CandidatesPerTask").Value),
                FileOffset = Convert.ToInt32(AppSettings("FileOffset").Value),
                MaxIteration = Convert.ToInt32(AppSettings("MaxIteration").Value)
            };

            return dpsoConfig;
        }

        private static List<SingleTask> GetTasks()
        {
            List<SingleTask> tasks = new List<SingleTask>();

            for (int i = 0;
                i < Convert.ToInt32(AppSettings("NumberOfTasks").Value);
                i++)
            {
                tasks.Add(new SingleTask { Title = $"Task-{i}" });
            }

            return tasks;
        }

        private static List<QualityAttributeWeight> GetQualityAttributeWeights()
        {
            List<QualityAttributeWeight> attributeWeights = new List<QualityAttributeWeight>();

            foreach (IConfigurationSection section
                in AppSettings("QualityAttributeWeights")
                    .GetSection("CostOriented").GetChildren())
            {
                attributeWeights.Add(new QualityAttributeWeight
                {
                    QualityAttribute = new QualityAttribute
                    {
                        Title = section.Key,
                        Type = QualityAttributeType.CostOriented
                    },
                    Weight = Convert.ToDouble(section.Value)
                });
            }

            foreach (IConfigurationSection section
                in AppSettings("QualityAttributeWeights")
                    .GetSection("BenefitOriented").GetChildren())
            {
                attributeWeights.Add(new QualityAttributeWeight
                {
                    QualityAttribute = new QualityAttribute
                    {
                        Title = section.Key,
                        Type = QualityAttributeType.BenefitOriented
                    },
                    Weight = Convert.ToDouble(section.Value)
                });
            }

            return attributeWeights;
        }
        
        private static IConfigurationSection AppSettings(string sectionName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build().GetSection(sectionName);
        }
    }
}
