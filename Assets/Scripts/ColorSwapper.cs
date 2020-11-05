using System.Collections;
using UnityEngine;

public class ColorSwapper : MonoBehaviour
{
    public int startingColorIndex = 0;
    SpriteRenderer[] sprites;
    WaitForSeconds webAnimationInterval = new WaitForSeconds(0.1f);

    void Awake()
    {
        sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
    }

    public IEnumerator StartBackgroundAnimation()
    {
        yield return IterateColors();
        EventBroker.TriggerEvent("AnimationFinished");
    }

    public IEnumerator StartBackgroundAnimations()
    {
        yield return IterateColors();
        yield return WebColorRotation();
        EventBroker.TriggerEvent("AnimationFinished");
    }

    public IEnumerator IterateColors()
    {
        int colorIndex = startingColorIndex;

        for (int i = 0; i < ColorList.colors.Count * 2; i++, colorIndex++)
        {
            for (int k = 0; k < sprites.Length; k++)
            {
                sprites[k].color = ColorList.colors[colorIndex % ColorList.colors.Count];
            }
            yield return webAnimationInterval;
        }
    }

    public IEnumerator WebColorRotation()
    {
        int colorIndex = 0;

        for (int i = 0; i < ColorList.colors.Count; i++)
        {
            for (int k = 0; k < sprites.Length; k++, colorIndex++)
            {
                sprites[k].color = ColorList.colors[colorIndex % ColorList.colors.Count];
            }
            yield return webAnimationInterval;
        }
    }

    public IEnumerator IterateUIColor(float speed)
    {
        foreach (Color color in ColorList.colors)
        {
            // UI element color change
        }

        yield return null;
    }

    public void ToggleObjectsColor(ColorNames color)
    {
        foreach (var sprite in sprites)
        {
            sprite.color = ColorList.colors[(int)color];
        }
    }

    public void DrawWebColliders(int[] spriteIndexes, ColorNames name)
    {
        for (int i = 0; i < spriteIndexes.Length; i++)
        {
            sprites[spriteIndexes[i]].color = ColorList.colors[(int)name];

            if (name == ColorNames.Red)
            {
                sprites[spriteIndexes[i]].gameObject.AddComponent<PolygonCollider2D>();
            }
        }
    }

    public void EraseWebColliders(int[] spriteIndexes)
    {
        for (int i = 0; i < spriteIndexes.Length; i++)
        {
            if (sprites[spriteIndexes[i]].color == ColorList.colors[(int)ColorNames.Red])
            {
                PolygonCollider2D poly = sprites[i].gameObject.GetComponent<PolygonCollider2D>();
                Destroy(poly);
            }

            sprites[spriteIndexes[i]].color = ColorList.colors[(int)ColorNames.Blue];
        }
    }

}
