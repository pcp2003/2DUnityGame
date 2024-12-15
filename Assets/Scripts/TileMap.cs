using System;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class TileMap : MonoBehaviour
{
    private Tilemap tilemap;                     // Referência ao TileMap onde os tiles serão desenhados
    public TileBase[] waterTiles;               // Conjunto de tiles para água
    public TileBase[] grassTiles;               // Conjunto de tiles para grama
    public TileBase[] grassWithWater;           // Tiles de borda de grama próxima à água
    public GameObject[] treePrefabs;            // Prefab de árvores
    public GameObject[] bushesAndRocks;         // Tiles de arbustos e pedras
    public GameObject[] chests;                 // baús
    public GameObject player;                   // Player
    public int size = 300;                      // Tamanho do mapa
    public float noiseScale = 0.1f;             // Escala do Perlin Noise para geração do terreno
    public float waterLevel = 0.4f;             // Nível de água
    private int seed = 0;                        // Semente para gerar o mapa
    public float treeNoiseScale = 0.1f;         // Escala do Perlin Noise para geração das árvores
    public float treeDensity = .5f;             // Densidade de árvores
    public float bushesAndRocksDensity = 0.01f; // Chance de gerar pedras e arbustos
    public float rocksInWaterDensity = 0.02f;   // Chance de gerar pedras na água
    public float chanceToSpawnChest = 0.001f;   // Chance de spawnar um baú
    public CameraFollow followCam;
    public CanvasUpdate canvas;
    public PowerUpManager powerUpManager;
    private Spawner spawner;
    public Cell[,] grid;                        // Matriz para representar o terreno e características de cada célula
    public GameOverScreen gameOverScreen;

    void Start()
    {
        seed = UnityEngine.Random.Range(0, 100);
        tilemap = GetComponent<Tilemap>();
        spawner = GetComponent<Spawner>();
        UnityEngine.Random.InitState(seed);                                             // Inicializa o gerador com a seed para repetibilidade
        GenerateGrid(GenerateNoiseMap(noiseScale), GenerateFalloffMap());               // Preenche o grid com dados do terreno
        GenerateTerrain();                                                              // Gera o terreno no Tilemap
        GenerateTrees(treeNoiseScale, treeDensity);                                     // Gera árvores a partir de um novo NoiseMap e a Grid
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        // Verifica posições aleatórias até encontrar uma célula válida
        while (true)
        {
            int x = UnityEngine.Random.Range(0, size);
            int y = UnityEngine.Random.Range(0, size);

            // Verifica se a célula não é água e não está ocupada
            if (!grid[x, y].isWater && !grid[x, y].isOccupied)
            {
                GameObject playerInstance = Instantiate(player, transform);
                playerInstance.transform.position = new Vector3(x, y, 0) + new Vector3(0.5f, 0.5f, 0);
                followCam.SetTarget(playerInstance);
                spawner.SetPlayerReference(playerInstance);
                spawner.SetCanvas(canvas);
                playerInstance.GetComponent<PlayerController>().SetCanvas(canvas);
                playerInstance.GetComponent<PlayerController>().SetGameOverScreen(gameOverScreen);
                powerUpManager.SetPlayer(playerInstance);
                powerUpManager.SetCanvas(canvas);
                gameOverScreen.SetCanvas(canvas);
                Debug.Log($"Player spawned at: ({x}, {y})");
                break;
            }
        }
    }

    public void SpawnEnemy(GameObject enemy, GameObject playerReference, float exactDistanceFromPlayer, float scaleFactor)
    {
        string enemyName = enemy.name;
        List<Vector3> validPositions = new List<Vector3>();
        Vector3 playerPosition = playerReference.transform.position;

        // Busca posições válidas, ajustando condições automaticamente
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (grid[x, y].isWater || grid[x, y].isOccupied) continue;

                Vector3 potentialPosition = new Vector3(x, y, 0) + new Vector3(0.5f, 0.5f, 0);
                float distance = Vector3.Distance(potentialPosition, playerPosition);

                // Adiciona posição se for exata ou próxima
                if (Mathf.Abs(distance - exactDistanceFromPlayer) < 0.1f ||
                    distance <= exactDistanceFromPlayer + 5f)
                {
                    validPositions.Add(potentialPosition);
                }
            }
        }

        // Se nenhuma posição foi encontrada, adiciona qualquer posição válida
        if (validPositions.Count == 0)
        {
            Debug.LogWarning("No suitable positions found. Using any valid position...");
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (!grid[x, y].isWater && !grid[x, y].isOccupied)
                    {
                        validPositions.Add(new Vector3(x, y, 0) + new Vector3(0.5f, 0.5f, 0));
                    }
                }
            }
        }

        // Seleciona uma posição aleatória
        Vector3 spawnPosition = validPositions[UnityEngine.Random.Range(0, validPositions.Count)];

        // Instancia o inimigo
        GameObject enemyInstance = Instantiate(enemy, transform);
        enemyInstance.transform.position = spawnPosition;

        // Configura referências específicas do inimigo
        if (enemyName == "Soldier"){
            Soldier soldier =  enemyInstance.GetComponent<Soldier>();
            soldier.SetPlayerReference(playerReference);
            soldier.setScaleFactor(scaleFactor);
            soldier.updateStats();
        }

        else if (enemyName == "Goblin"){
            Goblin goblin =  enemyInstance.GetComponent<Goblin>();
            goblin.SetPlayerReference(playerReference);
            goblin.setScaleFactor(scaleFactor);
            goblin.updateStats();
        }
        else
            Debug.LogWarning($"Enemy type '{enemyName}' is not recognized.");

        // Debug.Log($"Enemy '{enemyName}' spawned at: {spawnPosition}");
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

                // isOcuppied não permite a geração de texturas em terrenos a volta da agua
                bool isOcuppied = noiseValue < waterLevel + 0.1f;

                grid[x, y] = new Cell(noiseValue < waterLevel, isOcuppied);
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
                if (grid[x, y].isWater) tilemap.SetTile(new Vector3Int(x, y, 0), SelectWaterTile(rocksInWaterDensity));

                else
                {
                    int countAdjWater = CountAdjacentWater(x, y); // Conta tiles adjacentes de água
                    tilemap.SetTile(new Vector3Int(x, y, 0), SelectTile(x, y, countAdjWater)); // Define tiles de grama ou borda

                    // Gera pedra e arbustos em locais que não sejam borda do mapa.
                    if (countAdjWater == 0)
                    {
                        GenerateObjects(x, y, bushesAndRocksDensity, bushesAndRocks);
                        GenerateChests(x, y, chanceToSpawnChest);
                    }


                }
            }
        }
    }

    public void GenerateChests(int x, int y, float chanceToSpawnChest)
    {
        float randomValue = UnityEngine.Random.Range(0f, 1f);

        if (randomValue < chanceToSpawnChest && !grid[x, y].isOccupied)
        {
            GameObject chestPrefab = chests[UnityEngine.Random.Range(0, chests.Length)];
            GameObject chestInstance = Instantiate(chestPrefab, transform);
            chestInstance.transform.position = new Vector3(x, y, 0) + new Vector3(0.5f, 0.5f, 0);

            Chest chestScript = chestInstance.GetComponent<Chest>();

            chestScript.setCanvas(canvas);

            chestScript.SetPowerUpManager(powerUpManager);

            grid[x, y].setIsOccupied(true);
        }
    }


    // Gera ativos como arbustos ou pedras no layer especificado
    public void GenerateObjects(int x, int y, float density, GameObject[] prefabs)
    {
        float randomValue = UnityEngine.Random.Range(0f, 1f);

        if (randomValue < density && !grid[x, y].isOccupied )
        {
            grid[x, y].setIsOccupied(true);

            GameObject prefab = prefabs[UnityEngine.Random.Range(0, prefabs.Length)];
            Instantiate(prefab, transform);
            prefab.transform.position = new Vector3(x, y, 0) + new Vector3(0.5f, 0.5f, 0);

            // Ajusta o BoxCollider2D relativo ao tamanho da árvore
            BoxCollider2D prefabCollider = prefab.GetComponent<BoxCollider2D>();

            Vector2 originalSize = new Vector2(0, 0);

            if (prefabCollider != null)
                originalSize = prefabCollider.size; // Obtém o tamanho original do collider

            // Definindo uma escala aleatória para o tamanho de x e y entre 1 e 5

            float randomScale = GenerateNormal(3f, 2f, 1, 6);
            prefab.transform.localScale = new Vector3(randomScale, randomScale, prefab.transform.localScale.z);


            if (!(prefab.name == "Bush"))
            {
                prefabCollider.size = new Vector2(originalSize.x, originalSize.y);
            }



        }

    }

    // Gera árvores a partir de um parâmetro treeNoiseScale e um parâmetro treeDensity
    void GenerateTrees(float treeNoiseScale, float treeDensity)
    {

        float[,] noiseMap = GenerateNoiseMap(treeNoiseScale);

        for (int x = 0; x != size; x++)
        {
            for (int y = 0; y != size; y++)
            {
                if (!grid[x, y].isOccupied && !grid[x, y].isWater && noiseMap[x, y] < UnityEngine.Random.Range(0f, treeDensity))
                {

                    GameObject prefab = treePrefabs[UnityEngine.Random.Range(0, treePrefabs.Length)];
                    GameObject tree = Instantiate(prefab, transform);

                    // Ajusta o BoxCollider2D relativo ao tamanho da árvore
                    BoxCollider2D collider = tree.GetComponent<BoxCollider2D>();

                    // Ajusta o tamanho do collider baseado na escala da árvore
                    Vector2 originalSize = collider.size; // Obtém o tamanho original do collider

                    // Ajuste da posição para o centro do tile
                    tree.transform.position = new Vector3(x, y, 0) + new Vector3(0.5f, 0.5f, 0);

                    // Utilizada uma normal para gerar altura das árvores
                    float randomScale = GenerateNormal(5f, 2f, 3, 8);
                    tree.transform.localScale = new Vector3(randomScale, randomScale, tree.transform.localScale.z);

                    collider.size = new Vector2(originalSize.x, originalSize.y);

                    // Ajusta o offset do collider para acompanhar o centro do tile
                    collider.offset = new Vector2(0, collider.size.y / 2f);


                    grid[x, y].setIsOccupied(true);
                }
            }
        }
    }

    // Normal truncada através de loop
    public static float GenerateNormal(double mean, double stdDev, float min, float max)
    {   
        float result = 0;

        while(result <= min || result >= max){
            float U1 = UnityEngine.Random.value; 
            float U2 = UnityEngine.Random.value;
            double z0 = Math.Sqrt(-2.0 * Math.Log(U1)) * Math.Cos(2.0 * Math.PI * U2);
            result = (float)(mean + z0 * stdDev);
        }

        return result; 
        
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
    TileBase SelectWaterTile(float rocksOnWaterDensity)
    {
        return UnityEngine.Random.Range(0f, 1f) < rocksOnWaterDensity ? waterTiles[UnityEngine.Random.Range(1, waterTiles.Length)] : waterTiles[0];
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
