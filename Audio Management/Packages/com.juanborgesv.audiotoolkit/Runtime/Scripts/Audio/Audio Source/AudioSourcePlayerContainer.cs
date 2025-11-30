using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// A container class to manage and track active audio players using a unique 
/// integer key.
/// </summary>
public class AudioSourcePlayerContainer
{
    /// <summary>
    /// The next available unique key to be assigned to a new audio player.
    /// </summary>
    private int _nextUniqueKey = 0;

    /// <summary>
    /// A dictionary to store and retrieve active audio players using their 
    /// unique integer key.
    /// </summary>
    private Dictionary<int, AudioSourcePlayer> _playersDict;

    /// <summary>
    /// A dictionary to store active audio players using their 
    /// unique <see cref="AudioSourcePlayer"/> key.
    /// </summary>
    private Dictionary<AudioSourcePlayer, int> _playersSecDict;

    public AudioSourcePlayerContainer()
    {
        _playersDict = new();
        _playersSecDict = new();
    }

    /// <summary>
    /// Attempts to retrieve an audio player from the container by its unique key.
    /// </summary>
    /// <param name="id">The unique integer key of the audio player to retrieve.</param>
    /// <param name="player">When this method returns, contains the audio player if found.</param>
    /// <returns>True if the audio player was found; otherwise, false.</returns>
    public bool TryGet(int id, out AudioSourcePlayer player)
    {
        if (_playersDict.TryGetValue(id, out player))
        {
            if (player != null)
                return true;

            // If for some reason an Audio Source Player was destroyed while
            // being used, remove it from the container and return false.
            Remove(id);
            return false;
        }

        return false;
    }

    /// <summary>
    /// Adds a new audio player to the container with a unique key.
    /// </summary>
    /// <param name="player">The audio source player to add.</param>
    /// <returns>The unique integer key assigned to the added audio player.</returns>
    public int Add(AudioSourcePlayer player)
    {
        if (_playersDict.TryGetValue(_nextUniqueKey, out AudioSourcePlayer existingPlayer))
        {
            Debug.LogWarning("Audio source player given already exists in container.");
        }

        _playersDict.Add(_nextUniqueKey, player);
        _playersSecDict.Add(player, _nextUniqueKey);

        return _nextUniqueKey++;
    }

    /// <summary>
    /// Removes an audio player from the container by its unique key.
    /// </summary>
    /// <param name="key">The unique integer key of the audio player to remove.</param>
    /// <returns>True if the audio player was successfully removed; otherwise, false.</returns>
    public bool Remove(int key)
    {
        if (_playersDict.ContainsKey(key) && _playersDict.TryGetValue(key, out AudioSourcePlayer player))
        {
            _playersSecDict.Remove(player);
            _playersDict.Remove(key);
        }

        return true;
    }

    /// <summary>
    /// Removes an audio player from the container by its instance.
    /// This method is intended to be used with a secondary dictionary that maps players to their keys.
    /// </summary>
    /// <param name="player">The audio source player instance to remove.</param>
    /// <returns>True if the audio player was successfully removed; otherwise, false.</returns>
    public bool Remove(AudioSourcePlayer player)
    {
        if (_playersSecDict.ContainsKey(player) && _playersSecDict.TryGetValue(player, out int id))
        {
            _playersDict.Remove(id);
            _playersSecDict.Remove(player);
        }

        return true;
    }
}
