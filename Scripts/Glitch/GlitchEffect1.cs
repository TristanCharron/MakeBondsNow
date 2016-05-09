using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/GlitchEffect")]
public class GlitchEffect1 : ImageEffectBase
{

    public Texture2D displacementMap;

    //Shader parameters transfered to Shader.

    [Range(0.1f, 10f)]
    public float intensity;
    [Range(1f, 10f)]
    public float filterRadius;
    public bool flipDown = false, flipUp = false;
    static bool isGlitching = false;



    public IEnumerator onGlitch(float length)
    {
        isGlitching = true;

        onGlitchText(length);

        yield return new WaitForSeconds(length);

        isGlitching = false;
        yield break;
    }


    public void onGlitchText(float length)
    {
        Text[] txts = FindObjectsOfType(typeof(Text)) as Text[];
        foreach (Text txt in txts)
            txt.GetComponent<UIEffects>().shuffle(txt.text, length);
    }

    // Called by camera to apply image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        float _intensity = isGlitching ? intensity : 0;
        float _filterRadius = isGlitching ? Random.Range(-filterRadius, filterRadius) : 0;
        

        material.SetFloat("_Intensity", _intensity);
        material.SetFloat("_Chroma", Random.Range(0,2f));
        material.SetTexture("_DispTex", displacementMap);
        material.SetFloat("filterRadius", _filterRadius);


        if (isGlitching)
        {
            material.SetFloat("flip_up", Random.Range(0.3f,0.6f));
            material.SetFloat("flip_down", Random.Range(0.3f, 0.6f));

        }
        else
        {
            material.SetFloat("flip_down", 1);
            material.SetFloat("flip_up", 0);
        }


        //Tweak displacement map displacement probability
        if (Random.value < 0.1f)
        {
            material.SetFloat("displace", Random.Range(0,_intensity));
            material.SetFloat("scale", isGlitching ? 1 - Random.Range(0, _intensity) : 0);
        }
        else
            material.SetFloat("displace", 0);



        Graphics.Blit(source, destination, material);
    }
}

