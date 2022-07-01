using System.ComponentModel.Composition;

namespace Vocabulary.Database;

[Export(typeof(IDataContextProvider))]
internal sealed class DatabaseContextProvider : IDataContextProvider
{
    public DataContext Create() => new();
}