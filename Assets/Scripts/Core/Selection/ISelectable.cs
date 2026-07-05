using UnityEngine;

namespace CityBuilder.Core.Selection
{
    /// <summary>
    /// Interface for any object in the game that can be selected via raycast.
    /// </summary>
    public interface ISelectable
    {
        string GetName();
        void OnSelected();
        void OnDeselected();
    }
}
