using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPointToRay : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    Vector3 v3 = new Vector3(300, 600);
    Vector3 hitpoint = Vector3.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //射线沿着屏幕X轴从左向右循环扫描
        // v3.x = v3.x >= Screen.width ? 0.0f : v3.x + 1.0f;
        //生成射线
        ray = Camera.main.ScreenPointToRay(v3);
        // 射线起始位置是在近剪裁面上的点，就是鼠标点击的的屏幕位置，ScreenPointToRay 方法会把屏幕点转换成世界空间的点，
        // 然后返回的射线从摄像机的位置通过
        Debug.Log("射线起始位置:" + ray.origin + " 方向:" + ray.direction);
        //绘制线，在Scene视图中可见
        Debug.DrawLine(ray.origin, hit.point, Color.red);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            //输出射线探测到的物体的名称
            Debug.Log("射线探测到的物体名称：" + hit.transform.name);
        }

        // 在这段代码中，首先声明了一个变量v3，用于记录射线到屏幕上的实际像素坐标，
        // 然后在Update方法中更改v3的x分量值，使得射线从屏幕的左方向右方不断循环扫描，
        // 接着调用方法ScreenPointToRay生成射线ray，最后绘制射线和打印射线探测到的物体的名称。
    }
}
