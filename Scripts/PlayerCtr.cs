using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCtr : MonoBehaviour {
    public bool isAR=true;
    public Camera ctrCmera;
    public RaycastHit hit;
    public LayerMask layer;
    public Cube cube1,cube2;
    public Vector3[] pos;	// Use this for initialization
    public float rotateSpeed = 7;
    float xMove;
   
    public AudioSource rollAudio;
	void Start () {
        pos = new Vector3[2];
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            ctrCmera.transform.parent.Rotate(new Vector3(0, 10 * Time.deltaTime * rotateSpeed, 0));
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            
            
            if (Physics.Raycast(ctrCmera.ScreenPointToRay(Input.mousePosition), out hit, 1000, layer))
            {
               
                if (hit.collider)
                {
                  //  Debug.Log(hit.collider.name);
                    
                    if (hit.collider.tag == "Cube")
                    {
                  //      text.text = Time.time + "s";
                        xMove = 0;
                        cube1 = hit.collider.GetComponent<Cube>();
                      //  Debug.Log(cube1.selectTimer);
                        if (cube1.gameManager.canPlay)
                        {
                            cube1.Select();
                        }else
                        {
                            cube1 = null;
                        }
                    }
                  
                }
            }
            if (!cube1)
            {
                 
                  rollAudio.Play();
                
            }
          
        }
       
        if (Input.touchCount > 0&&cube1==null&&Input.GetTouch(0).phase==TouchPhase.Moved&&!isAR)
        {
           
         
               
              //  if (Input.GetTouch(0).deltaPosition.x )
                {
                    ctrCmera.transform.parent.Rotate(new Vector3(0, Input.GetTouch(0).deltaPosition.x*Time.deltaTime* rotateSpeed, 0));
                
               
                }

           
        }

        if (Input.GetKey(KeyCode.Mouse0)&&cube1&& cube1.gameManager.canPlay)
        {
            if(Input.touchCount>0)
            xMove += Input.GetTouch(0).deltaPosition.x * Time.deltaTime;
            if (Physics.Raycast(ctrCmera.ScreenPointToRay(Input.mousePosition), out hit, 1000, layer))
            {
                if (hit.collider)
                {
                   
                    if (hit.collider.tag == "Cube")
                    {

                        //if (cube2 != null)
                        //{
                        //    cube2.select = false;
                        //}

                        Cube t2 = hit.collider.GetComponent<Cube>();
                        
                        if (cube1.Near(t2))
                        {
                            if(cube2)cube2.select = false;
                            cube2 = t2;
                            cube2.Select();

                        }
                        else
                        {
                            if (t2 == cube1)
                            {
                                t2 = null;

                            }
                            else
                            {

                            }
                        }
                        
                    }
                }
            }
        }
       
        if (Input.GetKeyUp(KeyCode.Mouse0) )
        {
            rollAudio.Pause();
            if (cube1)
            {

                if (Physics.Raycast(ctrCmera.ScreenPointToRay(Input.mousePosition), out hit, 1000, layer))
                    if (hit.collider)
                        if (hit.collider.tag == "Center" && cube2 == null && cube1.Up())
                        {
                            if (xMove < 0)
                            {
                                cube1.Rotate(-1);
                            }
                            else if (xMove > 0)
                            {
                                cube1.Rotate(1);
                            }
                        }



                // if (Physics.Raycast(ctrCmera.ScreenPointToRay(Input.mousePosition), out hit, 1000, layer))
                {
                    //  if (hit.collider)
                    {
                        //     if (hit.collider.tag == "Cube")
                        {

                            if (cube2 && cube1.gameManager.canPlay)
                            {
                                //cube1.select = false;
                                //cube2.select = false;
                                cube1.Exchange(cube2);
                                cube2 = null;
                            }
                            else
                            {
                                cube1.select = false;
                            }
                            cube1 = null;

                        }
                    }
                }
            }
        }


    }
}
