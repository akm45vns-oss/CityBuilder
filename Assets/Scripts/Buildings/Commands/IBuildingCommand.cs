namespace CityBuilder.Buildings.Commands
{
    /// <summary>
    /// Interface for all undoable/redoable building operations.
    /// </summary>
    public interface IBuildingCommand
    {
        bool Validate(out string reason);
        void Execute();
        void Undo();
    }
}
