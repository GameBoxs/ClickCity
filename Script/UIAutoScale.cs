using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAutoScale : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        Camera camera = GetComponent<Camera>(); //카메라에 이 스크립트를 넣음.
        Rect rect = camera.rect; // 카메라 rect 가져옴
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)16 / 9); // 가로/세로 / 16/9 값이 나옴
        float scalewidth = 1f / scaleheight; //
        if (scaleheight < 1) // 만약 1보다 작을때는 높이가 작기때문에 위, 아래 화면은 검정색으로 작은부분만큼 나와야함.
        {
            rect.height = scaleheight; // 카메라 뷰포트 렉트의 height는 scaleheight 만큼
            rect.y = (1f - scaleheight) / 2f; // 렉트 y 는 1f 뺀만큼 넣음
        }
        else // 1보다 클때면 좌 우가 검정색으로 나와야 함.
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
