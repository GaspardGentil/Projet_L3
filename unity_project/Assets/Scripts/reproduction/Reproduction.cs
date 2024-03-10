using UnityEngine;

public class ReproductionSystem : MonoBehaviour
{
    // OnTrigger event to detect collisions with other entities
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the same tag as the current entity
        if (other.CompareTag(gameObject.tag))
        {
            Debug.Log("repoduction started");
            // Get the EntityProperties component of the collided entity
            EntityProperties myProperties = GetComponent<EntityProperties>();
            EntityProperties otherProperties = other.GetComponent<EntityProperties>();

            // Check if both entities have a fertility of at least 1
             if (myProperties.GetFertility() >= 1 && otherProperties != null && otherProperties.GetFertility() >= 1 && myProperties.GetSex() == otherProperties.GetSex())
            {
                // Duplicate the entity
                DuplicateEntity(other.gameObject);

                // Decrease the fertility of both entities
                myProperties.DecreaseFertility(1);
                otherProperties.DecreaseFertility(1);
            }
        }
    }

     // Function to duplicate the collided entity
       private void DuplicateEntity(GameObject entityToDuplicate)
    {
        Vector3 spawnPosition = entityToDuplicate.transform.position;
        Quaternion spawnRotation = entityToDuplicate.transform.rotation;
        Transform allChickenTransform = GameObject.Find("All_Chicken").transform;

        GameObject newEntity = Instantiate(entityToDuplicate, spawnPosition, spawnRotation, allChickenTransform);
    
        // Get the EntityProperties component of the duplicated entity
        EntityProperties newEntityProperties = newEntity.GetComponent<EntityProperties>();
    
        // Call the RandomizeSex function of the EntityProperties class
        newEntityProperties.RandomizeSex();
    }


}
