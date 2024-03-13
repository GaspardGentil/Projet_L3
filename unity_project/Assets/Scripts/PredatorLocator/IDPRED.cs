using UnityEngine;

[System.Serializable]
public class PredatorInfo 
{
    [SerializeField]
    public int id;

    [SerializeField]
    public Vector3 position;

 

    public PredatorInfo(int _id, Vector3 _position)
    {
        id = _id;
        position = _position;
    }
}
