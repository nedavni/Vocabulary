using System.ComponentModel.Composition;

namespace Vocabulary.Database;

[Export(typeof(IDataContextProvider))]
internal sealed class DatabaseContextProvider : IDataContextProvider
{
    public DataContext Create()
    {
        var context = new DataContext();
        context.Database.EnsureCreated();
        return context;
    }
}