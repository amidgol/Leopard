using System;
using System.Collections.Generic;
using System.IO;
using Domain;
using Domain.Algorithms.Ica;
using Domain.Enums;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Service.RequestGenerators;

namespace Cli
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
