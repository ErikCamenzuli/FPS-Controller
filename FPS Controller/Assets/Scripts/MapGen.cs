using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

//having some values that will define our map
public class MapGen : MonoBehaviour
{
    //deciding which thing to draw
    //either Noise or Color map
    public enum DrawMode
    {
        NoiseMap,
        ColorMap,
        Mesh
    };


    [System.Serializable]
    public struct TerrainTypes
    {
        public string terrainName;
        public float height;
        public Color color;
    }

    #region Variables
    public const int mapChunkSize = 241;

    private int levelOfDetail;
    public int octaves;
    //slider
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float noiseScale;
    public float meshHeightMultipler;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public TerrainTypes[] regions;
    public DrawMode drawMode;

    Queue<MapThreadInfo<MapData>> mapThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    Queue<MapThreadInfo<MeshData>> meshThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();
    #endregion

    public void DrawingInEditor()
    {
        MapData mapData = GenMapData();

        //referenceing MapDisplay & TextureGen for noise and color with width & height
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if(drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGen.TextureFromHeight(mapData.heightMap));
        }
        else if (drawMode == DrawMode.ColorMap)
        {
            display.DrawTexture(TextureGen.TextureFromMap(mapData.colorMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGen.GenTerrain(mapData.heightMap, meshHeightMultipler, meshHeightCurve, levelOfDetail), 
                    TextureGen.TextureFromMap(mapData.colorMap, mapChunkSize, mapChunkSize));
        }

    }
    
    public void ReqMapData(Action<MapData> callBack)
    {
        //creating a thread start which is going to show the MapDataThread
        ThreadStart threadStart = delegate
        {
            MapDataThread(callBack);
        };

        new Thread(threadStart).Start();
    }

    public void ReqMeshData(MapData mapData, Action<MeshData> callBack)
    {

    }

    void MeshDataThread(MapData mapData, Action<MeshData> callBack)
    {
        MeshData meshData = MeshGen.GenTerrain(mapData.heightMap, meshHeightMultipler, meshHeightCurve, levelOfDetail);
        lock(meshThreadInfoQueue)
        {
            meshThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callBack, meshData));
        }
    }

    void MapDataThread(Action<MapData> callBack)
    {
        MapData mapData = GenMapData();
        //making sure that our thread isn't accessed from other places
        //so no other thread can execute 
        lock (mapThreadInfoQueue)
        {
            //adding a new map thread
            mapThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callBack, mapData));
        }
    }

    void Update()
    {
        //if the map thread info queue has something in it (greater than 1)
        if (mapThreadInfoQueue.Count > 0)
        {
            //loop through the elements
            for (int i = 0; i < mapThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MapData> threadInfo = mapThreadInfoQueue.Dequeue();
                threadInfo.callBack(threadInfo.parameter);
            }
        }
        //if the mesh thread info queue has something in it (greater than 1)
        if(meshThreadInfoQueue.Count > 0)
        {
            //loop through the elements
            for (int i = 0; i < meshThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MeshData> threadInfo = meshThreadInfoQueue.Dequeue();
                threadInfo.callBack(threadInfo.parameter);
            }
        }
    }


    MapData GenMapData()
    {
        //fetching the 2d noise map to be able to draw to screen from the "MapDisplay" class
        float[,] noiseMap = Noise.generateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves,
                                                   persistance, lacunarity, offset);
        //1d color map
        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];

        //loop through the noise map
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                //looping through all the regions
                for (int i = 0; i < regions.Length; i++)
                {
                    //checking to see if the current height is less than the regions height
                    if (currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapChunkSize + x] = regions[i].color;
                        //Found the region!
                        break;
                    }
                }
            }
        }
        return new MapData(noiseMap, colorMap);
    }

    struct MapThreadInfo<T>
    {
        public readonly Action<T> callBack;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callBack, T parameter)
        {
            this.callBack = callBack;
            this.parameter = parameter;
        }
    }

    //called automatically if a variable is changed
    void OnValidate()
    {
        if(lacunarity < 1)
        {
            lacunarity = 1;
        }
        if(octaves < 0)
        {
            octaves = 0;
        }
    }

    public struct MapData
    {
        public readonly float[,] heightMap;
        public readonly Color[] colorMap;

        public MapData (float[,] heightMap, Color[] colorMap)
        {
            this.heightMap = heightMap;
            this.colorMap = colorMap;
        }
    }

}
