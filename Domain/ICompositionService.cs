using System;
using System.Collections.Generic;
using System.Text;
using Domain.Models;

namespace Domain
{
    public interface ICompositionService
    {
        CompositionPlan Combine(CompositionRequest request, 
            IAlgorithm<CompositionPlan, 
            CompositionRequest> algorithm);
    }
}
