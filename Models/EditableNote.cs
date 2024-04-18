namespace net.shonx.privatenotes.frontend.Models;

using net.shonx.privatenotes.backend;
using System.ComponentModel.DataAnnotations;


public class EditableNote : INote
{

    public string ID { get; set; } // Doesn't change, but needs to have set for MVC.

    public string OldValue { get; set; } // Doesn't change, but needs to have set for MVC.

    [RegularExpression(Program.REGEX, ErrorMessage = "Please provide a valid secret name. Secret names can only contain alphanumeric characters and dashes.")]
    public string? Name { get; set; }

    public string? Value { get; set; }

#pragma warning disable CS8618
    public EditableNote() { }

    public EditableNote(Note note)
    {
        this.ID = note.Name;
        this.Name = note.Name;
        this.OldValue = note.Value;
        this.Value = note.Value;
    }

    public bool NameRenamed()
    {
        if (ID.Equals(Name))
        {
            Name = null;
            return false;
        }
        return true;
    }

    public bool ValueChanged()
    {
        if (OldValue.Equals(Value))
        {
            Value = null;
            return false;
        }
        return true;
    }

    string? INote.Name()
    {
        return Name;
    }

    string? INote.Value()
    {
        return Value;
    }
}