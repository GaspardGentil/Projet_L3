using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEat : MonoBehaviour
{
    public EntityLife entityLife;

	[SerializeField] float foodValue = 10f; //Time added

    // Start is called before the first frame update
    void Start()
    {
        entityLife = GetComponent<EntityLife>();    
        if(entityLife == null){
			Debug.LogError("EntityLife not found on " + gameObject.name);
            enabled = false;
		}
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Food")){
			entityLife.setTime(foodValue);
			Destroy(other.gameObject);
		}
	}
}
