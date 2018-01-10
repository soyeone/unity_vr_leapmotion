using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour
{
    public Terrain terrain;

    /// <summary>
    /// map width
    /// </summary>
    /// <returns></returns>
    public int GetWidth()
    {
        return (int)terrain.terrainData.size.x;
    }


    /// <summary>
    /// map height
    /// </summary>
    /// <returns></returns>
    public int GetHeight()
    {
        return (int)terrain.terrainData.size.z;
    }

    /// <summary>
    /// retun all info about specified position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public MapData GetInfo(Vector2 position)
    {
        float height = terrain.SampleHeight(new Vector3((int)position.x, 0, (int)position.y)) + terrain.GetPosition().y ;
        int angle = (int)terrain.terrainData.GetSteepness(position.x / terrain.terrainData.size.x, position.y / terrain.terrainData.size.z);
        return new MapData(height, angle, TextureDetails(position));
    }

   /// <summary>
   ///return texture details
   /// </summary>
   /// <param name="position"></param>
   /// <returns></returns>
    private float[] TextureDetails(Vector2 position)
    {
        if (position.x >= GetWidth() || position.x < 0 || position.y >= GetHeight() || position.y < 0)
        {
            Debug.Log("out");
            return null;
        }
        int mapX = (int)(((position.x - terrain.transform.position.x) / terrain.terrainData.size.x) * terrain.terrainData.alphamapWidth);
        int mapZ = (int)(((position.y - terrain.transform.position.z) / terrain.terrainData.size.z) * terrain.terrainData.alphamapHeight);
        float [, ,] TerrCntrl = terrain.terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
        // This can get a grid. Since we are only getting 1, we have a 1x1 array
        // The 3rd is the 0-1 weight of that texture (if you have 4 textures, the 3rd
        // has size 4. TerrCntrl[0,0,0] is the weigth of texture#0.)
        // TC[0,0, 0-??] add to 1
        //Debug.Log(TerrCntrl.ToString());
       // string data = "1st -" + TerrCntrl [0, 0, 0].ToString() + " 2nd -" + TerrCntrl [0, 0, 1].ToString() + " 3rd -" + TerrCntrl [0, 0, 2].ToString();
        float [] cellMix = new float [TerrCntrl.GetUpperBound(2) + 1];
        string data = "";
        for (int n = 0; n < cellMix.Length; ++n)
        {
            cellMix[n] = TerrCntrl[0, 0, n];
            data += n.ToString() + ":" + TerrCntrl[0, 0, n].ToString() + " ";
        }
        return cellMix;
    }
}
