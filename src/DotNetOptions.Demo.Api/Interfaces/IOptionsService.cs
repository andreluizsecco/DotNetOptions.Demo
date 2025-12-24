using DotNetOptions.Demo.Api.Options;

namespace DotNetOptions.Demo.Api.Interfaces
{
    public interface IOptionsService
    {
        ApplicationOptions GetOptions();
        string GetDescription();
    }
}
