using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen 
{
    public static MeshData GenTerrain(float[,] heightMap, float multiHeight, AnimationCurve _heightCurve, int levelOfDetail)
    {
        AnimationCurve heightCurve = new AnimationCurve(_heightCurve.keys);

        //figuring out width and height
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        //making our mesh variables centered
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        //mesh incremement for the level detail
        //if the level of detail is = 0 then set the simplification incremement to 1
        //otherwise it'll be set to 2
        int meshSimplificationIncremement = (levelOfDetail == 0)?1: levelOfDetail * 2;
        int verticesPerLine = (width - 1) / meshSimplificationIncremement + 1;

        //creating mesh data veriable and passing in the vertices per line
        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);
        int vertIndex = 0;

        //loop through the height map
        for (int y = 0; y < height; y+= meshSimplificationIncremement)
        {
            for (int x = 0; x < width; x+= meshSimplificationIncremement)
            {   
               meshData.verts[vertIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y]) 
                                                            * multiHeight ,topLeftZ - y);


                meshData.uv[vertIndex] = new Vector2(x / (float)width, 
                                                     y / (float)height);

                //taking on triangles
                if(x < width - 1 && y < height - 1)
                {
                    //adding our triangles
                    meshData.AddTri(vertIndex, vertIndex + verticesPerLine + 1, vertIndex + verticesPerLine);
                    meshData.AddTri(vertIndex + width + 1, vertIndex, vertIndex + 1);

                }

                //setting the vertIndex to 1 to keep track of the 1d array
                vertIndex++;
            }
        }
        return meshData;
    }
}

public class MeshData
{
    //arry of vertices
    public Vector3[] verts;
    //arry of triangles
    public int[] tris;

    public Vector2[] uv;

    int triIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        //initializing vertices array with a new Vector3 with size of
        //mesh width and height
        verts = new Vector3[meshWidth * meshHeight];
        //insitializing triangles array with a size of mesh width - 1 &
        //mesh height - 1 * 6
        tris = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
        //telling each vertex realitive to the map in a % for both x & y axis 
        uv = new Vector2[meshWidth * meshHeight];
    }

    public void AddTri(int a,int b ,int c)
    {
        tris[triIndex] = a;
        tris[triIndex + 1] = b;
        tris[triIndex + 2] = c;
        triIndex += 3;
    }

    public Mesh CreateMesh()
    {
        //creating a new mesh
        Mesh mesh = new Mesh();

        //vertices
        mesh.vertices = verts;

        //triangles
        mesh.triangles = tris;

        //uv
        mesh.uv = uv;

        //lighting
        mesh.RecalculateNormals();

        return mesh;
    }
}
