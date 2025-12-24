using DotNetOptions.Demo.Api.Interfaces;
using DotNetOptions.Demo.Api.Options;
using Microsoft.Extensions.Options;

namespace DotNetOptions.Demo.Api.Services
{
    public class OptionsSnapshotService : IOptionsSnapshotService
    {
        private readonly ApplicationOptions _options;

        public OptionsSnapshotService(IOptionsSnapshot<ApplicationOptions> options) =>
            _options = options.Value;

        public ApplicationOptions GetOptions() => _options;

        public string GetDescription()
        {
            return "IOptionsSnapshot: Scoped - valores são recalculados uma vez por request, detecta mudanças no appsettings.";
        }
    }
}
