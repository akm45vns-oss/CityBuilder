using System;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using CityBuilder.Core.Logging;

namespace CityBuilder.SaveSystem
{
    /// <summary>
    /// Handles backward-compatibility migrations, version verification, and checksum checksum validation.
    /// </summary>
    public static class SaveMigrationSystem
    {
        public const int CurrentVersion = 2;

        public static string ComputeChecksum(string json)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static bool VerifyIntegrity(string json, string expectedChecksum)
        {
            if (string.IsNullOrEmpty(expectedChecksum)) return false;
            string actual = ComputeChecksum(json);
            return string.Equals(actual, expectedChecksum, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Migrates SaveDataDTO from legacy versions to the current version.
        /// </summary>
        public static SaveDataDTO Migrate(string rawJson, int saveVersion)
        {
            if (saveVersion > CurrentVersion)
            {
                throw new Exception($"Cannot load save from future version {saveVersion}. Max supported is {CurrentVersion}.");
            }

            SaveDataDTO dto = null;

            if (saveVersion == 1)
            {
                GameLogger.Info("[SaveMigrationSystem] Migrating save from Version 1 to Version 2...");
                dto = MigrateV1ToV2(rawJson);
                saveVersion = 2;
            }

            if (dto == null)
            {
                dto = JsonUtility.FromJson<SaveDataDTO>(rawJson);
            }

            dto.SaveVersion = CurrentVersion;
            return dto;
        }

        private static SaveDataDTO MigrateV1ToV2(string json)
        {
            // Simulating legacy conversion structural mapping
            // V1 used a different format for roads/buildings
            SaveDataDTO dto = JsonUtility.FromJson<SaveDataDTO>(json);
            
            if (dto.BuildingData == null)
            {
                dto.BuildingData = new BuildingSaveDTO();
            }

            // Perform any specific node/connection adaptations for backward-compatibility here
            return dto;
        }
    }
}
