using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

public abstract class DBData_Base<K>
{
    [HideInInspector]
    public K Key;
};

public abstract class DB_Base<K, V, Dict> : ScriptableObject where V : DBData_Base<K> where Dict : SerializableDictionaryBase<K, V>
{
    [SerializeField]
    public Dict dictionary;
}
