using DotNetOptions.Demo.Api.Interfaces;
using DotNetOptions.Demo.Api.Options;
using Microsoft.Extensions.Options;

namespace DotNetOptions.Demo.Api.Services
{
    public class OptionsService : IOptionsService
    {
        private readonly ApplicationOptions _appOptions;

        public OptionsService(IOptions<ApplicationOptions> appOptions) =>
            _appOptions = appOptions.Value;

        public ApplicationOptions GetOptions() => _appOptions;

        public string GetDescription()
        {
            return "IOptions: Singleton - valores são lidos uma vez e nunca mudam durante a vida da aplicação.";
        }
    }
}
