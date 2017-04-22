// Date   : 22.04.2017 08:44
// Project: Out of This Small World
// Author : bradur

using UnityEngine;
using TiledSharp;

enum LayerType
{
    None,
    Ground,
    Water,
    Wall,
    Tree
}

public enum Direction
{
    None,
    North,
    East,
    South,
    West,
    NorthEast,
    SouthEast,
    SouthWest,
    NorthWest
}

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private World worldPrefab;

    [SerializeField]
    private WorldBorder worldBorderPrefab;

    [SerializeField]
    private TiledMesh worldColliderPrefab;

    [SerializeField]
    private TiledMesh tiledMeshPrefab;

    [SerializeField]
    private Transform worldTransform;

    [SerializeField]
    private Material groundMaterial;

    [SerializeField]
    private TextAsset debugMap;

    [SerializeField]
    private LoopLevelLoader loopLevelLoader;

    private float layerDistance = 0.005f;

    private void Init(TextAsset mapFile)
    {
        Vector3 tempPosition;
        TmxMap map = new TmxMap(mapFile.text, "unused");
        World world = Instantiate(worldPrefab);
        world.transform.parent = worldTransform;
        world.Init(1, 1, loopLevelLoader);
        for (int index = 0; index < map.Layers.Count; index += 1)
        {
            TmxLayer layer = map.Layers[index];
            LayerType layerType = (LayerType)Tools.IntParseFast(layer.Properties["Type"]);
            TiledMesh tiledMesh;
            if (layerType == LayerType.Ground)
            {
                tiledMesh = Instantiate(worldColliderPrefab);
                WorldCollider worldCollider = tiledMesh.GetComponent<WorldCollider>();
                worldCollider.Init(world);
            } else
            {
                tiledMesh = Instantiate(tiledMeshPrefab);
            }
            
            tiledMesh.transform.parent = world.transform;
            tempPosition = tiledMesh.transform.position;
            tempPosition.z = layerDistance * -index;
            tiledMesh.Init(map.Width, map.Height, layer, groundMaterial);
            tiledMesh.transform.position = tempPosition;
        }

        float borderScale = worldBorderPrefab.transform.localScale.y;
        float borderMultiplier = 4;
        float bigBorder = borderScale * 4;
        InitWorldBorder(world, borderScale * borderMultiplier - bigBorder / 2, map.Height - bigBorder / 2, bigBorder, bigBorder, Direction.NorthWest);
        InitWorldBorder(world, map.Width / 2, map.Height - borderScale * borderMultiplier, map.Width, borderScale * 2, Direction.North);
        InitWorldBorder(world, map.Width - bigBorder / 2, map.Height - bigBorder / 2, bigBorder, bigBorder, Direction.NorthEast);
        InitWorldBorder(world, map.Width - borderScale * borderMultiplier, map.Height / 2, borderScale * 2, map.Height, Direction.East);
        InitWorldBorder(world, map.Width / 2, borderScale * borderMultiplier, map.Width, borderScale * 2, Direction.South);
        InitWorldBorder(world, map.Width - bigBorder / 2, borderScale * borderMultiplier - bigBorder / 2, bigBorder, bigBorder, Direction.SouthEast);
        InitWorldBorder(world, borderScale * borderMultiplier, map.Height / 2, borderScale * 2, map.Height, Direction.West);
        InitWorldBorder(world, borderScale * borderMultiplier - bigBorder / 2, borderScale * borderMultiplier - bigBorder / 2, bigBorder, bigBorder, Direction.SouthWest);

        world.IsCurrentWorld = true;
        loopLevelLoader.Init(world, map.Width, map.Height);
    }

    private void InitWorldBorder(World parent, float xPosition, float yPosition, float xScale, float yScale, Direction direction)
    {
        WorldBorder worldBorder = Instantiate(worldBorderPrefab);
        worldBorder.name = direction.ToString();
        worldBorder.transform.parent = parent.transform;
        worldBorder.transform.position = new Vector3(xPosition, yPosition, 0f);
        worldBorder.transform.localScale = new Vector3(xScale - 2, yScale - 2, worldBorder.transform.localScale.z);
        worldBorder.Init(parent, direction);
    }

    void Start()
    {
        Init(debugMap);
    }

    void Update()
    {

    }
}
