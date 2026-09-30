namespace example.solid;

/// <summary>Describes the single operation required of a printer.</summary>
public interface IPrinter
{
    /// <summary>Prints the supplied document.</summary>
    void Print(string document);
}

/// <summary>Describes the single operation required of a scanner.</summary>
public interface IScanner
{
    /// <summary>Scans a document and returns a description of the result.</summary>
    string Scan();
}

/// <summary>A printer that can print but does not need to support scanning.</summary>
public sealed class BasicPrinter : IPrinter
{
    /// <inheritdoc/>
    public void Print(string document) => Console.WriteLine($"Printing: {document}");
}

/// <summary>A device that supports both printing and scanning.</summary>
public sealed class OfficeMachine : IPrinter, IScanner
{
    /// <inheritdoc/>
    public void Print(string document) => Console.WriteLine($"Printing: {document}");

    /// <inheritdoc/>
    public string Scan() => "Scanned document";
}