using UnityEngine;
using System.Collections.Generic;
using CityBuilder.Core.Configs.Roads;

namespace CityBuilder.Roads
{
    /// <summary>
    /// Handles procedural mesh generation for roads and intersections.
    /// Fully decoupled from the RoadNetwork graph logic.
    /// </summary>
    public class RoadMeshGenerator : MonoBehaviour
    {
        private Dictionary<string, GameObject> _segmentMeshes = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject> _intersectionMeshes = new Dictionary<string, GameObject>();

        public void RebuildMesh(RoadSegment segment)
        {
            if (_segmentMeshes.TryGetValue(segment.ID, out GameObject existing))
            {
                Destroy(existing);
            }

            GameObject go = new GameObject($"Segment_{segment.ID}");
            go.transform.SetParent(transform);
            
            MeshFilter filter = go.AddComponent<MeshFilter>();
            MeshRenderer renderer = go.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = segment.Settings.RoadMaterial;

            // Generate simple quad from start to end (ignores spline curvature for this basic implementation)
            Vector3 startPos = segment.StartNode.Position;
            Vector3 endPos = segment.EndNode.Position;
            Vector3 dir = (endPos - startPos).normalized;
            Vector3 right = Vector3.Cross(Vector3.up, dir).normalized;
            
            float halfWidth = segment.Settings.RoadWidth / 2f;

            // Apply slight offset so roads sit slightly above terrain to prevent Z-fighting
            Vector3 offset = Vector3.up * 0.05f;

            Vector3[] vertices = new Vector3[4];
            vertices[0] = startPos - right * halfWidth + offset;
            vertices[1] = startPos + right * halfWidth + offset;
            vertices[2] = endPos - right * halfWidth + offset;
            vertices[3] = endPos + right * halfWidth + offset;

            int[] triangles = { 0, 2, 1, 2, 3, 1 };
            Vector2[] uvs = { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };

            Mesh mesh = new Mesh
            {
                vertices = vertices,
                triangles = triangles,
                uv = uvs
            };
            mesh.RecalculateNormals();

            filter.mesh = mesh;
            _segmentMeshes[segment.ID] = go;
        }

        public void RebuildIntersection(RoadNode node)
        {
            if (_intersectionMeshes.TryGetValue(node.ID, out GameObject existing))
            {
                Destroy(existing);
            }

            // Don't build intersections for dead ends
            if (node.IsDeadEnd()) return;

            GameObject go = new GameObject($"Intersection_{node.ID}");
            go.transform.SetParent(transform);
            
            MeshFilter filter = go.AddComponent<MeshFilter>();
            MeshRenderer renderer = go.AddComponent<MeshRenderer>();
            
            // Fallback material if missing
            Material mat = null;
            float radius = 3f;
            
            if (node.ConnectedSegments.Count > 0)
            {
                mat = node.ConnectedSegments[0].Settings.IntersectionMaterial;
                radius = node.ConnectedSegments[0].Settings.NodeRadius;
            }

            renderer.sharedMaterial = mat;

            // Generate flat polygonal disc for junction
            int segments = 16;
            Vector3[] vertices = new Vector3[segments + 1];
            int[] triangles = new int[segments * 3];
            Vector2[] uvs = new Vector2[segments + 1];

            Vector3 offset = Vector3.up * 0.06f; // Slightly above roads
            vertices[0] = node.Position + offset;
            uvs[0] = new Vector2(0.5f, 0.5f);

            float angleStep = 360f / segments;
            for (int i = 0; i < segments; i++)
            {
                float rad = Mathf.Deg2Rad * (i * angleStep);
                Vector3 point = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad)) * radius;
                vertices[i + 1] = node.Position + point + offset;
                
                uvs[i + 1] = new Vector2((point.x / radius + 1) * 0.5f, (point.z / radius + 1) * 0.5f);

                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = (i == segments - 1) ? 1 : i + 2;
            }

            Mesh mesh = new Mesh
            {
                vertices = vertices,
                triangles = triangles,
                uv = uvs
            };
            mesh.RecalculateNormals();

            filter.mesh = mesh;
            _intersectionMeshes[node.ID] = go;
        }

        public void DestroyMesh(string segmentId)
        {
            if (_segmentMeshes.TryGetValue(segmentId, out GameObject go))
            {
                Destroy(go);
                _segmentMeshes.Remove(segmentId);
            }
        }

        public void DestroyIntersection(string nodeId)
        {
            if (_intersectionMeshes.TryGetValue(nodeId, out GameObject go))
            {
                Destroy(go);
                _intersectionMeshes.Remove(nodeId);
            }
        }
    }
}
