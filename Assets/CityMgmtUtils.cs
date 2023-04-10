using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityMgmtUtils
{
    public static bool Color32IsEqual(Color32 color1, Color32 color2) {
        return (color1.r == color2.r && color1.g == color2.g && color1.b == color2.b && color1.a == color2.a);
    }

    public static Color32[] FlattenedColor32ReverseY(Color32[] data, int width, int height) {
        for (int i = 0; i < data.Length / width; i++) {
            Color32[] temp = new Color32[width];
            for (int j = 0; j < width; j++) {
                temp[i] = data[i * width + j];
            }
            Array.Reverse(temp);
            for (int j = 0; j < width; j++) {
                data[i * width + j] = temp[i * width + j];
            }
        }
        return data;
    }

    public IEnumerator Wait(float sec){
        yield return new WaitForSeconds(sec);
    }
}
