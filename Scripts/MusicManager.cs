using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    public static MusicManager single;
    public AudioClip[] cubeAudioClip;
    public float[] musicScale;
	// Use this for initialization
	void Awake () {
        if (!single)
        {
            single = this;
        }
       // musicScale = new float[5];

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
