using DotNetOptions.Demo.Api.Interfaces;
using DotNetOptions.Demo.Api.Options;
using Microsoft.Extensions.Options;

namespace DotNetOptions.Demo.Api.Services
{
    public class OptionsMonitorService : IOptionsMonitorService
    {
        private readonly ApplicationOptions _options;

        public OptionsMonitorService(IOptionsMonitor<ApplicationOptions> options) =>
            _options = options.CurrentValue;

        public ApplicationOptions GetOptions() => _options;

        public string GetDescription()
        {
            return "IOptionsMonitor: Singleton - Detecta mudanças em tempo real, CurrentValue sempre retorna o valor mais recente.";
        }
    }
}
