using UnityEngine;



public class singletonComponents : MonoBehaviour {
    // singleton specific to hold a larger amounts of managers through my scenes (attach to root gameObject)
    public static singletonComponents Instance ;
    // Use this for initialization
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
           
             Destroy(transform.root.gameObject);
             return;
        }
           
        else
            Instance = this;


        DontDestroyOnLoad(transform.gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
