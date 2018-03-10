using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrow : MonoBehaviour
{
    public List<TreeState> State = new List<TreeState>();
    public int currentId;
    public GameObject plant;
    //public List<Transform> mash = new List<Transform>();
    [Range(0, 100)]
    public float timer;
    // Use this for initialization
    void Start()
    {
        transform.localScale = Vector3.zero;
        currentId = -1;
        NextState();
    }
    public void Grow(float time)
    {
        if (currentId >= 0)
        {
            plant.transform.localScale = transform.localScale;
            if (timer > 0)
            {
                timer -= time;
                transform.localScale += Vector3.one * State[currentId].growSpeed * time;
            }
            else
            {
                if (currentId + 1 < State.Count)
                {
                    NextState();
                }
               
            }
        }
    }
    public void NextState()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        currentId++;
        timer = State[currentId].growTime;
        plant = Instantiate(State[currentId].treePrefab, transform.position, transform.rotation);
        plant.transform.parent = transform;
        plant.transform.localScale = transform.localScale;
        // Transform[] ts = GetComponentsInChildren<Transform>();

    }
    // Update is called once per frame
    void Update()
    {

    }
}
[System.Serializable]
public class TreeState
{

    public GameObject treePrefab;
    public float growTime;
    public float growSpeed;
}
