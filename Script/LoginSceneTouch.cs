using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSceneTouch : MonoBehaviour
{
    bool IsTouch = false;
    bool corutinstart = false;
    public CanvasGroup Text; // TouchToStart 오브젝트 넣기.
    public GameObject TouchToStart;
    public GameObject Login;
    IEnumerator alphashowhide()
    {
        corutinstart = true;
        if(IsTouch == false)
        {
            yield return StartCoroutine(alphaup());
            yield return StartCoroutine(alphadown());
        }
    }
    IEnumerator alphaup()
    {
        while(Text.alpha < 1.0f)
        {
            Text.alpha += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator alphadown()
    {
        while (Text.alpha > 0f)
        {
            Text.alpha -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        corutinstart = false;
    }
    
    void Update()
    {
        if(IsTouch==false&&corutinstart==false)
        {
            StartCoroutine(alphashowhide());
        }
        if (Input.touchCount == 1 || Input.GetMouseButtonDown(0))
        {
            IsTouch = true;
            TouchToStart.SetActive(false);
            Login.SetActive(true);
        }
    }
}
