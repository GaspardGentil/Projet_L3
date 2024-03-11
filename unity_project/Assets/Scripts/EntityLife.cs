using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityLife : MonoBehaviour
{
	[SerializeField] float lifespan = 5f; //life remaining in seconds

    private float timer; //time to death

    public void setTime(float time){
		this.timer = time;
	}
    
    // Start is called before the first frame update
    void Start()
    {
        timer = lifespan;      
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
		if (timer <= 0f){
			Destroy(gameObject);
		}        
    }
}
