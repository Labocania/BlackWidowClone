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

public enum Enemies
{
    Mosquito, Beetle, Hornet, Egg, Steak, Spoiler, Grenade, Rocket, Thunderbug,
    Slayer
}

[System.Serializable]
public class EnemyDictionary : SerializableDictionaryBase<Enemies, int> { }
