using System.Diagnostics.CodeAnalysis;

namespace BotCore
{
    internal record MenuAnswer(string Command, long UserId, string Payload);

    internal record TextRecord(string Text);

    internal record KeyValueRecord(string RawData, [AllowNull] string Key = null, [AllowNull] string Value = null);
}
