using System;
using System.Collections.Generic;

/// <summary>
/// 인스펙터에서 dictianary를 사용할 수 있도록 만들어진 스크립트
/// </summary>

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
