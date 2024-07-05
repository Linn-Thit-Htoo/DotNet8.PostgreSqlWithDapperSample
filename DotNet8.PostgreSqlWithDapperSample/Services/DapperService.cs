namespace DotNet8.PostgreSqlWithDapperSample.Services;

public class DapperService
{
    private readonly IConfiguration _configuration;

    public DapperService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<T>> QueryAsync<T>(
        string query,
        object? parameters = null,
        CommandType commandType = CommandType.Text
    )
    {
        using IDbConnection db = GetConnection();
        var lst = await db.QueryAsync<T>(query, parameters, commandType: commandType);

        return lst.ToList();
    }

    public async Task<T> QueryFirstOrDefaultAsync<T>(
        string query,
        object? parameters = null,
        CommandType commandType = CommandType.Text
    )
    {
        using IDbConnection db = GetConnection();
        var item = await db.QueryFirstOrDefaultAsync<T>(
            query,
            parameters,
            commandType: commandType
        );
        return item!;
    }

    public async Task<int> ExecuteAsync(string query, object parameters)
    {
        using IDbConnection db = GetConnection();
        return await db.ExecuteAsync(query, parameters);
    }

    private NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(
            _configuration.GetConnectionString("DbConnection")
        );
    }
}
