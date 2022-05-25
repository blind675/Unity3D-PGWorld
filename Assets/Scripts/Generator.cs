using UnityEngine;
using AccidentalNoise;

public class Generator : MonoBehaviour
{
    // Adjustable variables for Unity Inspector
    [SerializeField]
    int Width = 256;
    [SerializeField]
    int Height = 256;
    [SerializeField]
    int TerrainOctaves = 6;
    [SerializeField]
    double TerrainFrequency = 1.25;

    [SerializeField]
    float DeepWater = 0.1f;
    [SerializeField]
    float ShallowWater = 0.2f;
    [SerializeField]
    float Sand = 0.3f;
    [SerializeField]
    float Grass = 0.5f;
    [SerializeField]
    float Forest = 0.8f;
    [SerializeField]
    float Rock = 0.9f;

    // Noise generator module
    ImplicitFractal HeightMap;

    // Height map data
    MapData HeightData;

    // Final Objects
    Tile[,] Tiles;

    // Our texture output (unity component)
    public GameObject plane;
    MeshRenderer HeightMapRenderer;

    void Start()
    {
        // Get the mesh we are rendering our output to
        HeightMapRenderer = plane.GetComponent<MeshRenderer>();

        // Initialize the generator
        Initialize();

        // Build the height map
        GetData(HeightMap, ref HeightData);

        // Build our final objects based on our data
        LoadTiles();

        // Render a texture representation of our map
        HeightMapRenderer.materials[0].mainTexture = TextureGenerator.GetTexture(Width, Height, Tiles);
    }

    private void Initialize()
    {
        // Initialize the HeightMap Generator
        HeightMap = new ImplicitFractal(FractalType.MULTI,
                                       BasisType.SIMPLEX,
                                       InterpolationType.QUINTIC,
                                       TerrainOctaves,
                                       TerrainFrequency,
                                       UnityEngine.Random.Range(0, int.MaxValue));
    }

    // Extract data from a noise module
    private void GetData(ImplicitModuleBase module, ref MapData mapData)
    {
        mapData = new MapData(Width, Height);

        // loop through each x,y point - get height value
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {

                // Noise range
                float x1 = 0, x2 = 2;
                float y1 = 0, y2 = 2;
                float dx = x2 - x1;
                float dy = y2 - y1;

                // Sample noise at smaller intervals
                float s = x / (float)Width;
                float t = y / (float)Height;

                // Calculate our 4D coordinates
                float nx = x1 + Mathf.Cos(s * 2 * Mathf.PI) * dx / (2 * Mathf.PI);
                float ny = y1 + Mathf.Cos(t * 2 * Mathf.PI) * dy / (2 * Mathf.PI);
                float nz = x1 + Mathf.Sin(s * 2 * Mathf.PI) * dx / (2 * Mathf.PI);
                float nw = y1 + Mathf.Sin(t * 2 * Mathf.PI) * dy / (2 * Mathf.PI);

                float heightValue = (float)HeightMap.Get(nx, ny, nz, nw);

                // keep track of the max and min values found
                if (heightValue > mapData.Max) mapData.Max = heightValue;
                if (heightValue < mapData.Min) mapData.Min = heightValue;

                mapData.Data[x, y] = heightValue;
            }
        }
    }

    // Build a Tile array from our data
    private void LoadTiles()
    {
        Tiles = new Tile[Width, Height];

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                Tile t = new Tile();
                t.X = x;
                t.Y = y;

                float value = HeightData.Data[x, y];

                //normalize our value between 0 and 1
                value = (value - HeightData.Min) / (HeightData.Max - HeightData.Min);

                t.HeightValue = value;

                //HeightMap Analyze
                if (value < DeepWater)
                {
                    t.HeightType = HeightType.DeepWater;
                }
                else if (value < ShallowWater)
                {
                    t.HeightType = HeightType.ShallowWater;
                }
                else if (value < Sand)
                {
                    t.HeightType = HeightType.Sand;
                }
                else if (value < Grass)
                {
                    t.HeightType = HeightType.Grass;
                }
                else if (value < Forest)
                {
                    t.HeightType = HeightType.Forest;
                }
                else if (value < Rock)
                {
                    t.HeightType = HeightType.Rock;
                }
                else
                {
                    t.HeightType = HeightType.Snow;
                }

                Tiles[x, y] = t;
            }
        }
    }

}