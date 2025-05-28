using MesajX.ChatService.DataAccessLayer.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

public class RedisConnectionFactory : IRedisConnectionFactory, IAsyncDisposable
{
    private IConnectionMultiplexer _connectionMultiplexer;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RedisConnectionFactory> _logger;

    public RedisConnectionFactory(IConfiguration configuration, ILogger<RedisConnectionFactory> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IConnectionMultiplexer> GetConnectionAsync()
    {
        if (_connectionMultiplexer != null)
            return _connectionMultiplexer;

        var redisConfig = ConfigurationOptions.Parse(_configuration["Redis:ConnectionString"]);
        redisConfig.AbortOnConnectFail = false;
        redisConfig.ConnectRetry = 5;
        redisConfig.ConnectTimeout = 5000;
        redisConfig.SyncTimeout = 5000;
        redisConfig.ReconnectRetryPolicy = new LinearRetry(1000);

        try
        {
            _logger.LogInformation("Attempting to connect to Redis...");
            _connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(redisConfig);

            _connectionMultiplexer.ConnectionFailed += (sender, args) =>
                _logger.LogError($"Redis Connection Failed: {args.Exception}");

            _connectionMultiplexer.ConnectionRestored += (sender, args) =>
                _logger.LogInformation("Redis Connection Restored");

            return _connectionMultiplexer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to Redis");
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_connectionMultiplexer != null)
        {
            await _connectionMultiplexer.CloseAsync();
            _connectionMultiplexer.Dispose();
        }
    }
}
