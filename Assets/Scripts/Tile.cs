public enum HeightType
{
    DeepWater = 1,
    ShallowWater = 2,
    Shore = 3,
    Sand = 4,
    Grass = 5,
    Forest = 6,
    Rock = 7,
    Snow = 8
}

public class Tile
{
    public HeightType HeightType;
    public float HeightValue { get; set; }
    public int X, Y;
    public int Bitmask;

    public Tile Left;
    public Tile Right;
    public Tile Top;
    public Tile Bottom;

    public bool Collidable;
    public bool FloodFilled;

    public Tile()
    {
    }

    public void UpdateBitmask()
    {
        int count = 0;

        if (Top.HeightType == HeightType)
            count += 1;
        if (Right.HeightType == HeightType)
            count += 2;
        if (Bottom.HeightType == HeightType)
            count += 4;
        if (Left.HeightType == HeightType)
            count += 8;

        Bitmask = count;
    }
}
