using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    List<GameObject> childObjects;
    public WaveData waveData;

    void Awake()
    {
        childObjects = gameObject.GetChildren();
    }

    IEnumerator Start()
    {
        foreach (GameObject obj in childObjects)
        {
           StartCoroutine(obj.GetComponent<ColorSwapper>().StartBackgroundAnimation());
        }

        yield return new WaitForSeconds(1.7f);

        foreach (GameObject obj in childObjects)
        {
            obj.GetComponent<ColorSwapper>().ToggleObjectsColor(ColorNames.Blue);
        }

        PlaceWebColliders(waveData.webColliders);

        EventBroker.TriggerEvent("AnimationFinished");
    }

    public void PlaceWebColliders(WebDictionary webDictionary)
    {
        foreach (KeyValuePair<string, IndexColorMap> web in webDictionary)
        {
            childObjects.Find(x => x.name == web.Key).GetComponent<ColorSwapper>().DrawWebColliders(web.Value.IntArray, web.Value.color);
        }
    }

    public void ClearWebColliders(WebDictionary webDictionary)
    {
        foreach (KeyValuePair<string, IndexColorMap> web in webDictionary)
        {
            childObjects.Find(x => x.name == web.Key).GetComponent<ColorSwapper>().EraseWebColliders(web.Value.IntArray);
        }
    }
}
