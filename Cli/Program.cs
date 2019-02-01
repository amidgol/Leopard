using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using WebServiceComposition.Algorithms.Ica;
using WebServiceComposition.Algorithms.Pso;
using WebServiceComposition.Domain;
using WebServiceComposition.Domain.Enums;
using WebServiceComposition.Domain.Models;
using WebServiceCompositionService.RequestGenerators;

namespace WebServiceComposition.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            IcaConfig icaConfig = GetIcaConfig();

            Ica ica = new Ica(icaConfig);

            IRequestGenerator requestGenerator = new QwsRequestGenerator();
            CompositionRequest request = requestGenerator.Generate(icaConfig);

            ica.Execute(request, icaConfig);

            //PsoConfig psoConfig = GetPsoConfig();

            //Pso pso = new Pso(psoConfig);
            
            //IRequestGenerator requestGenerator = new QwsRequestGenerator();
            //CompositionRequest request = requestGenerator.Generate(psoConfig);

            //pso.Execute(request, psoConfig);

            Console.ReadLine();
        }

        private static IcaConfig GetIcaConfig()
        {
            IcaConfig icaConfig = new IcaConfig
            {
                Tasks = GetTasks(),
                DataSetFilePath = AppSettings("DataSetFilePath").Value,
                QualityAttributeWeights = GetQualityAttributeWeights(),
                CandidatesPerTask = Convert.ToInt32(AppSettings("CandidatesPerTask").Value),
                FileOffset = Convert.ToInt32(AppSettings("FileOffset").Value),
                InitialEmpiresCount = Convert.ToInt32(AppSettings("ICA")
                    .GetSection("InitialEmpiresCount").Value),
                Zeta = Convert.ToDouble(AppSettings("ICA").GetSection("Zeta").Value),
                RevolutionRate = Convert.ToDouble(AppSettings("ICA")
                    .GetSection("RevolutionRate").Value)
            };

            return icaConfig;
        }

        private static PsoConfig GetPsoConfig()
        {
            PsoConfig psoConfig = new PsoConfig()
            {
                C1 = Convert.ToDouble(AppSettings("PSO").GetSection("C1").Value),
                C2 = Convert.ToDouble(AppSettings("PSO").GetSection("C2").Value),
                Tasks = GetTasks(),
                DataSetFilePath = AppSettings("DataSetFilePath").Value,
                QualityAttributeWeights = GetQualityAttributeWeights(),
                CandidatesPerTask = Convert.ToInt32(AppSettings("CandidatesPerTask").Value),
                FileOffset = Convert.ToInt32(AppSettings("FileOffset").Value)
            };

            return psoConfig;
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
