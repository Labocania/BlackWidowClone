using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour 
{
    [Tooltip("Prefab for this object pool")]
    public GameObject prefab;

    private List<GameObject> gameObjs = new List<GameObject>();

    [Tooltip("Size of this object pool")]
    public int initialSize = 1;
    [Tooltip("Maximum size of this object pool")]
    public int maximumSize = 1;
    [HideInInspector]
    public int spawnCounter = 0;

    UnityEngine.GameObject organizer;

    void Awake()
    {
        organizer = new UnityEngine.GameObject();
        organizer.transform.position = Vector2.zero;
        organizer.transform.rotation = Quaternion.identity;
        organizer.name = $"{prefab.name} Pool";
    }

    void Start()
    {
        //Instantiate new objects and put them in a list for later use
        for (int i = 0; i < initialSize; i++)
        {
            GenerateObject();
        }
    }

    //Generate a single new object and put it in the list
    private void GenerateObject()
    {
        GameObject newObject = Instantiate(prefab);

        newObject.transform.SetParent(organizer.transform);
        newObject.gameObject.SetActive(false);

        gameObjs.Add(newObject);
    }

    public GameObject GetObject()
    {
        if (spawnCounter > 0)
        {
            spawnCounter--;
        }

        //Try to find an inactive bullet
        for (int i = 0; i < gameObjs.Count; i++)
        {
            GameObject thisObject = gameObjs[i];

            if (!thisObject.gameObject.activeInHierarchy)
            {
                thisObject.gameObject.SetActive(true);
                return thisObject;
            }
        }

        //We are out of objects so we have to instantiate another bullet (if we can)
        if (gameObjs.Count < maximumSize)
        {
            GenerateObject();

            //The new object is last in the list so get it
            GameObject lastObject = gameObjs[gameObjs.Count - 1];

            lastObject.gameObject.SetActive(true);
            return lastObject;
        }

        return null;
    }
}
