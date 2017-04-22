// Date   : 22.04.2017 08:44
// Project: Out of This Small World
// Author : bradur

using UnityEngine;
using TiledSharp;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private TiledMesh tiledMeshPrefab;

    [SerializeField]
    private Transform worldTransform;

    [SerializeField]
    private Material groundMaterial;

    [SerializeField]
    private TextAsset debugMap;

    private float layerDistance = 0.005f;

    private void Init (TextAsset mapFile)
    {
        Vector3 tempPosition;
        TmxMap map = new TmxMap(mapFile.text, "unused");
        for (int index = 0; index < map.Layers.Count; index += 1)
        {
            
            TmxLayer layer = map.Layers[index];
            TiledMesh tiledMesh = Instantiate(tiledMeshPrefab);
            tiledMesh.transform.parent = worldTransform;
            tempPosition = tiledMesh.transform.position;
            tempPosition.z = layerDistance * -index;
            tiledMesh.Init(map.Width, map.Height, layer, groundMaterial);
            tiledMesh.transform.position = tempPosition;
        }
    }

    void Start()
    {
        Init(debugMap);
    }

    void Update()
    {

    }
}
