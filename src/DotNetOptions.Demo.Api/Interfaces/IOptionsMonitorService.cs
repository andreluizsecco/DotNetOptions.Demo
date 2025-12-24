using DotNetOptions.Demo.Api.Options;

namespace DotNetOptions.Demo.Api.Interfaces
{
    public interface IOptionsMonitorService
    {
        ApplicationOptions GetOptions();
        string GetDescription();
    }
}
