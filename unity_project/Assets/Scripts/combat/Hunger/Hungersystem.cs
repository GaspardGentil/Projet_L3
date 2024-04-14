/*
Ce script g�re le syst�me de faim pour une entit� dans le jeu.
*/
using UnityEngine;

public class HungerSystem : MonoBehaviour
{
    public int maxHunger = 60;
    public int currentHunger;

    private float hungerDepletionRate = 1f; 

     /*
    Cette m�thode est appel�e au d�marrage de l'entit� et initialise la faim maximale et planifie la d�pl�tion de la faim de fa�on r�p�titive.
    */
    private void Start()
    {
        currentHunger = maxHunger;
        InvokeRepeating(nameof(DepleteHunger), 1f, 1f);
    }

        /*
    Cette m�thode d�pl�te la faim de l'entit� � un taux constant.
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
    Cette m�thode d�truit l'entit� lorsque sa faim atteint z�ro.
    */
    private void DestroyEntity()
    {
        Debug.Log("Entity destroyed due to hunger!");
        Destroy(this.gameObject);
    }

      /*
    Cette m�thode journalise avant de d�truire l'entit� � cause de la faim.
    */
    private void LogBeforeDestroy()
    {
        Debug.Log("Entity's hunger reached zero. Destroying entity...");
    }

     /*
    Cette m�thode augmente la faim de l'entit� d'une certaine quantit�.
    @param: amount : type: int : quantit� d'augmentation de la faim
    */
    public void IncreaseHunger(int amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
    }
      /*
    Cette m�thode r�cup�re le niveau de faim actuel de l'entit�.
    @returnValue: int : type : niveau de faim actuel
    */
    public int GetHunger()
    {
        return currentHunger;
    }
}
