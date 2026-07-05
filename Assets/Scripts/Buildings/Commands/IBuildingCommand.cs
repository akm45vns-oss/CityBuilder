namespace CityBuilder.Buildings.Commands
{
    /// <summary>
    /// Interface for all undoable/redoable building operations.
    /// </summary>
    public interface IBuildingCommand
    {
        void Execute();
        void Undo();
    }
}
