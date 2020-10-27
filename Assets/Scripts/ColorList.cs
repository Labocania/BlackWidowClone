using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorNames
{
    Blue, Green, LightBlue, Red, Pink, Yellow, White
}

public static class ColorList
{
    /*
    public static readonly Color blue = new Color(13f, 0f, 214f);
    public static readonly Color green = new Color(0f, 218f, 15f);
    public static readonly Color lightBlue = new Color(13f, 247f, 255f);
    public static readonly Color red = new Color(219f, 7f, 0f);
    public static readonly Color pink = new Color(222f, 8f, 202f);
    public static readonly Color yellow = new Color(212f, 226f, 3f);
    public static readonly Color white = Color.white;
    public static readonly Color red = new Color(219f, 7f, 0f);
    */

    public static List<Color32> colors = new List<Color32>()
    {
        new Color32(13, 0, 214, 255),
        new Color32(0, 218, 15, 255),
        new Color32(13, 247, 255, 255),
        new Color32(219, 7, 0, 255),
        new Color32(222, 8, 202, 255),
        new Color32(212, 226, 3, 255),
        Color.white,
        new Color32(219, 7, 0, 255)
    };

    static WaitForSeconds flashAnimationInterval = new WaitForSeconds(0.08f);

    public static Color RandomColor()
    {
        return colors[Random.Range(0, 8)];
    }

    public static IEnumerator FlashSprite(SpriteRenderer sprite)
    {
        while (true)
        {
            Color colorCache = sprite.color;

            sprite.color = Color.white;

            yield return flashAnimationInterval;

            sprite.color = colorCache;

            yield return flashAnimationInterval;
        }
    }
}
