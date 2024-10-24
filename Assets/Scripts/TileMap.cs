using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator2D : MonoBehaviour
{
    public Tilemap tilemap;           // Referência ao TileMap onde os tiles serão desenhados
    public TileBase[] waterTiles;     // Water tile + Water and rocks
    public TileBase[] grassTiles;     // Simple grass tiles
    public TileBase[] grassWithWater; // Parts of land next to the sea or lakes
    public int size = 100;            // Tamanho do TileMap
    public float noiseScale = 0.1f;   // Escala do Perlin Noise
    public float waterLevel = 0.4f;   // Nível da água
    public int seed = 0;              // Seed do mapa gerado 

    Cell[,] grid;                     // Grid

    void Start()
    {
        UnityEngine.Random.InitState(seed);  // Inicializa o gerador com a seed

        float[,] noiseMap = GenerateNoiseMap();
        float[,] falloffMap = GenerateFalloffmap();

        GenerateGrid(noiseMap, falloffMap);
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (grid[x, y].isWater)
                {
                    TileBase waterTile = SelectWaterTile();
                    tilemap.SetTile(new Vector3Int(x, y, 0), waterTile);
                }
                else
                {
                    int countAdjWater = CountAdjacentWater(x, y);

                    tilemap.SetTile(new Vector3Int(x, y, 0), SelectTile(x,y,countAdjWater));

                }
            }
        }
    }

    int CountAdjacentWater(int x, int y)
    {
        int waterCount = 0;

        if (grid[x - 1, y].isWater) waterCount++;      // Esquerda
        if (grid[x + 1, y].isWater) waterCount++;      // Direita
        if (grid[x, y - 1].isWater) waterCount++;      // Baixo
        if (grid[x, y + 1].isWater) waterCount++;      // Cima

        return waterCount;
    }

    TileBase SelectTile(int x, int y, int waterCount)
    {
        bool up = grid[x, y + 1].isWater;
        bool down = grid[x, y - 1].isWater;
        bool left = grid[x - 1, y].isWater;
        bool right = grid[x + 1, y].isWater;

        if (waterCount == 4) return grassWithWater[25];

        else if (waterCount == 3)
        {
            if (!up)
                return grassWithWater[UnityEngine.Random.Range(16, 18)];
            if (!down)
                return grassWithWater[UnityEngine.Random.Range(18, 20)];
            if (!left)
                return grassWithWater[UnityEngine.Random.Range(22, 24)];
            if (!right)
                return grassWithWater[UnityEngine.Random.Range(20, 22)];
        }

        else if (waterCount == 2)
        {

            if (up && right) return grassWithWater[10];        // TopRight corner
            if (up && left) return grassWithWater[11];          // TopLeft corner
            if (down && right) return grassWithWater[9];       // BottomRight corner
            if (down && left) return grassWithWater[8];        // BottomLeft corner
            if (left && right) return grassWithWater[UnityEngine.Random.Range(12, 14)];       // Vertical bridge 
            if (up && down) return grassWithWater[UnityEngine.Random.Range(14, 16)];         // Horizontal bridge
        }

        else if (waterCount == 1) {

            if (up) return grassWithWater[UnityEngine.Random.Range(0, 2)];

            else if (down) return grassWithWater[UnityEngine.Random.Range(2, 4)];

            else if (left) return grassWithWater[UnityEngine.Random.Range(4, 6)];

            else if (right) return grassWithWater[UnityEngine.Random.Range(6, 8)];
        }

        return grassTiles[UnityEngine.Random.Range(0, grassTiles.Length)];

    }

    float[,] GenerateNoiseMap()
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (UnityEngine.Random.Range(-10000f, 10000f), UnityEngine.Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * noiseScale + xOffset, y * noiseScale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }
        return noiseMap;
    }

    float[,] GenerateFalloffmap()
    {
        float[,] falloffMap = new float[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float xv = x / (float)size * 2 - 1;
                float yv = y / (float)size * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                falloffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
            }
        }
        return falloffMap;
    }

    void GenerateGrid(float[,] noiseMap, float[,] falloffMap)
    {
        grid = new Cell[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = noiseMap[x, y];
                noiseValue -= falloffMap[x, y];
                bool isWater = noiseValue < waterLevel;
                Cell cell = new Cell(isWater);
                grid[x, y] = cell;
            }
        }
    }

    TileBase SelectWaterTile()
    {
        float randomValue = UnityEngine.Random.Range(0f, 1f);
        if (randomValue < 0.98f)
        {
            return waterTiles[0];
        }
        else
        {
            return waterTiles[UnityEngine.Random.Range(1, waterTiles.Length)];
        }
    }
}
