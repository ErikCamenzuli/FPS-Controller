using System.Collections;
using UnityEngine;

public static class Noise
{
    //method for generating a noise map and returning a grid of values between 0 and 1
    public static float[,] generateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {

        System.Random RandNumBGen = new System.Random(seed);
        //creating an array of offsets of octaves
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = RandNumBGen.Next(-100000, 100000) + offset.x;
            float offsetY = RandNumBGen.Next(-100000, 100000) + offset.y;

            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        //doing a check to see if scale is 0
        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        //zooming into centre of map instead of top right
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        //2d array
        float[,] noiseMap = new float[mapWidth, mapHeight];

        //looping through the noise map
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    //figuring out & choosing sample values
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    float perlinVal = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    //increasing the noise height by the perlin value
                    noiseHeight += perlinVal * amplitude;

                    //amplitude will decrease each ocatuive
                    amplitude *= persistance;
                    //frequency will increase each ocatuive
                    frequency *= lacunarity;
                }
                //checking to see if noise height is greater than the max noise height
                if (noiseHeight > maxNoiseHeight)
                {
                    //set the max noise height to the noise height
                    maxNoiseHeight = noiseHeight;
                }
                //otherwise check if the noise height is less than the minium noise height
                else if (noiseHeight < minNoiseHeight)
                {
                    //if so then set the minium noise height to the noise height
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }

        //normalizing the noise map by looping through x and y values
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                //returning a value between 0 and 1
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }

}
