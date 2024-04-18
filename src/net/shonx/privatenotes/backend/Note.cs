namespace net.shonx.privatenotes.backend;

using net.shonx.privatenotes.frontend;
using net.shonx.privatenotes.frontend.Models;
using Newtonsoft.Json;

public class Note : INote
{
    public string Name { get; }
    public string Value { get; }

    [JsonConstructor]
    public Note(string name, string value)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
            throw new NullReferenceException();
        this.Name = name;
        this.Value = value;
    }

    public Note(CreateableNote CreateableNote)
    {
        if (CreateableNote is null)
            throw new NullReferenceException();
        if (string.IsNullOrEmpty(CreateableNote.Name) || string.IsNullOrEmpty(CreateableNote.Value))
            throw new NullReferenceException();
        this.Name = CreateableNote.Name;
        this.Value = CreateableNote.Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Note o)
            return false;
        return Name.Equals(o.Name) && Value.Equals(o.Value);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode() ^ Value.GetHashCode();
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
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