using UnityEngine;
using System.Collections.Generic;

namespace CityBuilder.Core.Configs.Buildings
{
    /// <summary>
    /// Centralized registry of all BuildingDefinitions.
    /// Acts as the game's building catalog — the UI palette queries this.
    /// </summary>
    [CreateAssetMenu(fileName = "BuildingDatabase", menuName = "CityBuilder/Buildings/BuildingDatabase")]
    public class BuildingDatabase : ScriptableObject
    {
        [SerializeField] private List<BuildingDefinition> _definitions = new List<BuildingDefinition>();

        private Dictionary<string, BuildingDefinition> _lookup;

        /// <summary>
        /// Call once during initialization to build the lookup dictionary.
        /// </summary>
        public void Initialize()
        {
            _lookup = new Dictionary<string, BuildingDefinition>();
            foreach (var def in _definitions)
            {
                if (def != null && !string.IsNullOrEmpty(def.BuildingID))
                {
                    _lookup[def.BuildingID] = def;
                }
            }
        }

        public BuildingDefinition GetByID(string id)
        {
            if (_lookup == null) Initialize();
            return _lookup.TryGetValue(id, out var def) ? def : null;
        }

        public IReadOnlyList<BuildingDefinition> GetAll() => _definitions;

        public List<BuildingDefinition> GetByCategory(BuildingCategory category)
        {
            return _definitions.FindAll(d => d.Category == category);
        }

        public List<BuildingDefinition> Search(string query)
        {
            if (string.IsNullOrEmpty(query)) return _definitions;
            string lower = query.ToLower();
            return _definitions.FindAll(d =>
                d.BuildingName.ToLower().Contains(lower) ||
                d.BuildingID.ToLower().Contains(lower));
        }
    }
}
