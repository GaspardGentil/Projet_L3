using UnityEngine;

public class EntityProperties : MonoBehaviour
{
    [SerializeField] 
    private int fertility; // Measure of the fertility of the entity

    [SerializeField] 
    private Sex sex; // Sex of the entity

    public enum Sex
    {
        Male,
        Female
    }

    private void Start()
    {
        RandomizeSex();
        fertility=1;
    }

    public void RandomizeSex()
    {
        // Randomizing the sex of the entity
        sex = (Sex)Random.Range(0, 2);
        Debug.Log("Sex randomized to: " + sex);
    }

    public Sex GetSex()
    {
        // Returns the sex of the entity
        return sex;
    }

    public int GetFertility()
    {
        // Returns the fertility of the entity
        return fertility;
    }

    public void IncreaseFertility(int amount)
    {
        // Increases the fertility of the entity by the specified amount
        fertility += amount;
    }

    public void DecreaseFertility(int amount)
    {
        // Decreases the fertility of the entity by the specified amount
        fertility -= amount;
        if (fertility < 0)
        {
            fertility = 0; // Ensure fertility doesn't go negative
        }
    }
}
