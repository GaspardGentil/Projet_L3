using UnityEngine;

[System.Serializable]
public class PredatorInfoMono : MonoBehaviour
{
    [SerializeField]
    public int id;

    [SerializeField]
    public Vector3 position;



    private void Start()
    {
        // Log the initial position of the predator GameObject
        position = transform.position;
        Debug.Log("Predator initial position logged: " + position);
    }

    public PredatorInfoMono(int _id, Vector3 _position)
    {
        id = _id;
        position = _position;
    }
}
