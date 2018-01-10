using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapData  {

    public float height; 
    public int angle;//sloap  0-90
    public float[] textureData; // array af numbers 0-1, sum of these numbers is 1,  each  value represent how many percent of X.th texture is on position 1=100%

    public MapData(float height, int angle, float[] textureData)
    {
        this.height = height;
        this.angle = angle;
        this.textureData = textureData;
    }
}
