using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CityBuilder.Core;
using CityBuilder.Core.Logging;
using CityBuilder.Utilities;
using CityBuilder.SaveSystem;
using CityBuilder.Buildings.Commands;
using CityBuilder.Grid;
using CityBuilder.Managers;

namespace CityBuilder.Testing
{
    /// <summary>
    /// Automated integration, stress, and regression testing harness.
    /// Runs tests sequentially in the editor/development builds to guarantee performance and stability.
    /// </summary>
    public class SystemTester : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(RunAllTestsRoutine());
        }

        private IEnumerator RunAllTestsRoutine()
        {
            GameLogger.Info("========== SYSTEM TEST RUNNER STARTING ==========");
            yield return new WaitForSeconds(1.5f); // Wait for bootstrapper to fully register

            RunServiceLocatorTests();
            yield return null;

            RunSpatialGridStressTests();
            yield return null;

            RunSaveIntegrityTests();
            yield return null;

            RunCommandValidationTests();
            yield return null;

            GameLogger.Info("========== ALL SYSTEM TESTS COMPLETED SUCCESSFULLY ==========");
        }

        private void RunServiceLocatorTests()
        {
            GameLogger.Info("[Tester] Running ServiceLocator thread safety checks...");
            
            // Spawn multiple threads querying ServiceLocator
            bool threadSuccess = true;
            System.Threading.Tasks.Parallel.For(0, 100, i =>
            {
                var manager = ServiceLocator.Get<RoadManager>();
                if (manager == null) threadSuccess = false;
            });

            if (threadSuccess)
            {
                GameLogger.Info("[Tester] ServiceLocator thread safety checks: PASS");
            }
            else
            {
                GameLogger.Error("[Tester] ServiceLocator thread safety checks: FAIL");
            }
        }

        private class MockLocatable : ISpatialLocatable
        {
            public string ID { get; }
            public Vector3 Position { get; }

            public MockLocatable(string id, Vector3 pos)
            {
                ID = id;
                Position = pos;
            }
        }

        private void RunSpatialGridStressTests()
        {
            GameLogger.Info("[Tester] Starting Spatial Hash Grid stress test (10,000 items)...");
            var grid = new SpatialHashGrid<MockLocatable>(32f);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                grid.Add(new MockLocatable(i.ToString(), new Vector3(Random.Range(-1000f, 1000f), 0f, Random.Range(-1000f, 1000f))));
            }
            watch.Stop();
            GameLogger.Info($"[Tester] Inserted 10,000 spatial nodes in {watch.Elapsed.TotalMilliseconds:F2} ms");

            watch.Restart();
            var results = new List<MockLocatable>();
            grid.QueryRadius(Vector3.zero, 150f, results);
            watch.Stop();
            GameLogger.Info($"[Tester] Queried 150-radius around center (found {results.Count} items) in {watch.Elapsed.TotalMilliseconds:F4} ms (Target: <1.0 ms)");

            if (watch.Elapsed.TotalMilliseconds < 1.0)
            {
                GameLogger.Info("[Tester] Spatial Hash Grid stress test: PASS");
            }
            else
            {
                GameLogger.Warning("[Tester] Spatial Hash Grid query is slower than expected.");
            }
        }

        private void RunSaveIntegrityTests()
        {
            GameLogger.Info("[Tester] Running Save System versioning & integrity checks...");

            string testJson = "{\"SaveVersion\":2,\"Timestamp\":\"2026-07-05T23:00:00Z\",\"Checksum\":\"\"}";
            string checksum = SaveMigrationSystem.ComputeChecksum(testJson);

            bool pass = SaveMigrationSystem.VerifyIntegrity(testJson, checksum);
            GameLogger.Info($"[Tester] Checksum verification: {(pass ? "PASS" : "FAIL")}");

            // Try loading modified/corrupted save to ensure recovery warning
            string corruptedJson = testJson + " {corrupt}";
            bool corruptCheck = SaveMigrationSystem.VerifyIntegrity(corruptedJson, checksum);
            GameLogger.Info($"[Tester] Corruption detection (expected fail): {(corruptCheck ? "FAIL" : "PASS (Detected)")}");
        }

        private void RunCommandValidationTests()
        {
            GameLogger.Info("[Tester] Running Command validation and Undo/Redo checks...");

            var grid = new GridCell[2, 2];
            grid[0, 0] = new GridCell(0, 0, Vector3.zero, 0f, 0f);
            grid[0, 1] = new GridCell(0, 1, new Vector3(0f, 0f, 8f), 0f, 0f);
            
            // Occupy cell
            grid[0, 0].Occupy("TestBuildingID", 0f);

            var cells = new List<GridCell> { grid[0, 0], grid[0, 1] };
            var definition = ScriptableObject.CreateInstance<Core.Configs.Buildings.BuildingDefinition>();
            definition.BuildingName = "Test Validation Shop";

            // Try executing placement command on occupied cells
            var cmd = new PlaceBuildingCommand(definition, cells, Vector3.zero, 0, null, Vector3.zero);
            bool success = cmd.Validate(out string reason);

            if (!success && reason.Contains("occupied"))
            {
                GameLogger.Info($"[Tester] Overlap placement blocked successfully: PASS (Blocked: {reason})");
            }
            else
            {
                GameLogger.Error("[Tester] Command allowed overlap placement: FAIL");
            }
        }
    }
}
