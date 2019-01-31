using System;
using System.Collections.Generic;
using System.IO;
using Domain;
using Domain.Algorithms.Ica;
using Domain.Enums;
using Domain.Models;
using Service.RequestGenerators;

namespace Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            List<SingleTask> tasks = new List<SingleTask>
            {
                new SingleTask{Title = "A"},
                new SingleTask{Title = "B"},
                new SingleTask{Title = "C"},
                new SingleTask{Title = "D"},
                new SingleTask{Title = "E"},
                new SingleTask{Title = "F"},
                new SingleTask{Title = "G"},
                new SingleTask{Title = "H"}
            };
            
            QualityAttribute availability = new QualityAttribute
            {
                Title = "Availability",
                Type = QualityAttributeType.BenefitOriented
            };
            QualityAttribute reliability = new QualityAttribute
            {
                Title = "Reliability",
                Type = QualityAttributeType.BenefitOriented
            };
            QualityAttribute throughput = new QualityAttribute
            {
                Title = "Throughput",
                Type = QualityAttributeType.BenefitOriented

            };
            QualityAttribute responseTime = new QualityAttribute
            {
                Title = "ResponseTime",
                Type = QualityAttributeType.CostOriented
            };

            IcaConfig icaConfig = new IcaConfig
            {
                Tasks = tasks,
                DataSetFilePath = @"C:\Users\Amid\Desktop\QWS_Dataset_V2.txt",
                QualityAttributeWeights = new List<QualityAttributeWeight>
                {
                    new QualityAttributeWeight
                    {
                        QualityAttribute = availability,
                        Weight = 0.3
                    },
                    new QualityAttributeWeight
                    {
                        QualityAttribute = reliability,
                        Weight = 0.3
                    },
                    new QualityAttributeWeight
                    {
                        QualityAttribute = throughput,
                        Weight = 0.2
                    },
                    new QualityAttributeWeight
                    {
                        QualityAttribute = responseTime,
                        Weight = 0.2
                    }
                }
            };

            Ica ica = new Ica(icaConfig);

            IRequestGenerator requestGenerator = new QwsRequestGenerator();
            CompositionRequest request = requestGenerator.Generate(icaConfig);

            ica.Execute(request, icaConfig);

            Console.ReadLine();
        }

        private static CompositionRequest GenerateRequest(IcaConfig config, params QualityAttribute[] attributes)
        {
            List<TaskCandidateService> candidateServices = new List<TaskCandidateService>();

            Random random = new Random();

            TaskCandidateService weatherForecast = new TaskCandidateService
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

            TaskCandidateService stock = new TaskCandidateService
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
               TaskCandidateServices  = new List<TaskCandidateService>
               {
                   stock,weatherForecast
               }
            };

            return request;
        }
    }
}
