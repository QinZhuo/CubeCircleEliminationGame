using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantGrow : MonoBehaviour {
    public static PlantGrow single;
    public float PlantTimer;
    public Dictionary<PlantData, float> data=new Dictionary<PlantData, float>();
    public AnimationCurve[] curves;
    public Text text;
    public float oTimer;
    public float changeScale=1;
    public TreeGrow tree;
    public Slider[] sliders;
    // Use this for initialization
    void Awake () {
        if (single == null) single = this;
        for (int i = 0; i < 8; i++)
        {
            float f = 0.5f;
            data.Add((PlantData)i, f);
        }
        data[PlantData.生命] = 1;
        data[PlantData.生长] = 0;
        data[PlantData.技能] = 0;
        oTimer = 0;

    }
    public void DataUp(int index)
    {
        data[(PlantData)index] += 0.05f*changeScale;
    }
    float f1, f2;
    void PlantDataChange()
    {
    

        f1 = (-curves[0].Evaluate(data[PlantData.水分])
            - curves[1].Evaluate(data[PlantData.物资])
            - curves[1].Evaluate(data[PlantData.光照])
             - curves[1].Evaluate(data[PlantData.除虫])
            - (oTimer > 15 ? (curves[1].Evaluate(1 - data[PlantData.氧气])) : 0));
        f2 = (1 - curves[1].Evaluate(1 - data[PlantData.氧气]))
            * (1 - curves[1].Evaluate(1 - data[PlantData.物资]))
            * (1 - curves[1].Evaluate(1 - data[PlantData.光照]));

        data[PlantData.生命] += f1*0.01f * Time.deltaTime* changeScale;
        data[PlantData.生长]+= f2* Time.deltaTime* changeScale;
        tree.Grow(f2 * Time.deltaTime * changeScale);
        data[PlantData.水分] += -curves[2].Evaluate(data[PlantData.光照]) * Time.deltaTime*0.01f* changeScale;
        if (1 - data[PlantData.氧气] > 0.8f)
        {
            oTimer += Time.deltaTime;
        }
        else
        {
            oTimer -= Time.deltaTime * 0.7f;
            if (oTimer < 0) oTimer = 0;
        }

        data[PlantData.氧气] -= 0.01f * Time.deltaTime * changeScale;
        data[PlantData.物资] -= 0.005f * Time.deltaTime * changeScale;
        data[PlantData.光照] -= 0.02f * Time.deltaTime * changeScale;
        data[PlantData.除虫] -= 0.01f * Time.deltaTime * changeScale;
        data[PlantData.水分] -= 0.017f * Time.deltaTime * changeScale;
        for (int i = 0; i < 8; i++)
        {

            if (data[(PlantData)i] < 0) data[(PlantData)i] = 0;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        PlantTimer += Time.deltaTime;
        PlantDataChange();
        for (int i = 0; i < 8; i++)
        {
            //if ((PlantData)i==PlantData.生命)
            //{
            //    sliders[i].value = data[(PlantData)i]/100f;
            //}
            //else
            if((PlantData)i == PlantData.生长)
            {
               if(sliders[i]) sliders[i].value = data[(PlantData)i]/100f;
            }
            else
            {
                if (sliders[i]) sliders[i].value = data[(PlantData)i];

            }
            
        }
       
        //text.text = "";
        //for (int i = 0; i < 8; i++)
        //{
        //    text.text += ((PlantData)i).ToString() + ":" + data[(PlantData)i]+"\n";
        //}
        //text.text += f2+" "+f1 + "\n";
    }
    int ci=0;
    private void Update()
    {

        //if( Input.GetKeyDown(KeyCode.RightArrow)){
        //    ci++;
        //    ci = ci % 8;
        //    Debug.Log(ci);
        //}
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    data[(PlantData)ci] += 0.1f;
        //}else if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    data[(PlantData)ci] -= 0.1f;
        //}
    }
}
public enum PlantData{
    生长,
    生命,
    水分,
    氧气,
    物资,
    除虫,
    光照,
    技能
}
