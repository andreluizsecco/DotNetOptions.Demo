using DotNetOptions.Demo.Api.Options;

namespace DotNetOptions.Demo.Api.Interfaces
{
    public interface IOptionsSnapshotService
    {
        ApplicationOptions GetOptions();
        string GetDescription();
    }
}
