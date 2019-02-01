using System;
using WebServiceComposition.Domain.Models;

namespace WebServiceComposition.Domain
{
    public interface IAlgorithm
    {
        CompositionPlan Execute(CompositionRequest request, Action<string> display);
    }
}
