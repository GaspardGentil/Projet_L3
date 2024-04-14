/*
Ce script gère le système de faim pour une entité dans le jeu.
*/
using UnityEngine;

public class HungerSystem : MonoBehaviour
{
    public int maxHunger = 60;
    public int currentHunger;

    private float hungerDepletionRate = 1f; 

     /*
    Cette méthode est appelée au démarrage de l'entité et initialise la faim maximale et planifie la déplétion de la faim de façon répétitive.
    */
    private void Start()
    {
        currentHunger = maxHunger;
        InvokeRepeating(nameof(DepleteHunger), 1f, 1f);
    }

        /*
    Cette méthode déplète la faim de l'entité à un taux constant.
    */
    private void DepleteHunger()
    {
        if (currentHunger > 0)
        {
            currentHunger -= 1;
            if (currentHunger <= 0)
            {
                LogBeforeDestroy();
                DestroyEntity();
            }
        }
    }

      /*
    Cette méthode détruit l'entité lorsque sa faim atteint zéro.
    */
    private void DestroyEntity()
    {
        Debug.Log("Entity destroyed due to hunger!");
        Destroy(this.gameObject);
    }

      /*
    Cette méthode journalise avant de détruire l'entité à cause de la faim.
    */
    private void LogBeforeDestroy()
    {
        Debug.Log("Entity's hunger reached zero. Destroying entity...");
    }

     /*
    Cette méthode augmente la faim de l'entité d'une certaine quantité.
    @param: amount : type: int : quantité d'augmentation de la faim
    */
    public void IncreaseHunger(int amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
    }
      /*
    Cette méthode récupère le niveau de faim actuel de l'entité.
    @returnValue: int : type : niveau de faim actuel
    */
    public int GetHunger()
    {
        return currentHunger;
    }
}
