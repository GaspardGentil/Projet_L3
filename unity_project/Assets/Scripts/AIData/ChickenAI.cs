using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenAI : MonoBehaviour
{
    private  MonoBehaviour FoodInformationsScript;
    private  MonoBehaviour PredatorInformationsScript;

    // Start is called before the first frame update
    void Start()
    {
        FoodInformationsScript = FindScript("FoodLogger", "loggerfood");
        PredatorInformationsScript=FindScript("Predator_log","loggerpredator");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


     // Function to find a script attached to an object in the scene
    public MonoBehaviour FindScript(string scriptName, string objectName)
    {
        // Find the GameObject with the specified name in the scene
        GameObject obj = GameObject.Find(objectName);

        if (obj != null)
        {
            // Try to get the script component attached to the object
            MonoBehaviour script = obj.GetComponent(scriptName) as MonoBehaviour;

            if (script != null)
            {
                // Script found, return it
                 Debug.Log("Script component with name '" + scriptName + "' was found successfully found on the object named '" + objectName + "'.");
                return script;
            }
            else
            {
                // Script not found, log an error
                Debug.LogError("Script component with name '" + scriptName + "' not found on the object named '" + objectName + "'.");
                return null;
            }
        }
        else
        {
            // Object not found, log an error
            Debug.LogError("Object named '" + objectName + "' not found in the scene.");
            return null;
        }
    }
}
