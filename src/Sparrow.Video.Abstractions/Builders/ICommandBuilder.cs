namespace Sparrow.Video.Abstractions.Builders
{
    public interface ICommandBuilder
    {
        ICommandBuilder Insert(string argument);
        ICommandBuilder InsertLast(string argument);
        string Build();
    }
}
