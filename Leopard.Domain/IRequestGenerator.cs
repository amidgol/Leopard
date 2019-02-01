using Leopard.Domain.Models;

namespace Leopard.Domain
{
    public interface IRequestGenerator
    {
        CompositionRequest Generate(Config config);
    }
}
