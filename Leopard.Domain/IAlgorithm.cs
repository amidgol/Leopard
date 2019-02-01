using System;
using Leopard.Domain.Models;

namespace Leopard.Domain
{
    public interface IAlgorithm
    {
        CompositionPlan Execute(CompositionRequest request, Action<string> display);
    }
}
