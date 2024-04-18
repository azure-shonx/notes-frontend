namespace net.shonx.privatenotes.frontend.Models;

using System.ComponentModel.DataAnnotations;

public class CreateableNote : INote
{
    [RegularExpression(Program.REGEX, ErrorMessage = "Please provide a valid secret name. Secret names can only contain alphanumeric characters and dashes.")]
    public string? Name { get; set; }

    public string? Value { get; set; }

    public CreateableNote() { }

    string? INote.Name()
    {
        return Name;
    }

    string? INote.Value()
    {
        return Value;
    }
}