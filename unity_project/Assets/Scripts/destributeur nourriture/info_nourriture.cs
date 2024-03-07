using UnityEngine;

public class NourritureInfo
{
    public int id;
    public bool isSpawned;
    public Vector3 position;

    public NourritureInfo(int _id, bool _isSpawned, Vector3 _position)
    {
        id = _id;
        isSpawned = _isSpawned;
        position = _position;
    }
}
