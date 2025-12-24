using DotNetOptions.Demo.Api.Options;
using Microsoft.Extensions.Options;

namespace DotNetOptions.Demo.Api.BackgroundServices
{
    public class OptionsMonitorBackgroundService : BackgroundService
    {
        private readonly IOptionsMonitor<ApplicationOptions> _optionsMonitor;
        private readonly ILogger<OptionsMonitorBackgroundService> _logger;
        private IDisposable? _changeListener;
        private ApplicationOptions? previousOptions = null;

        public OptionsMonitorBackgroundService(
            IOptionsMonitor<ApplicationOptions> optionsMonitor,
            ILogger<OptionsMonitorBackgroundService> logger)
        {
            _optionsMonitor = optionsMonitor;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("OptionsMonitorBackgroundService iniciado");

            // Log inicial
            LogCurrentOptions("Valores iniciais");

            // Registrar listener para mudanças
            _changeListener = _optionsMonitor.OnChange((options, name) =>
            {
                if (previousOptions is null || !options.Equals(previousOptions))
                {
                    previousOptions = options;

                    _logger.LogWarning("""
                    ========================================");
                    MUDANÇA DETECTADA nas configurações!
                    ========================================
                    """);
                    LogCurrentOptions($"Novos valores");
                }
            });

            return Task.CompletedTask;
        }

        private void LogCurrentOptions(string context)
        {
            var options = _optionsMonitor.CurrentValue;
            _logger.LogInformation($"""
            --- {context} ---
            CompanyName: {options.CompanyName}
            City: {options.City}
            Timestamp: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}
            """);
        }

        public override void Dispose()
        {
            _changeListener?.Dispose();
            base.Dispose();
        }
    }
}
