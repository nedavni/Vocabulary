using System.Collections.Generic;

namespace Vocabulary;

public readonly record struct WordWithMeanings(string Word, IReadOnlyList<string> Meanings);