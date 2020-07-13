using System;
using System.Collections.Generic;
using Leopard.Domain.Extensions;
using Leopard.Domain.Models;

namespace Leopard.Algorithms.Ica.Extensions
{
    public static class CountryExtensions
    {
        public static CompositionPlan Revolution(this CompositionPlan country,
            List<CompositionPlan> countries, double revolutionRate)
        {
            Random random = new Random();
            double randomNumber = (double)random.Next(0, 100) / 100;

            if (randomNumber > revolutionRate)
            {
                int randomTaskIndex = random.Next(0, country.TaskServices.Count - 1);
                int randomCountryIndex = random.Next(0, countries.Count);

                if (!countries[randomCountryIndex].Equals(country))
                {
                    country.TaskServices[randomTaskIndex].WebService =
                        countries[randomCountryIndex].TaskServices[randomTaskIndex].WebService;
                }
            }

            return country;
        }
    }
}
