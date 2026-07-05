using CityBuilder.Core.Selection;

namespace CityBuilder.Events
{
    public struct ObjectSelectedEvent
    {
        public ISelectable SelectedObject;
    }

    public struct ObjectDeselectedEvent
    {
        public ISelectable DeselectedObject;
    }
}
