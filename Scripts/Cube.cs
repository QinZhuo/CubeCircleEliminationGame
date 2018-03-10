using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Cube : MonoBehaviour {
    
    public int index;
    public EliminationGame_3D gameManager;
    public int mi,mj;
    public int xScore, yScore;
    [SerializeField] float timer;
    [SerializeField] Vector3 lastPosition;
    public AnimationCurve downCurve;
    public float selectTimer;
    public bool select;
    public AudioSource audiosource;
   // public Vector3 selectPosition;
    //List<Cube> removeList;
    public int downLayerNum;
    public MeshRenderer meshs;
  //  public bool Raycast=true;
    // Use this for initialization
    void Awake () {
        //  removeList = new List<Cube>();
        audiosource = GetComponent<AudioSource>();
        audiosource.volume = MusicManager.single.musicScale[0];
        meshs = GetComponentInChildren<MeshRenderer>();
          select = false;
        meshs.enabled = false;
     //   selectPosition = transform.position;
    }

    public bool Up()
    {
        int up = mi + 1 > gameManager.height - 1 ? 0 : mi + 1;
        if (gameManager.cubes[up, mj]==null||mi==gameManager.height)
        {
            return true; 
        }
        return false;
    }
    public void Rotate(int i)
    {
        gameManager.cubes[mi, mj] = null;
        mj = GetSpace(i);
        transform.position = gameManager.GetCubePos(mi,mj );
        gameManager.cubes[mi, mj] = this;
        gameManager.cubes[mi, mj].transform.rotation = gameManager.InitPos[mj].rotation;
        gameManager.EliminateRun();
    }
    public int GetSpace(int i)
    {
        int sum = 0;
        for(int t=1;t<18;t++)
        {
            
            if (gameManager.cubes[mi, ((mj + i * t)<0? (mj + i * t)+18: (mj + i * t)) %18])
            {
                break;
            }
            else
            {
                sum++;
            }
        }
        return ((mj + i * sum) < 0 ? (mj + i * sum) + 18 : (mj + i * sum)) % 18;
    }
    //public bool Elimination()
    //{
    //    if (xScore >= 3 || yScore >= 3)
    //    {
    //        //foreach (var item in removeList)
    //        //{
    //        //    //item.Elimination();
    //        //    if(item.removeList.Count>0) item.Elimination();
    //        //}
    //        //removeList.Clear();
    //        Remove();
    //        return true;
    //    }
    //    return false;
    //}
    public void Remove()
    {
        gameManager.cubes[mi, mj] = null;
        //Color c = GetComponent<Renderer>().material.color;
        //c.a = 0.3f;
        //GetComponent<Renderer>().material.color = c;
        GetComponent<Animator>().SetTrigger("remove");
        audiosource.clip = MusicManager.single.cubeAudioClip[0];
        if (EliminationGame_3D.gameStarted) { audiosource.Play(); PlantGrow.single.DataUp(index); }
        Destroy(gameObject,1);
        Destroy(this);
        
    }
    public void GetScore()
    {
      //  removeList.Clear();
        xScore = 1;yScore =1;
        int left, right, up, down;
        left = mj - 1 < 0 ? 17 : mj - 1;
        right = mj + 1 > 17 ? 0 : mj + 1;
        down = mi - 1 < 0 ? gameManager.height-1: mi - 1;
        up= mi + 1 > gameManager.height - 1 ? 0: mi + 1;
        if(gameManager.cubes[mi, left])
        if (gameManager.cubes[mi, left].index == this.index) xScore++;
        if (gameManager.cubes[mi, right])
            if (gameManager.cubes[mi, right].index == this.index) xScore++;
        if (xScore >= 3) {
            gameManager.cubeRemoveFlag[mi, left] = true;
            gameManager.cubeRemoveFlag[mi, right] = true;
            gameManager.cubeRemoveFlag[mi, mj] = true;
            //removeList.Add(gameManager.cubes[mi, left]);
            //removeList.Add(gameManager.cubes[mi, right]);
        }
        if (gameManager.cubes[up, mj])
            if (gameManager.cubes[up, mj].index == this.index) yScore++;
        if (gameManager.cubes[down, mj])
            if (gameManager.cubes[down, mj].index == this.index) yScore++;
        if (yScore >= 3)
        {
            gameManager.cubeRemoveFlag[up, mj] = true;
            gameManager.cubeRemoveFlag[down, mj] = true;
            gameManager.cubeRemoveFlag[mi, mj] = true;
            //removeList.Add(gameManager.cubes[up, mj]);
            //removeList.Add(gameManager.cubes[down, mj]);
        }
    }
    public bool Near(Cube other)
    {
        int y =Mathf.Abs(other.mi - mi);
        int x = Mathf.Abs(other.mj - mj);
        if ((x + y) == 1)
        {
            return true;
        }
        else if(x==17&&y==0)
        {
            return true;
        }
        return false;
    }
    public void Down(int num=1)
    {
        if (num == 0) return;
        lastPosition = transform.position;
        //    Raycast = false;
        gameManager.canPlay = false;
        timer = 0;
        downLayerNum = num;
        
    }
    [ContextMenu("select")]
    public void Select()
    {
       // if (s)
        {
            if (!select)
            {
                
                
                
                select = true;
               // Raycast = false;
            }
        }
        

    }
    


    public void DownAnim()
    {
        if (lastPosition != Vector3.zero)
        {
            timer += Time.deltaTime;
            transform.position = lastPosition - new Vector3(0, downCurve.Evaluate(timer) * 0.8f * downLayerNum);
            gameManager.canPlay = false;
            if (timer >= 1)
            {
                timer = 0;
             //   selectPosition = transform.position;
                lastPosition = Vector3.zero;
                //Raycast = true;
                gameManager.EliminateRun();
                gameManager.canPlay = true;
            }
        }
        else
        {

            Down(gameManager.Down(this));

        }
    }
    public void SelectAnim()
    {
        if (select)
        {
            if (selectTimer < 1)
            {
                selectTimer += Time.deltaTime;
            }
            else
            {
                selectTimer = 1;

            }
            transform.position = gameManager.GetCubePos(mi,mj) - transform.forward * gameManager.selectCurve.Evaluate (selectTimer / 1f) * .5f;
            

        }
        else if (lastPosition == Vector3.zero)
        {
            if (selectTimer > 0)
            {
                selectTimer -= Time.deltaTime;
            }
            else
            {
                selectTimer = 0;
             //   Raycast = true;
            }
            transform.position = Vector3.Lerp(gameManager.GetCubePos(mi, mj), transform.position, selectTimer);
        }
    }
    Vector3 changeLastPos;
    bool change = true;
    public void ExchangeAnim()
    {
        if (moveId==0) return; 
        exchangeTimer += Time.deltaTime;
        if (exchangeTimer > 1) exchangeTimer = 1;
        transform.position = Vector3.Lerp(changeLastPos, gameManager.GetCubePos(mi, mj), exchangeTimer)+gameManager.exchangeCurve.Evaluate(exchangeTimer)
            *transform.forward*(moveId==1?-1:1);
        transform.rotation = Quaternion.Lerp(transform.rotation, gameManager.InitPos[mj].rotation, exchangeTimer);
        if (exchangeTimer >= 1)
        {
            exchangeTimer = 0;
            if (moveId == 1&& change)
            {
                gameManager.EliminateRun();
                if (gameManager.eliminateNum == 0)
                {
                    other.exchangeTimer = 0;
                    change = false;
                    Exchange(other);
                    
                    return;
                }
            }
            change = true;
            gameManager.canPlay = true;
            moveId = 0;
            
           
        }
        
    }
    [SerializeField] float exchangeTimer = 0;
    public int moveId=0;
    //public Vector3 movePos;
    //public Quaternion moveRot;
    Cube other;
    public void Exchange(Cube other)
    {
        this.other = other;
        select = false;
        other.select = false;   
        exchangeTimer = 0;
        moveId = 1;other.moveId = 2;
        changeLastPos = transform.position;
        other.changeLastPos = other.transform.position;
       // moveRot = gameManager.InitPos[] other.transform.rotation;

        // other.moveRot = transform.rotation;
        int ti = mi, tj = mj;
        mi = other.mi;
        mj = other.mj;
        other.mj = tj;
        other.mi = ti;
        gameManager.cubes[mi, mj] = this;
        gameManager.cubes[other.mi, other.mj] = other;
        gameManager.canPlay = false;
       
    }
    // Update is called once per frame
    void FixedUpdate () {
        if (!EliminationGame_3D.gameStarted) return;
        DownAnim();
        SelectAnim();
        ExchangeAnim();
        
    }
    public void Update()
    {
        if (mi >gameManager.playHeight+2)
        {
            meshs.enabled = false;
        }
        else
        {
            meshs.enabled = true;
        }
    }
}
