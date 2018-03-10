using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameLevelManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	public void NextLevel(int id)
    {
        SceneManager.LoadScene(id);   
    }
	// Update is called once per frame
	void Update () {
		
	}
}
