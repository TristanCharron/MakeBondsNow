using UnityEngine;
using UnityEngine.UI;




public class colorManager : MonoBehaviour
{
    private static SpriteRenderer sprite_Renderer;
    private static SpriteRenderer[] sprite_Renderers;
    private static Image img_Renderer;
    private static Image[] img_Renderers;
    private static Text[] txt_Renderers;
    public static string currentColor;
    
    //System names and const values for color settings
    public const string system = "System", defaultColor = "BILL", color1 = "ANNA", color2 = "DOUG";
    public static colorManager Instance { get; private set; }
    public float _txtAlpha;
    public static float txtAlpha;
    public Shader gameModeShader;


    // Default Teal Color
    public  Color bill;
    private static Color Bill;
    // Purple Color
    public  Color anna;
    private static Color Anna;
    // Red Color
    public  Color doug;
    private static Color Doug;
    

    
    void Awake()
    {

      
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
           
        else
        {
            Instance = this;
            txtAlpha = _txtAlpha;
            Bill = bill;
            Anna = anna;
            Doug = doug;
            PlayerPrefs.SetString(system, defaultColor);
        }

        



    }
    public static void onInitialize()
    {

       currentColor = PlayerPrefs.GetString(system);

    }

    private void Update()
    {
       
        //Synchronize UI color in real time based on system
        if (currentColor != PlayerPrefs.GetString(system)) 
            currentColor = PlayerPrefs.GetString(system);
           
    }

    

   
    
    private static Color returnColorByName()
    {
        switch (PlayerPrefs.GetString(system))
        {
            case color1:
                return Anna;
            case color2:
                return Doug;
            default:
                return Bill;

        }
       

    }

   
    public void setAlpha(float alpha)
    {
        if (sprite_Renderer != null)
            setAlpha(ref sprite_Renderer, alpha);
        else if (img_Renderer != null)
            setAlpha(ref img_Renderer, alpha);
        else
            Debug.LogError("No components found for this colorManager" + gameObject.name);
    }


    public void setAlpha(ref SpriteRenderer renderer, float alpha)
    {

        Color color = renderer.color;
        renderer.color = new Color(color.r, color.g, color.b, alpha);
    }

    public void setAlpha(ref Image renderer, float alpha)
    {

        Color color = renderer.color;
        renderer.color = new Color(color.r, color.g, color.b, alpha);
    }

 

    private static void setUIColor()
    {
        // Set all UI elements color based on system chosen.

        Color newColor = returnColorByName();

        img_Renderers = FindObjectsOfType(typeof(Image)) as Image[];
        sprite_Renderers = FindObjectsOfType(typeof(SpriteRenderer)) as SpriteRenderer[];
        txt_Renderers = FindObjectsOfType(typeof(Text)) as Text[];
        
        //Image Renderers
        for (int i = 0; i < img_Renderers.Length; i++)
        {
            if(img_Renderers[i].transform.root.gameObject.tag == "colorManagerTag" && img_Renderers[i].transform.gameObject.tag != "colorManagerDont")
            img_Renderers[i].color = new Color(newColor.r, newColor.g, newColor.b, img_Renderers[i].color.a);
        }
            
        // Sprite Renderer
        for (int j = 0; j < sprite_Renderers.Length; j++)
        {
            if (sprite_Renderers[j].transform.root.gameObject.tag == "colorManagerTag" && sprite_Renderers[j].transform.gameObject.tag != "colorManagerDont")
                sprite_Renderers[j].color = new Color(newColor.r, newColor.g, newColor.b, sprite_Renderers[j].color.a);
        }
    
        // Text UI Renderers
    
        for (int k = 0;k < txt_Renderers.Length; k++)
        {
            if ( txt_Renderers[k].transform.root.gameObject.tag == "colorManagerTag" && txt_Renderers[k].transform.gameObject.tag != "colorManagerDont")
                txt_Renderers[k].color = new Color(newColor.r, newColor.g, newColor.b, txtAlpha);
        }
      

    }

    



}