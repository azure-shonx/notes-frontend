namespace net.shonx.privatenotes.frontend;

using System.Text.RegularExpressions;


public partial interface INote
{

    public string? Name();

    public string? Value();

    public bool IsValidName()
    {
        if (string.IsNullOrEmpty(Name()))
            throw new NullReferenceException();
#pragma warning disable CS8604
        return MyRegex().IsMatch(Name());
    }

    [GeneratedRegex(Program.REGEX)]
    private static partial Regex MyRegex();
}