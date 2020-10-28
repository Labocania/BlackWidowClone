using RotaryHeart.Lib.SerializableDictionary;

// Base Types for
[System.Serializable]
public class WebDictionary : SerializableDictionaryBase<string, IndexColorMap> { }

[System.Serializable]
public class IndexColorMap
{
    public int[] IntArray;
    public ColorNames color;
}

[System.Serializable]
public class EnemyDictionary : SerializableDictionaryBase<string, int> { }
