using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminationGame_3D : MonoBehaviour {
    public GameObject gameover;
    public static bool gameStarted = false;
    public bool canPlay=true;
    public Transform[] InitPos;
   // public GameObject cubeObject;
    public Cube[,] cubes;
    public bool[,] cubeRemoveFlag;
    public AnimationCurve downCurve;
    public AnimationCurve selectCurve;
    public AnimationCurve exchangeCurve;
    public GameObject[] cubePrefabs;
    public int height=20;
    public int playHeight = 8;
    public int eliminateNum;
    public float timer;
    public float[] initRate;
    public int currentHeight;
    public Transform[] centerTrans;
    
    // Use this for initialization
    public Vector3 GetCubePos(int i,int j)
    {
        return InitPos[j].position + new Vector3(0, i * 0.8f, 0);
    }
    
    void Start () {
       
        gameStarted = false;
        eliminateNum =10;
        timer = 0;
        cubes =new Cube[height, 18];
        cubeRemoveFlag = new bool[height, 18];
        currentHeight = height-1;
        int randi=-1;
        float initI;
        for (int i = 0; i < cubes.GetLength(0); i++)
        {
            for (int j = 0; j < cubes.GetLength(1); j++)
            {
                initI = Random.Range(0, 1f);
                for (int z = 0; z < initRate.Length; z++)
                {
                    if (initI<initRate[z]) { randi = z;break; }
                }

               // randi = Random.Range(0, cubePrefabs.Length);
                cubes[i,j]= Instantiate(cubePrefabs[randi], GetCubePos(i,j), InitPos[j].rotation).GetComponent<Cube>();
                cubes[i, j].gameManager = this;
                cubes[i, j].mi = i;
                cubes[i, j].mj = j;
           
                cubes[i, j].transform.parent = gameObject.transform;
                cubes[i, j].name = i.ToString() + "," + j.ToString();
            }
          
        }
       // target = centerTrans[0].position;
        InitGame();
    }
    
    public int Down(Cube cube)
    {
        int num = 0;
        int ti=cube.mi;
        if (ti > 0) {
            if (cubes[ti - 1, cube.mj] != null) return num;
            cubes[ti - 1, cube.mj] = cube;
            cube.mi = ti - 1;
            cubes[ti, cube.mj] = null;
            num++;
            num += Down(cube);
           
               
            
           // 
           
        }
        return num ;

    }
   // [ContextMenu("EliminateRun")]
   public void InitGame(int f=0)
    {
        
        for (int i = 0; i < cubes.GetLength(0); i++)
        {
            for (int j = 0; j < cubes.GetLength(1); j++)
            {
                if (cubes[i, j])
                    cubes[i, j].GetScore();
            }
        }
        //for (int i = 0; i < cubes.GetLength(0); i++)
        //{
        //    for (int j = 0; j < cubes.GetLength(1); j++)
        //    {
        //        if (cubes[i, j])
        //        {
        //            if (cubes[i, j].Elimination())
        //            {
        //                eliminateNum++;

        //            }
        //        }

        //    }
        //}
        for (int i = 0; i < cubes.GetLength(0); i++)
        {
            for (int j = 0; j < cubes.GetLength(1); j++)
            {
                if (cubeRemoveFlag[i, j])
                {
                    cubes[i, j].Remove();
                    eliminateNum++;
                }
            }
        }

        for (int i = 0; i < cubes.GetLength(0); i++)
        {
            for (int j = 0; j < cubes.GetLength(1); j++)
            {
                if (cubes[i, j] == null)
                {
                    int randi = Random.Range(0, cubePrefabs.Length);
                    cubes[i, j] = Instantiate(cubePrefabs[randi], InitPos[j].position + new Vector3(0, i * 0.8f, 0), InitPos[j].rotation).GetComponent<Cube>();
                    cubes[i, j].gameManager = this;
                    cubes[i, j].mi = i;
                    cubes[i, j].mj = j;
                    cubes[i, j].transform.parent = gameObject.transform;
                    cubes[i, j].name = i.ToString() + "," + j.ToString();
                }
            }
        }
        gameStarted = true;
        EliminateRun();
    }
    public void EliminateRun()
    {
        eliminateNum = 0;

        for (int i = 0; i < playHeight; i++)
        {
            for (int j = 0; j < cubes.GetLength(1); j++)
            {
                cubeRemoveFlag[i, j] = false;
            }
        }

        for (int i = 0; i < playHeight; i++)
        {
            for (int j = 0; j < cubes.GetLength(1); j++)
            {

                if (cubes[i, j])
                    cubes[i, j].GetScore();
            }
        }
        //for (int i = 0; i < cubes.GetLength(0); i++)
        //{
        //    for (int j = 0; j < cubes.GetLength(1); j++)
        //    {
        //        if (cubes[i, j])
        //        {
        //            if (cubes[i, j].Elimination())
        //            {
                       
        //            }
        //        }

        //    }
        //}
        for (int i = 0; i < playHeight; i++)
        {
            for (int j = 0; j < cubes.GetLength(1); j++) {
                if (cubeRemoveFlag[i,j])
                {
                    cubes[i, j].Remove();
                    eliminateNum++;
                }
            }
        }
                timer = 0;
    }
	// Update is called once per frame
	void FixedUpdate () {
        if (!EliminationGame_3D.gameStarted) return;
        bool space = true;
        for (int j = 0; j < 18; j++)
        {
            if (cubes[currentHeight, j])
            {
                space = false;
            }
        } 
        if (space&&currentHeight>1)
        {
            currentHeight--;
           
        }
        centerTrans[0].position = Vector3.Lerp(centerTrans[0].position,new Vector3(0,GetCubePos(currentHeight, 0).y + 0.1f, 0), Time.deltaTime);
        if (centerTrans[0].position.y<= GetCubePos(12, 0).y)
        {
            centerTrans[1].parent = centerTrans[0];
        }
        if (currentHeight <= 11)
        {
            canPlay = false;
            gameover.SetActive(true);
            for (int i = 2; i <currentHeight; i++)
            {
                for (int j = 0; j < cubes.GetLength(1); j++)
                {
                    if (cubes[i, j]) cubes[i, j].Remove();
                }
            }
        }
        //timer += Time.deltaTime;
        //if (timer >= 1&&(eliminateNum>0))
        //{
        //    EliminateRun();
        //}

    }
}
