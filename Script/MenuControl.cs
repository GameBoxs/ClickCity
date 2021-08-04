using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    public bool IsOpen = false;
    public bool OpenStop = false;
    Vector3 closeposition = new Vector3(868, -481, 0);
    Vector3 openposition = new Vector3(55, -481, 0);
    Vector3 nowposition;
    public void CloseAndOpen()
    {
        if(OpenStop == false) // 만약 Update문에서 사용하는 bool타입이 false라면
            OpenStop = true; // true 로 바꾸어줌
        else // 만약 Update에서 사용중일때 누른다면
        {
            IsOpen = !IsOpen; // IsOpen 변수에 IsOpen현재값의 반대 값을 넣어줌.
        }
    }
    private void Update()
    {
        if(OpenStop == true) // True가 바뀌면 밑의 구문들 실행함, lerp(선형보간) 사용을 위해 Update에 사용함.
        {
            nowposition = this.transform.localPosition; // 현재 포지션은 this(현재 스크립트가 적용된 오브젝트)의 로컬 포지션을 저장함.
            if (IsOpen == false) // 만약 IsOpen이 flase라면
            {
                this.transform.localPosition = Vector3.Lerp(nowposition, openposition, 0.05f); // 선형보간을 사용하여 현재위치에서 openposition위치로 이동함.
                if(Mathf.Approximately(this.transform.localPosition.x, openposition.x)) 
                // Mathf.Approximately를 사용한 이유는 선형보간 이동시 정해진 위치에 정확하게 이동되는것이 아니라 부동소수점처럼 0.00001처럼 오차 범위가 있기에
                // 오차 범위를 검사하기 위해 유니티에서 제공하는 함수임.
                {
                    IsOpen = true;
                    OpenStop = false;
                }
                /* Mathf.Approximately를 몰랐을때 사용했던 잘못된 방식.
                if (this.transform.localPosition.x <= openposition.x)
                {
                    IsOpen = true;
                    OpenStop = false;
                }*/
            }
            else
            {
                this.transform.localPosition = Vector3.Lerp(nowposition, closeposition, 0.05f);
                if (Mathf.Approximately(this.transform.localPosition.x, closeposition.x))
                {
                    IsOpen = false;
                    OpenStop = false;
                }
            }
        }
    }
}
