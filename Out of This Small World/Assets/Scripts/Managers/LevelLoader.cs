// Date   : 22.04.2017 08:44
// Project: Out of This Small World
// Author : bradur

using UnityEngine;
using System.Collections.Generic;
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

public enum ObjectType
{
    None,
    Spawn,
    Door,
    Switch,
    Wormhole,
    Key,
    Axe,
    Flippers,
    PogoStick,
    SwitchWall,
    Tree
}

public enum KeyColor
{
    None,
    Yellow,
    Purple,
    Red,
}

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private World worldPrefab;

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

    [SerializeField]
    private PlayerMovement player;

    [SerializeField]
    private List<GenericWorldObject> genericWorldObjectPrefabs = new List<GenericWorldObject>();

    [SerializeField]
    private List<Level> levels = new List<Level>();

    [SerializeField]
    private int currentLevelIndex = 0;

    private float layerDistance = 0.005f;

    [SerializeField]
    private Sprite tooltipSprite;

    public GenericWorldObject GetWorldObjectPrefab(ObjectType objectType)
    {
        return genericWorldObjectPrefabs[(int)objectType];
    }

    public void RestartLevel()
    {
        Init(levels[currentLevelIndex - 1].file);
    }

    public void LoadNextLevel()
    {
        if (currentLevelIndex < levels.Count)
        {
            Init(levels[currentLevelIndex].file);
            currentLevelIndex += 1;
        } else
        {
            GameManager.main.ShowToolTip(
                "Congratulations! You beat the game! Press Q to quit.",
                tooltipSprite,
                KeyColor.None
            );
            GameManager.main.StartWaitingForExitKey();
        }
    }

    private void Init(TextAsset mapFile)
    {
        foreach(Transform child in worldTransform)
        {
            Destroy(child.gameObject);
        }
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
                tiledMesh.transform.SetParent(world.transform, false);
                tiledMesh.Init(map.Width, map.Height, layer, groundMaterial, world.transform);
            }

            tiledMesh = Instantiate(tiledMeshPrefab);
            tiledMesh.transform.parent = world.transform;
            tempPosition = Vector3.zero;
            tempPosition.z = layerDistance * -index;
            tiledMesh.Init(map.Width, map.Height, layer, groundMaterial, world.transform);
            tiledMesh.transform.position = tempPosition;
            tiledMesh.GetComponent<MeshCollider>().enabled = false;

        }
        for (int index = 0; index < map.ObjectGroups.Count; index += 1)
        {
            for (int oIndex = 0; oIndex < map.ObjectGroups[index].Objects.Count; oIndex += 1)
            {
                TmxObjectGroup.TmxObject tmxObject = map.ObjectGroups[index].Objects[oIndex];
                SpawnObject(tmxObject, world.GetItemContainer().transform, map.Width, map.Height);
            }
        }

        loopLevelLoader.Init(world, map.Width, map.Height);
    }

    private void SpawnObject(TmxObjectGroup.TmxObject tmxObject, Transform parent, int width, int height)
    {
        GenericWorldObject worldObject = Instantiate(genericWorldObjectPrefabs[Tools.IntParseFast(tmxObject.Properties["Type"])]);
        worldObject.Init(tmxObject.Properties);
        worldObject.transform.parent = parent;
        worldObject.Spawn(tmxObject.X, tmxObject.Y, width, height);
    }

    void Start()
    {
        LoadNextLevel();
        GameManager.main.ShowToolTip (
            "Hold " + KeyManager.main.GetKey(Action.Sprint) + " to sprint.\nPress " + KeyManager.main.GetKey(Action.OpenExitMenu) + " to open menu.",
            tooltipSprite,
            KeyColor.None
        );
    }

    void Update()
    {

    }
}

[System.Serializable]
public class Level : System.Object
{
    public TextAsset file;
}
