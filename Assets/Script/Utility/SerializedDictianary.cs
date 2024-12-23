using System;
using System.Collections.Generic;

[Serializable]
public class SerializedDictianary<TKey, TValue>
{
    public List<TKey> keys = new List<TKey>();
    public List<TValue> values = new List<TValue>();

    public Dictionary<TKey, TValue> ToDictionary()
    {
        Dictionary<TKey,TValue> retDic = new Dictionary<TKey, TValue>();
        for (int i = 0; i < keys.Count; ++i)
        {
            if (!retDic.ContainsKey(keys[i]))
            {
                retDic.Add(keys[i], values[i]);
            }
        }
        return retDic;
    }



}
