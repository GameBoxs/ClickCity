using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private float Speed = 0.5f; // 1f 로 바꿔야함.
    private Vector2 nowPos, prePos;
    private Vector3 movePos;
    [Tooltip("카메라 오브젝트")]
    public Camera camera;

    private void Start()
    {
        Debug.Log(camera.transform.localPosition);
    }
    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) // 터치 시작할때
            {
                prePos = touch.position - touch.deltaPosition; // 카메라 이전 포지션 x,y,z 저장.
            }
            else if (touch.phase == TouchPhase.Moved) // 터치가 움직였을때
            {
                nowPos = touch.position - touch.deltaPosition; // 움직이고 멈췄을 현재 포지션 저장
                movePos = (Vector3)(prePos - nowPos) * Time.deltaTime * Speed; // 움직인 포지션 = 움직이기전 - 현재 포지션 값

                
                camera.transform.Translate(movePos); // 카메라 포지션 변경
                prePos = touch.position - touch.deltaPosition;
                // 아래는 카메라 영역 지정 해놓은것.
                if (camera.transform.localPosition.x < -15f)
                    camera.transform.localPosition = new Vector3(-15f, camera.transform.localPosition.y, camera.transform.localPosition.z);
                else if(camera.transform.localPosition.x > 35f)
                    camera.transform.localPosition = new Vector3(35f, camera.transform.localPosition.y, camera.transform.localPosition.z);
                if (camera.transform.localPosition.y < -15f)
                    camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, -15f, camera.transform.localPosition.z);
                else if (camera.transform.localPosition.y > 35f)
                    camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, 35f, camera.transform.localPosition.z);
                Debug.Log(camera.transform.localPosition);
            }
        }
        else if(Input.touchCount > 1)
        { }
    }
}
