# DotNetOptions.Demo

Uma aplicação ASP.NET Core Minimal API demonstrando o **Options Pattern** com exemplos completos de todos os tipos de opções disponíveis no .NET.

## 📋 Visão Geral

Esta aplicação demonstra o uso de:
- **IOptions<T>**: Singleton - valores são lidos uma vez e nunca mudam durante a vida da aplicação
- **IOptionsSnapshot<T>**: Scoped - valores são recalculados uma vez por request, detecta mudanças no appsettings
- **IOptionsMonitor<T>**: Singleton - detecta mudanças em tempo real, CurrentValue sempre retorna o valor mais recente

## 📡 Endpoints Disponíveis

### Endpoint Raiz
- `GET /` - Informações sobre a API e lista de endpoints

### Endpoints de Options
- `GET /options/options` - Demonstra IOptions
- `GET /options/options-snapshot` - Demonstra IOptionsSnapshot
- `GET /options/options-monitor` - Demonstra IOptionsMonitor

### Endpoint de Comparação
- `GET /options/compare` - Compara todos os tipos de Options Pattern em uma única resposta

## 🧪 Testando o Reload de Configurações

1. Execute a aplicação com `dotnet run`
2. Faça uma requisição para `http://localhost:5000/options/compare`
3. Edite o arquivo `appsettings.json` e mude os valores em `Application`
4. Salve o arquivo
5. Observe os logs - o BackgroundService detectará a mudança automaticamente
6. Faça outra requisição para `http://localhost:5000/options/compare`
7. Compare os resultados:
   - **IOptions**: Mantém os valores antigos (não recarrega)
   - **IOptionsSnapshot**: Mostra os novos valores (recarrega por request)
   - **IOptionsMonitor**: Mostra os novos valores (recarrega em tempo real)
  
## 🚀 Adicional

Há um exemplo adicional usando Azure App Configuration como provider de configurações, que pode sobrescrever as configurações definidas no `appsettings.json`

## 📝 ApplicationOptions

A classe `ApplicationOptions` contém duas propriedades:

```csharp
public class ApplicationOptions
{
    public string CompanyName { get; set; }
    public string City { get; set; }
}
```

## 📚 BackgroundService de Monitoramento

A classe `OptionsMonitorBackgroundService` monitora mudanças no `appsettings.json` e loga:
- Valores iniciais ao iniciar a aplicação
- Detecção de mudanças em tempo real
- Novos valores após cada mudança

## 🛠️ Tecnologias

- .NET 10
- ASP.NET Core Minimal API
- Options Pattern
- BackgroundService
- File Watcher (reloadOnChange)
- Azure App Configuration

## 📖 Quando Usar Cada Tipo

### IOptions<T>
✅ Use quando:
- As configurações não precisam mudar durante a execução
- Você quer melhor performance (não verifica mudanças)
- As configurações são lidas uma única vez

❌ Não use quando:
- Precisa detectar mudanças em tempo real
- As configurações podem ser atualizadas durante a execução

### IOptionsSnapshot<T>
✅ Use quando:
- Precisa de valores atualizados por request
- Trabalha com dependency injection scoped
- Quer evitar leituras inconsistentes durante um request

❌ Não use quando:
- Trabalha com singleton services
- Precisa de notificações de mudanças

### IOptionsMonitor<T>
✅ Use quando:
- Precisa de valores sempre atualizados
- Quer receber notificações de mudanças
- Trabalha com singleton services
- Precisa reagir a mudanças de configuração

❌ Não use quando:
- Não precisa de detecção de mudanças
- Quer evitar overhead de monitoramento
