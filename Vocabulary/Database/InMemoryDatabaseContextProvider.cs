using Microsoft.EntityFrameworkCore;

namespace Vocabulary.Database;

internal sealed class InMemoryDatabaseContextProvider : IDataContextProvider
{
    public DataContext Create()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "Test")
            .Options;
        return new DataContext(options);
    }
}