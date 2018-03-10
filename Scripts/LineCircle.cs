using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCircle : MonoBehaviour {
    public Transform[] child;
    public LineCircle to;
	// Use this for initialization
	void Start () {
       // child = GetComponentsInChildren<Transform>();
        

    }
	
	// Update is called once per frame
	void Update () {
        if(to)
        for (int i = 0; i < child.Length; i++)
        {
            if (child[i])
            {
                child[i].GetComponent<LineRenderer>().SetPositions(new Vector3[] { child[i].position, to.child[i].position });
            }
        }
	}
}
