using System;
using System.Collections.Generic;
using System.Text;
using Domain.Models;

namespace Domain
{
    public interface IFitnessCalculator<in T> where T:class 
    {
        double FitnessFunction(T input);
    }
}
