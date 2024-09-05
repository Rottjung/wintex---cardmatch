using RotaryHeart.Lib.SerializableDictionary;
using System;
using UnityEngine;

[Serializable]
public class Dictionary_Int_CardsDBData : SerializableDictionaryBase<int, CardsDBData> { };

[Serializable]
public class CardsDBData : DBData_Base<int>
{
    public Sprite[] sprites = { };
}

[Serializable]
[CreateAssetMenu(fileName = "CardsDB", menuName = "ScriptableObjects/CardsDB", order = 1)]
public class DB_Cards : DB_Base<int, CardsDBData, Dictionary_Int_CardsDBData>
{
    
}
