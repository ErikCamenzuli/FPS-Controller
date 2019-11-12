using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    public const float maxVeiwRange = 450;
    public Transform view;

    static MapGen mapGen;

    public Material mapMaterial;

    //preventing duplications
    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> terrainChunksVisibleLast = new List<TerrainChunk>();

    public static Vector2 viewerPosition;
    int chunkSize;
    int visibleChunks;

    void Start()
    {
        mapGen = FindObjectOfType<MapGen>();
        chunkSize = MapGen.mapChunkSize - 1;
        visibleChunks = Mathf.RoundToInt(maxVeiwRange / chunkSize); 
    }

    void Update()
    {
        viewerPosition = new Vector2(view.position.x, view.position.z);
        UpdateVisibleChunks();
    }

    void UpdateVisibleChunks()
    {

        for (int i = 0; i < terrainChunksVisibleLast.Count; i++)
        {
            terrainChunksVisibleLast[i].SetVisible (false);
        }
        terrainChunksVisibleLast.Clear();

        //getting the coordinate that the chunk the viewer is on
        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / visibleChunks);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / visibleChunks);

        //looping through all chunks around/in view distance
        for (int yOffSet = -visibleChunks; yOffSet <= visibleChunks; yOffSet++)
        {
            for (int xOffSet = -visibleChunks; xOffSet <= visibleChunks; xOffSet++)
            {
                Vector2 viewedChuck = new Vector2(currentChunkCoordX + xOffSet, currentChunkCoordY + yOffSet);

                //instanciate new chunk
                if(terrainChunkDictionary.ContainsKey(viewedChuck))
                {
                    //updating the chunk
                    terrainChunkDictionary[viewedChuck].UpdateTerrain();
                    if(terrainChunkDictionary[viewedChuck].IsVisible())
                    {
                        terrainChunksVisibleLast.Add(terrainChunkDictionary[viewedChuck]);
                    }
                }
                //if it doesn't contain that key then instanciate new chunk
                else
                {
                    terrainChunkDictionary.Add(viewedChuck, new TerrainChunk(viewedChuck, chunkSize, transform, mapMaterial));
                }
            }
        }
    }

    public class TerrainChunk
    {
        GameObject meshObject;
        Vector2 position;
        Bounds bounds;

        MeshRenderer meshRend;
        MeshFilter meshFilter;


        public TerrainChunk(Vector2 coord, int size, Transform parent, Material material)
        {
            //setting position to coord of the size
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            //positioning into 3d space
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);

            //instansiate a chunk object 
            meshObject = new GameObject("Terrain Chunk");
            meshRend = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();

            //transforming the position into the Vector3 position
            meshObject.transform.position = positionV3;

            //setting the scale to it's correct scale 
            meshObject.transform.parent = parent;
            meshRend.material = material;

            SetVisible(false);

            mapGen.ReqMapData(ReceivedMapData);
        }

        void ReceivedMapData(MapGen.MapData mapData)
        {
            //calling the mesh data
            mapGen.ReqMeshData(mapData, ReceievedMeshData);
        }

        void ReceievedMeshData(MeshData meshData)
        {
            meshFilter.mesh = meshData.CreateMesh();
        }

        //telling the terrain chunk to update itself
        //by finding a point that is closest to the viewers position
        public void UpdateTerrain()
        {
            //returning the square distance between given point & bounding spot 
            float viewDistanceFromEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            bool visible = viewDistanceFromEdge <= maxVeiwRange;
            SetVisible(visible);
        }

        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }

        //checking to see if mesh is visible
        public bool IsVisible()
        {
            return meshObject.activeSelf;
        }
    }
}
