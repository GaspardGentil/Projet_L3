using UnityEngine;

public class ReproductionSystem : MonoBehaviour
{
    private ParticleSystem reproductionParticles; // Reference to the Particle System component

    private void Start()
    {
        // Find the "Body" child object
        Transform bodyTransform = transform.Find("Body");
        
        if (bodyTransform != null)
        {
            // Get the Particle System component attached to the "Body" child object
            reproductionParticles = bodyTransform.GetComponentInChildren<ParticleSystem>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(gameObject.tag))
        {
            EntityProperties myProperties = GetComponent<EntityProperties>();
            EntityProperties otherProperties = other.GetComponent<EntityProperties>();

            if (myProperties.GetFertility() >= 1 && otherProperties != null && otherProperties.GetFertility() >= 1 && myProperties.GetSex() == otherProperties.GetSex())
            {
                DuplicateEntity(other.gameObject);

                myProperties.DecreaseFertility(1);
                otherProperties.DecreaseFertility(1);

                // Turn on particle emission
                ToggleParticleEmission(true);

                // Turn off particle emission after 5 seconds
                Invoke("TurnOffParticleEmission", 5f);
            }
        }
    }

    private void DuplicateEntity(GameObject entityToDuplicate)
    {
        Vector3 spawnPosition = entityToDuplicate.transform.position;
        Quaternion spawnRotation = entityToDuplicate.transform.rotation;
        Transform allChickenTransform = GameObject.Find("All_Chicken").transform;

        GameObject newEntity = Instantiate(entityToDuplicate, spawnPosition, spawnRotation, allChickenTransform);

        EntityProperties newEntityProperties = newEntity.GetComponent<EntityProperties>();

        newEntityProperties.RandomizeSex();
    }

    private void TurnOffParticleEmission()
    {
        ToggleParticleEmission(false);
    }

    private void ToggleParticleEmission(bool enable = false)
    {
        if (reproductionParticles != null)
        {
            var emission = reproductionParticles.emission;
            emission.enabled = enable;
        }
    }
}
