using System.Collections.Generic;

using UnityEngine;

public class AudioSourcePlayerContainer
{
    private int _nextUniqueKey = 0;
    private Dictionary<int, AudioSourcePlayer> _playersDict;

    public AudioSourcePlayerContainer()
    {
        _playersDict = new();
    }

    public bool TryGet(int id, out AudioSourcePlayer player)
    {
        if (_playersDict.TryGetValue(id, out player))
            return true;

        return false;
    }

    public int Add(AudioSourcePlayer player)
    {
        _playersDict.Add(_nextUniqueKey, player);

        return _nextUniqueKey++;
    }

    public bool Remove(int key)
    {
        if (_playersDict.ContainsKey(key))
            _playersDict.Remove(key);

        return true;
    }
}
