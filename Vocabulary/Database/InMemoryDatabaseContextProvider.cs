using System;
using Microsoft.EntityFrameworkCore;

namespace Vocabulary.Database;

internal sealed class InMemoryDatabaseContextProvider : IDataContextProvider
{
    private readonly string _databaseName;

    public InMemoryDatabaseContextProvider()
    {
        _databaseName = Guid.NewGuid().ToString();
    }

    public DataContext Create()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: _databaseName)
            .Options;
        return new DataContext(options);
    }
}