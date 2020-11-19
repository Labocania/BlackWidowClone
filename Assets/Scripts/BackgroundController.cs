using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    List<GameObject> childObjects;
    WaitForSeconds animationInterval;
    WaveData waveData;

    void Awake()
    {
        childObjects = gameObject.GetChildren();
        animationInterval = new WaitForSeconds(1.7f);
        waveData = DataReader.instance.LoadWaveData();
        EventList.waveChanged += Background_OnWaveChanged;
    }

    private void Background_OnWaveChanged()
    {
        DataReader.instance.NextWave();
        waveData = DataReader.instance.LoadWaveData();
        ClearWebColliders(waveData.webColliders);
        StartCoroutine(WaveChangeRoutine());
    }

    void Start()
    {
        StartCoroutine(WaveChangeRoutine());
    }

    IEnumerator WaveChangeRoutine()
    {
        yield return BackgroundAnimation();
        PlaceWebColliders(waveData.webColliders);
        EventList.waveStarted.Invoke();
    }

    IEnumerator BackgroundAnimation()
    {
        foreach (GameObject obj in childObjects)
        {
            StartCoroutine(obj.GetComponent<ColorSwapper>().StartBackgroundAnimation());
        }

        yield return animationInterval;

        foreach (GameObject obj in childObjects)
        {
            obj.GetComponent<ColorSwapper>().ToggleObjectsColor(ColorNames.Blue);
        }
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
