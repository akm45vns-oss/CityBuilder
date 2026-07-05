using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Logging;
using CityBuilder.Buildings;
using System.Collections.Generic;

namespace CityBuilder.Managers
{
    public class ConstructionJob
    {
        public Building TargetBuilding;
        public float TotalTime;
        public float TimeRemaining;
        public bool IsPaused;

        public float Progress => Mathf.Clamp01(1f - TimeRemaining / TotalTime);
    }

    /// <summary>
    /// Manages the queue of buildings under construction.
    /// Supports Pause, Cancel, and Instant Build modes.
    /// </summary>
    public class ConstructionManager : Singleton<ConstructionManager>, IService
    {
        private bool _isInitialized;
        private List<ConstructionJob> _activeJobs = new List<ConstructionJob>();

        public IReadOnlyList<ConstructionJob> ActiveJobs => _activeJobs;

        public void Initialize()
        {
            if (_isInitialized) return;
            ServiceLocator.Register<ConstructionManager>(this);
            GameLogger.Info("[ConstructionManager] Initialized.");
            _isInitialized = true;
        }

        private void Update()
        {
            if (!_isInitialized || _activeJobs.Count == 0) return;

            for (int i = _activeJobs.Count - 1; i >= 0; i--)
            {
                var job = _activeJobs[i];
                if (job.IsPaused || job.TargetBuilding == null) continue;

                job.TimeRemaining -= Time.deltaTime;

                if (job.TimeRemaining <= 0)
                {
                    job.TargetBuilding.FinishConstruction();
                    _activeJobs.RemoveAt(i);
                    GameLogger.Verbose($"[ConstructionManager] Finished: {job.TargetBuilding.Definition.BuildingName}");
                }
            }
        }

        public void AddToQueue(Building building, bool instantBuild = false)
        {
            if (instantBuild || building.Definition.ConstructionTime <= 0)
            {
                building.FinishConstruction();
                return;
            }

            building.BeginConstruction();
            _activeJobs.Add(new ConstructionJob
            {
                TargetBuilding = building,
                TotalTime = building.Definition.ConstructionTime,
                TimeRemaining = building.Definition.ConstructionTime,
                IsPaused = false
            });
        }

        public void PauseConstruction(Building building)
        {
            var job = _activeJobs.Find(j => j.TargetBuilding == building);
            if (job != null) job.IsPaused = true;
        }

        public void ResumeConstruction(Building building)
        {
            var job = _activeJobs.Find(j => j.TargetBuilding == building);
            if (job != null) job.IsPaused = false;
        }

        public void CancelConstruction(Building building)
        {
            _activeJobs.RemoveAll(j => j.TargetBuilding == building);
        }

        public ConstructionJob GetJob(Building building)
        {
            return _activeJobs.Find(j => j.TargetBuilding == building);
        }
    }
}
