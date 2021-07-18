using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private float Speed = 1f;
    private Vector2 nowPos, prePos;
    private Vector3 movePos;
    [Tooltip("카메라 오브젝트")]
    public Camera camera;
    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                prePos = touch.position - touch.deltaPosition;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                nowPos = touch.position - touch.deltaPosition;
                movePos = (Vector3)(prePos - nowPos) * Time.deltaTime * Speed;
                camera.transform.Translate(movePos);
                prePos = touch.position - touch.deltaPosition;
            }
        }
    }
}
