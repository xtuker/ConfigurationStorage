namespace TestWorker;

using Xtuker.ConfigurationStorage;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private readonly IConfigurationStorage _repository;

    public Worker(ILogger<Worker> logger, IConfiguration configuration, IConfigurationStorage repository)
    {
        _logger = logger;
        _configuration = configuration;
        _repository = repository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // _repository.SetData(new InMemoryDbConfigurationRepository.DbConfiguration()
        // {
        //     Key = "Test:CryptoValue",
        //     Value = "This is crypto value",
        //     IsEncrypted = true
        // });

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Test:Value: {0}", _configuration["Test:Value"]);
            _logger.LogInformation("Test:CryptoValue: {0}", _configuration["Test:CryptoValue"]);

            _logger.LogWarning("Key: {0}", _configuration[ConfigurationStorageSource.CryptoTransformerKeyPath]);

            _configuration["Test:Value"] = $"test-{DateTime.Now:u}";

            await Task.Delay(2000, stoppingToken);
        }
    }
}