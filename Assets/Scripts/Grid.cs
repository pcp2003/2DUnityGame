// using UnityEngine;
// using System.Collections.Generic;
// using UnityEngine.Rendering;
// using System;
// using UnityEditor.ShaderGraph.Internal;

// public class Grid : MonoBehaviour
// {
//     // public GameObject [] treePrefabs;
//     public Material terrainMaterial;
//     public float waterLevel = .2f; // Lower value, more terrain
//     public float scale = .3f; // Higher value bigger islands
//     public int size = 100;
//     public int seed;  // Seed configurável
//     Cell[,] grid;
//     void Start() {

//         UnityEngine.Random.InitState(seed);  // Inicializa o gerador com a seed

//         float[,] noiseMap = new float[size, size];
//         (float xOffset, float yOffset) = (UnityEngine.Random.Range(-10000f, 10000f), UnityEngine.Random.Range(-10000f, 10000f));
//         for(int y = 0; y < size; y++) {
//             for(int x = 0; x < size; x++) {
//                 float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
//                 noiseMap[x, y] = noiseValue;
//             }
//         }

//         float[,] falloffMap = new float[size, size];
//         for(int y = 0; y < size; y++) {
//             for(int x = 0; x < size; x++) {
//                 float xv = x / (float)size * 2 - 1;
//                 float yv = y / (float)size * 2 - 1;
//                 float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
//                 falloffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
//             }
//         }

//         grid = new Cell[size, size];
//         for(int y = 0; y < size; y++) {
//             for(int x = 0; x < size; x++) {
//                 float noiseValue = noiseMap[x, y];
//                 noiseValue -= falloffMap[x, y];
//                 bool isWater = noiseValue < waterLevel;
//                 Cell cell = new Cell(isWater, noiseValue);
//                 grid[x, y] = cell;
//             }
//         }

//         DrawTerrainMesh(grid);
        
//     }


//     void DrawTerrainMesh(Cell[,] grid) {
//         Mesh mesh = new Mesh();
//         List<Vector3> vertices = new List<Vector3>();
//         List<int> triangles = new List<int>();
//         List<Vector2> uvs = new List<Vector2>();
//         for(int y = 0; y < size; y++) {
//             for(int x = 0; x < size; x++) {
//                 Cell cell = grid[x, y];
//                 if(!cell.isWater) {
//                     Vector3 a = new Vector3(x - .5f, 0, y + .5f);
//                     Vector3 b = new Vector3(x + .5f, 0, y + .5f);
//                     Vector3 c = new Vector3(x - .5f, 0, y - .5f);
//                     Vector3 d = new Vector3(x + .5f, 0, y - .5f);
//                     Vector2 uvA = new Vector2(x / (float)size, y / (float)size);
//                     Vector2 uvB = new Vector2((x + 1) / (float)size, y / (float)size);
//                     Vector2 uvC = new Vector2(x / (float)size, (y + 1) / (float)size);
//                     Vector2 uvD = new Vector2((x + 1) / (float)size, (y + 1) / (float)size);
//                     Vector3[] v = new Vector3[] { a, b, c, b, d, c };
//                     Vector2[] uv = new Vector2[] { uvA, uvB, uvC, uvB, uvD, uvC };
//                     for(int k = 0; k < 6; k++) {
//                         vertices.Add(v[k]);
//                         triangles.Add(triangles.Count);
//                         uvs.Add(uv[k]);
//                     }
//                 }
//             }
//         }
//         mesh.vertices = vertices.ToArray();
//         mesh.triangles = triangles.ToArray();
//         mesh.uv = uvs.ToArray();
//         mesh.RecalculateNormals();

//         MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
//         meshFilter.mesh = mesh;

//         MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
//         meshRenderer.material = terrainMaterial;
//     }

//     // void GenerateTrees(Cell[,] grid) {
//     //     float[,] noiseMap = new float[size, size];
//     //     (float xOffset, float yOffset) = (UnityEngine.Random.Range(-10000f, 10000f), UnityEngine.Random.Range(-10000f, 10000f));
//     //     for(int y = 0; y < size; y++) {
//     //         for(int x = 0; x < size; x++) {
//     //             float noiseValue = Mathf.PerlinNoise(x * treeNoiseScale + xOffset, y * treeNoiseScale + yOffset);
//     //             noiseMap[x, y] = noiseValue;
//     //         }
//     //     }

//     //     for(int y = 0; y < size; y++) {
//     //         for(int x = 0; x < size; x++) {
//     //             Cell cell = grid[x, y];
//     //             if(!cell.isWater && cell.height < 1f) {
//     //                 float v = UnityEngine.Random.Range(0f, treeDensity);
//     //                 if(noiseMap[x, y] < v) {
//     //                     GameObject prefab = treePrefabs[UnityEngine.Random.Range(0, treePrefabs.Length)];
//     //                     GameObject tree = Instantiate(prefab, transform);
//     //                     tree.transform.position = new Vector3(x, cell.height, y);
//     //                     tree.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360f), 0);
//     //                     tree.transform.localScale = Vector3.one * UnityEngine.Random.Range(.8f, 1.2f);
//     //                 }
//     //             }
//     //         }
//     //     }
//     // }

    
// }
