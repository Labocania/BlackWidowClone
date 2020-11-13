using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour 
{
    [Tooltip("Prefab for this object pool")]
    public GameObject prefab;
    public List<GameObject> GameObjs { get; private set; } = new List<GameObject>();

    [Tooltip("Size of this object pool")]
    public int initialSize = 1;
    [Tooltip("Maximum size of this object pool")]
    public int maximumSize;
    [HideInInspector]
    public int spawnCounter = 0;

    UnityEngine.GameObject organizer;

    void Awake()
    {
        organizer = new UnityEngine.GameObject();
        organizer.transform.position = Vector2.zero;
        organizer.transform.rotation = Quaternion.identity;
    }

    void Start()
    {
        organizer.name = $"{prefab.name} Pool";

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

        GameObjs.Add(newObject);
    }

    public GameObject GetObject()
    {
        if (spawnCounter > 0)
        {
            spawnCounter--;
        }

        //Try to find an inactive bullet
        for (int i = 0; i < GameObjs.Count; i++)
        {
            GameObject thisObject = GameObjs[i];

            if (!thisObject.gameObject.activeInHierarchy)
            {
                thisObject.gameObject.SetActive(true);
                return thisObject;
            }
        }

        //We are out of objects so we have to instantiate another bullet (if we can)
        if (GameObjs.Count < maximumSize)
        {
            GenerateObject();

            //The new object is last in the list so get it
            GameObject lastObject = GameObjs[GameObjs.Count - 1];

            lastObject.gameObject.SetActive(true);
            return lastObject;
        }

        return null;
    }
}
