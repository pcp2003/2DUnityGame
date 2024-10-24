using UnityEngine;
using UnityEngine.Tilemaps;


public class TerrainGenerator2D : MonoBehaviour
{
    public Tilemap tilemap;           // Referência ao TileMap onde os tiles serão desenhados
    public TileBase waterTile;        // Tile para água
    public TileBase landTile;         // Tile para terra  
    public int size = 100;            // Tamanho do TileMap
    public float noiseScale = 0.1f;   // Escala do Perlin Noise
    public float waterLevel = 0.4f;   // Nível da água
    public int seed = 0;              // Seed do mapa gerado 
    Cell[,] grid;

    void Start()
    {
        UnityEngine.Random.InitState(seed);  // Inicializa o gerador com a seed

        float[,] noiseMap = GenerateNoiseMap();

        float[,] falloffMap = GenerateFalloffmap();

        GenerateGrid(noiseMap,falloffMap);
        
        GenerateTerrain();
    }

    void GenerateTerrain()
    {

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++){
        
                if (grid[x,y].isWater)
                    tilemap.SetTile(new Vector3Int(x, y, 0), waterTile);
                else
                    tilemap.SetTile(new Vector3Int(x, y, 0), landTile);
            }
        }
    }


    float[,] GenerateNoiseMap(){

        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (UnityEngine.Random.Range(-10000f, 10000f), UnityEngine.Random.Range(-10000f, 10000f));
        for(int y = 0; y < size; y++) {
            for(int x = 0; x < size; x++) {
                float noiseValue = Mathf.PerlinNoise(x * noiseScale + xOffset, y * noiseScale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }
        return noiseMap;
    }

    float[,] GenerateFalloffmap(){
        
        float[,] falloffMap = new float[size, size];
        for(int y = 0; y < size; y++) {
            for(int x = 0; x < size; x++) {
                float xv = x / (float)size * 2 - 1;
                float yv = y / (float)size * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                falloffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
            }
        }
        return falloffMap;
    }

    void GenerateGrid(float [,] noiseMap, float [,] falloffMap){
        
        grid = new Cell[size, size];
        for(int y = 0; y < size; y++) {
            for(int x = 0; x < size; x++) {
                float noiseValue = noiseMap[x, y];
                noiseValue -= falloffMap[x, y];
                bool isWater = noiseValue < waterLevel;
                Cell cell = new Cell(isWater);
                grid[x, y] = cell;
            }
        }
    }

}

