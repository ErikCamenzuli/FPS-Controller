using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRend;
    public MeshFilter meshFilter;
    public MeshRenderer meshRend;

    //1d noise map
    public void DrawTexture(Texture2D texture)
    {
        //appling the texture to the textureRend and being able to generate inside Unity
        textureRend.sharedMaterial.mainTexture = texture;
        //setting the size of the plane to the map
        textureRend.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        //sharing the mesh since it's being generated outside of game mode
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRend.sharedMaterial.mainTexture = texture;
    }
}
