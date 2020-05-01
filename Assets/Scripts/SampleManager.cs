using System.Collections;
using System.Collections.Generic;
using SpriteGlow;
using UnityEngine;

public class SampleManager
{
    public List<Sample> samples;
    public Transform parent;
    public GameObject prefab;
    private SpriteGlowEffect sampGlow;
    public float glowLowRange = 3f;
    public float glowHighRange = 4f;
    
    // Start is called before the first frame update
    public void Initialize(Transform parent, GameObject prefab)
    {
        this.parent = parent;
        this.prefab = prefab;
        samples = new List<Sample>();
    }
    public void CreateSample(Vector2Int pos){
        Sample samp = new Sample(pos);
        samples.Add(samp);
        sampGlow = samp.obj.GetComponentInChildren<SpriteGlowEffect>();
    }
    public void ClearSamples(){
        foreach(Sample samp in samples){
            samp.DestroySelf();
        }
        samples.Clear();
    }
    public bool IsSampleHere(Vector2Int pos){
        foreach(Sample samp in samples){
            if(samp.beingCarried == false && samp.pos == pos){
                Debug.Log(pos);
                return true;
            }
        }
        return false;
    }
    public Sample GetSampleHere(Vector2Int pos){
        foreach(Sample samp in samples){
            if(samp.pos == pos){
                return samp;
            }
        }
        Debug.Log("There is no sample at ("+pos.x+","+pos.y+")");
        return null;
    }
    
}
