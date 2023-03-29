using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static CityMgmtUtils;

public class AnimationMapper : MonoBehaviour
//TODO
//make meeple sprite wider, 16->18
//make code work for multiple frames
{
    int colorizedSpriteWidth;
    int colorizedSpriteHeight;
    int mapWidth;
    int mapHeight;
    int frames;
    [SerializeField] 
    Texture2D mapSprite;
    [SerializeField]
    Texture2D colorizedSprite;
    [SerializeField]
    Texture2D[] spriteComponents; 
    List<Dictionary<(int, int), (int, int)>> animMap = new List<Dictionary<(int, int), (int, int)>>();
    // maps a pixel in the frame img to a pixel in the ref img

    void Start() {
        //mapSprite = GetComponent<SpriteRenderer>().sprite.texture;
        //mapWidth = (int)GetComponent<SpriteRenderer>().sprite.rect.width;
        //mapHeight = (int)GetComponent<SpriteRenderer>().sprite.rect.height;
        mapWidth = mapSprite.width;
        mapHeight = mapSprite.height;
        //colorizedSprite = GameObject.Find("ColorizedSprite").GetComponent<SpriteRenderer>().sprite.texture;
        //colorizedSpriteWidth = (int)GameObject.Find("ColorizedSprite").GetComponent<SpriteRenderer>().sprite.rect.width;
        //colorizedSpriteHeight = (int)GameObject.Find("ColorizedSprite").GetComponent<SpriteRenderer>().sprite.rect.height;
        colorizedSpriteWidth = colorizedSprite.width;
        colorizedSpriteHeight = colorizedSprite.height;
        frames = (mapWidth / colorizedSpriteWidth) - 1;
        GenerateAnimMap();
        /*print("aniMap irregularities");
        foreach(Dictionary<(int, int), (int, int)> dict in animMap) {
            foreach((int, int) key in dict.Keys) {
                if(key != dict[key]) {
                    print(key);
                    print(dict[key]);
                }
            }
        }*/
        GenerateAnimFromMap();
    }

    void GenerateAnimMap() {
        Dictionary<(int, int), (int, int)> emptyOffsetDict = new Dictionary<(int, int), (int, int)>();
        animMap.Add(emptyOffsetDict);
        //for the "frame 0" ref dict
        Color32[] mapSpritePixels = mapSprite.GetPixels32();
        //aseprite coords are left->right top->down, GetPixels32() coords are left->right bottom->top
        for (int curFrame = 1; curFrame <= frames; curFrame++) {
            Dictionary<(int, int), (int, int)> newDict = new Dictionary<(int, int), (int, int)>();
            for (int y = 0; y < colorizedSpriteHeight; y++) {
                for (int x = curFrame*colorizedSpriteWidth; x < curFrame*colorizedSpriteWidth + colorizedSpriteWidth; x++) {
                    /*if (curFrame == 2) {
                        print(x + ", " + y);
                    }*/
                    newDict.Add((x, y), FindMatchingPixelRef(mapSpritePixels, (x, y)));
                }
            }
            animMap.Add(newDict);
        }
    }

    void GenerateAnimFromMap() {
        Color32[] colorizedSpritePixels = colorizedSprite.GetPixels32();
        Color32[] newTexturePixels = new Color32[(mapWidth - colorizedSpriteWidth) * colorizedSpriteHeight];
        for (int curFrame = 1; curFrame <= frames; curFrame++) {
        //frame 0 is the reference image in the map
            for (int y = 0; y < colorizedSpriteHeight; y++) {
            // 0 <= y < 3
                for (int x = curFrame*colorizedSpriteWidth; x < curFrame*colorizedSpriteWidth + colorizedSpriteWidth; x++) {
                // 3 <= x < 6
                    newTexturePixels[x - colorizedSpriteWidth + y * (mapWidth - colorizedSpriteWidth)] = colorizedSpritePixels[animMap[curFrame][(x, y)].Item1 + animMap[curFrame][(x, y)].Item2 * colorizedSpriteWidth];
                }
            }
        }
        Texture2D newTexture = new Texture2D((mapWidth - colorizedSpriteWidth), colorizedSpriteHeight);
        newTexture.SetPixels32(newTexturePixels);
        File.WriteAllBytes(Application.persistentDataPath + "/MeepleAnimTest.png", newTexture.EncodeToPNG());
    }

    (int, int) FindMatchingPixelRef(Color32[] pixels, (int, int) coords) {
        //find the coordinates of the pixel in the first frame size of pixels that matches the color of the pixel at pixels[coords]
        //pixels is the map sprite, coords is the absolute coordinates of a pixel
        for (int y = 0; y < colorizedSpriteHeight; y++) {
            for (int x = 0; x < colorizedSpriteWidth; x++) {
                if (CityMgmtUtils.Color32IsEqual(pixels[x + y * mapWidth], pixels[coords.Item1 + coords.Item2 * mapWidth])) {
                    return (x, y);
                }
            }
        }
        //Debug.Log("No matching pixel found in call to FindMatchingPixelRef()");
        return (0, 0);
    }


}
