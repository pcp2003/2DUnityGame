using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class TerrainGenerator2D : MonoBehaviour
{
    public Tilemap tilemap;                     // Referência ao TileMap onde os tiles serão desenhados
    public TileBase[] waterTiles;               // Conjunto de tiles para água
    public TileBase[] grassTiles;               // Conjunto de tiles para grama
    public TileBase[] grassWithWater;           // Tiles de borda de grama próxima à água
    public GameObject[] treeObjects;                // Tiles de árvores
    public TileBase[] bushesAndRocksTiles;      // Tiles de arbustos e pedras
    public int size = 100;                      // Tamanho do mapa
    public float noiseScale = 0.1f;             // Escala do Perlin Noise para geração do terreno
    public float waterLevel = 0.4f;             // Nível de água
    public int seed = 0;                        // Semente para gerar o mapa
    public float treeNoiseScale = 0.1f;         // Escala do Perlin Noise para geração das árvores
    public float treeDensity = .5f;             // Densidade de árvores
    public float bushesAndRocksDensity = 0.01f; // Chance de gerar pedras e arbustos
    public float rocksInWaterDensity = 0.02f;   // Chance de gerar pedras na água

    Cell[,] grid;                               // Matriz para representar o terreno e características de cada célula

    void Start()
    {
        UnityEngine.Random.InitState(seed);                                             // Inicializa o gerador com a seed para repetibilidade
        GenerateGrid(GenerateNoiseMap(noiseScale), GenerateFalloffMap());               // Preenche o grid com dados do terreno
        GenerateTerrain();                                                              // Gera o terreno no Tilemap
        GenerateTrees(treeNoiseScale, treeDensity);                                     // Gera árvores a partir de um novo NoiseMap e a Grid
    }

    // Preenche a grade com valores de células de acordo com o ruído e o mapa de queda
    void GenerateGrid(float[,] noiseMap, float[,] falloffMap)
    {
        grid = new Cell[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = noiseMap[x, y] - falloffMap[x, y];
                grid[x, y] = new Cell(noiseValue < waterLevel, false);
            }
        }
    }

    // Gera e desenha o terreno no Tilemap
    void GenerateTerrain()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {   
                // Define tile de água
                if (grid[x, y].isWater)  tilemap.SetTile(new Vector3Int(x, y, 0), SelectWaterTile(rocksInWaterDensity));

                else  
                {
                    int countAdjWater = CountAdjacentWater(x, y); // Conta tiles adjacentes de água
                    tilemap.SetTile(new Vector3Int(x, y, 0), SelectTile(x, y, countAdjWater)); // Define tiles de grama ou borda

                    // Gera pedra e arbustos.
                    GenerateObjectOnLayerSpecified(x,y, bushesAndRocksDensity, bushesAndRocksTiles);
                }
            }
        }
    }

    

    // Gera ativos como arbustos ou pedras no layer especificado
    void GenerateObjectOnLayerSpecified(int x, int y, float density, TileBase[] tiles)
    {
        float randomValue = UnityEngine.Random.Range(0f, 1f);

        if (randomValue < density && !grid[x,y].isOccupied)
        
            tilemap.SetTile(new Vector3Int(x, y, 1), tiles[UnityEngine.Random.Range(0, tiles.Length)]);
        
    }

    // Gera árvores a partir de um parâmetro treeNoiseScale e um parâmetro treeDensity
    void GenerateTrees (float treeNoiseScale, float treeDensity) {
        
        float [,] noiseMap = GenerateNoiseMap(treeNoiseScale);

        for (int x = 0; x != size; x++){
            for (int y = 0; y != size; y++){
                if (!grid[x,y].isOccupied && !grid[x,y].isWater && noiseMap[x,y] < UnityEngine.Random.Range(0f, treeDensity)){
                    GameObject newTree = Instantiate(treeObjects[UnityEngine.Random.Range(0, treeObjects.Length)], new Vector3Int(x,y,1), new Quaternion());
                    newTree.transform.localScale = Vector3.one * UnityEngine.Random.Range(.8f, 1.2f);
                    grid[x,y].setIsOccupied(true);
                }
            }
        }
    }

    // Gera um mapa de ruído usando Perlin Noise
    float[,] GenerateNoiseMap(float noiseScale)
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (UnityEngine.Random.Range(-10000f, 10000f), UnityEngine.Random.Range(-10000f, 10000f));

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                noiseMap[x, y] = Mathf.PerlinNoise(x * noiseScale + xOffset, y * noiseScale + yOffset);
            }
        }
        return noiseMap;
    }

    // Gera o mapa de queda para limitar a área da ilha
    float[,] GenerateFalloffMap()
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

    

     // Seleciona o tile apropriado com base nos adjacentes de água
    TileBase SelectTile(int x, int y, int waterCount)
    {
        // Verificações de bordas e água para aplicar o tile correto de borda ou grama
        bool up = IsWithinBounds(x, y + 1);
        bool down = IsWithinBounds(x, y - 1); 
        bool left = IsWithinBounds(x - 1, y);
        bool right = IsWithinBounds(x + 1, y);

        if (up) up = grid[x, y + 1].isWater;
        if (down) down = grid[x, y - 1].isWater;
        if (left) left = grid[x - 1, y].isWater;
        if (right) right = grid[x + 1, y].isWater;

        // Retorna tile de borda apropriado com base na contagem de água adjacente
        // (Cada caso de contagem recebe um tile diferente de borda)
        if (waterCount == 4) return grassWithWater[25];

        else if (waterCount == 3)
        {
            if (!up) return grassWithWater[UnityEngine.Random.Range(16, 18)];
            if (!down) return grassWithWater[UnityEngine.Random.Range(18, 20)];
            if (!left) return grassWithWater[UnityEngine.Random.Range(22, 24)];
            if (!right) return grassWithWater[UnityEngine.Random.Range(20, 22)];
        }
        else if (waterCount == 2)
        {
            if (up && right) return grassWithWater[10];        // Canto superior direito
            if (up && left) return grassWithWater[11];         // Canto superior esquerdo
            if (down && right) return grassWithWater[9];       // Canto inferior direito
            if (down && left) return grassWithWater[8];        // Canto inferior esquerdo
            if (left && right) return grassWithWater[UnityEngine.Random.Range(12, 14)]; // Ponte vertical
            if (up && down) return grassWithWater[UnityEngine.Random.Range(14, 16)];   // Ponte horizontal
        }
        else if (waterCount == 1)
        {
            if (up) return grassWithWater[UnityEngine.Random.Range(0, 2)];
            else if (down) return grassWithWater[UnityEngine.Random.Range(2, 4)];
            else if (left) return grassWithWater[UnityEngine.Random.Range(4, 6)];
            else if (right) return grassWithWater[UnityEngine.Random.Range(6, 8)];
        }

        if (IsWithinBounds(x - 1, y + 1)) 
            if (grid[x - 1, y + 1].isWater) return grassWithWater[26];

        else if (IsWithinBounds(x + 1, y + 1))
            if (grid[x + 1, y + 1].isWater) return grassWithWater[27];

        else if (IsWithinBounds(x + 1, y - 1))
            if (grid[x + 1, y - 1].isWater) return grassWithWater[28];

        else if (IsWithinBounds(x - 1, y - 1))
            if (grid[x - 1, y - 1].isWater) return grassWithWater[29];

        return grassTiles[UnityEngine.Random.Range(0, grassTiles.Length)]; // Tile de grama padrão
    }


    // Seleciona aleatoriamente um tile de água
    TileBase SelectWaterTile(float rocksOnWaterDensity) {
        return UnityEngine.Random.Range(0f, 1f) < rocksOnWaterDensity ? waterTiles[UnityEngine.Random.Range(1, waterTiles.Length)] :  waterTiles[0];
    }

    // Função para contar o número de tiles de água adjacentes
    int CountAdjacentWater(int x, int y)
    {
        int waterCount = 0;

        if (grid[x - 1, y].isWater) waterCount++;      // Esquerda
        if (grid[x + 1, y].isWater) waterCount++;      // Direita
        if (grid[x, y - 1].isWater) waterCount++;      // Abaixo
        if (grid[x, y + 1].isWater) waterCount++;      // Acima

        return waterCount;
    }

   
    // Verifica se uma posição está dentro dos limites do mapa
    bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < size && y >= 0 && y < size;
    }
}
