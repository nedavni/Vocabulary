namespace Vocabulary.Database
{
    internal interface IDataContextProvider
    {
        public DataContext Create();
    }
}
