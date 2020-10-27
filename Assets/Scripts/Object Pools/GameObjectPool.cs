using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [Tooltip("Prefab for this object pool")]
    public T prefab;

    private List<T> gameObjs = new List<T>();

    [Tooltip("Size of this object pool")]
    public int initialSize = 2;
    [Tooltip("Maximum size of this object pool")]
    public int maximumSize = 2;

    //GameObject organizer;

    private void Awake()
    {
        //organizer = new GameObject();
        //organizer.transform.position = Vector2.zero;
        //organizer.transform.rotation = Quaternion.identity;
        //organizer.name = $"{prefab.name} Pool";

        if (prefab == null)
        {
            Debug.LogError("Need a reference to the object prefab");
        }


        //Instantiate new objects and put them in a list for later use
        for (int i = 0; i < initialSize; i++)
        {
            GenerateObject();
        }
    }

    //Generate a single new object and put it in the list
    private void GenerateObject()
    {
        T newObject = Instantiate(prefab);

        //newObject.transform.SetParent(organizer.transform);
        newObject.gameObject.SetActive(false);

        gameObjs.Add(newObject);
    }

    public T GetObject()
    {
        //Try to find an inactive bullet
        for (int i = 0; i < gameObjs.Count; i++)
        {
            T thisObject = gameObjs[i];

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
            T lastObject = gameObjs[gameObjs.Count - 1];

            lastObject.gameObject.SetActive(true);
            return lastObject;
        }

        return null;
    }
}
