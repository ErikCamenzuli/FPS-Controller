using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//creating a texture out of 1d color map
public static class TextureGen
{
    public static Texture2D TextureFromMap(Color[] colorMap, int width, int height)
    {
        //setting a new texture to the size of width and height
        Texture2D texture = new Texture2D(width, height);

        //fixing up blurryness
        texture.filterMode = FilterMode.Point;
        //fixing wrapping into blocks
        texture.wrapMode = TextureWrapMode.Clamp;

        //setting the texture to the color map and appling 
        //then returning the texture
        texture.SetPixels(colorMap);
        texture.Apply();
        return texture;
    }

    public static Texture2D TextureFromHeight(float[,] heightMap)
    {
        //figure out the width and height of the noise map
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        //creating a 2d texture
        Texture2D texture = new Texture2D(width, height);

        //setting a color of the pixels in the texture
        Color[] colorMap = new Color[width * height];

        //looping through all of the values in the noise map
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //setting the color map from 1d to 2d
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }
        return TextureFromMap(colorMap, width, height);
    }
}
