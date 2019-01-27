using System;
using System.Collections.Generic;
using Domain.Algorithms.Ica;
using Domain.Enums;
using Domain.Models;

namespace Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            QualityAttribute availability = new QualityAttribute
            {
                Title = "Availability",
                MinPossibleValue = 0,
                MaxPossibleValue = 1,
                Type = QualityAttributeType.BenefitOriented,
                Unit = "Probability",
                Scale = typeof(double)
            };
            QualityAttribute reliability = new QualityAttribute
            {
                Title = "Reliability",
                MinPossibleValue = 0,
                MaxPossibleValue = 1,
                Type = QualityAttributeType.BenefitOriented,
                Unit = "Probability",
                Scale = typeof(double)
            };
            QualityAttribute rank = new QualityAttribute
            {
                Title = "Rank",
                MinPossibleValue = 1,
                MaxPossibleValue = 5,
                Type = QualityAttributeType.BenefitOriented,
                Unit = "int",
                Scale = typeof(int)
            };
            IcaConfig icaConfig = new IcaConfig
            {
                QualityAttributeWeights = new List<QualityAttributeWeight>
                {
                    new QualityAttributeWeight
                    {
                        QualityAttribute = availability,
                        Weight = 0.4
                    },
                    new QualityAttributeWeight
                    {
                        QualityAttribute = reliability,
                        Weight = 0.6
                    }
                }
            };

            Ica ica = new Ica(icaConfig);

            ica.Execute(GenerateRequest(icaConfig, availability, reliability, rank), icaConfig);

            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }

        private static CompositionRequest GenerateRequest(IcaConfig config, params QualityAttribute[] attributes)
        {
            List<TaskCandidateServices> candidateServices = new List<TaskCandidateServices>();

            Random random = new Random();

            TaskCandidateServices weatherForecast = new TaskCandidateServices
            {
                Task = new SingleTask { Title = "Weather Forecast" },
                WebServices = new List<WebService>()
            };

            for (int i = 0; i < config.CandidatesPerTask; i++)
            {
                double a = (double)random.Next(5000, 10000) / 10000;
                double b = (double)random.Next(5500, 10000) / 10000;
                double c = random.Next(1, 5);

                weatherForecast.WebServices.Add(new WebService
                {
                    Title = $"weather-{i}",
                    QualityAttributeValues = new List<QualityAttributeValue>
                    {
                        new QualityAttributeValue
                        {
                            QualityAttribute = attributes[0],
                            Value = a
                        },
                        new QualityAttributeValue
                        {
                            QualityAttribute = attributes[1],
                            Value = b
                        },
                        new QualityAttributeValue
                        {
                            QualityAttribute = attributes[2],
                            Value = c
                        }
                    }
                });
            }

            TaskCandidateServices stock = new TaskCandidateServices
            {
                Task = new SingleTask { Title = "Stock Market" },
                WebServices = new List<WebService>()
            };

            for (int i = 0; i < config.CandidatesPerTask; i++)
            {
                double a = (double)random.Next(50, 100) / 100;
                double b = (double)random.Next(55, 100) / 100;
                double c = random.Next(1, 5);

                stock.WebServices.Add(new WebService
                {
                    Title = $"stock-{i}",
                    QualityAttributeValues = new List<QualityAttributeValue>
                    {
                        new QualityAttributeValue
                        {
                            QualityAttribute = attributes[0],
                            Value = a
                        },
                        new QualityAttributeValue
                        {
                            QualityAttribute = attributes[1],
                            Value = b
                        },
                        new QualityAttributeValue
                        {
                            QualityAttribute = attributes[2],
                            Value = c
                        }
                    }
                });
            }


            CompositionRequest request = new CompositionRequest
            {
                TaskCandidateServices = new List<TaskCandidateServices>
               {
                   stock,weatherForecast
               }
            };

            return request;
        }
    }
}
