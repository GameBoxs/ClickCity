using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadingSceneController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup panle;
    [SerializeField]
    private Image progressBar;

    private string loadSceneName;
    public static string lname;
    void Start()
    {
        LoadScene(lname);
    }
    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);

        
        SceneManager.sceneLoaded += OnSceneLoaded; //SceneManager.sceneLoaded 을 통해 유니티에서는 로딩씬이 끝나는 시점을 받을 수 있음
        //OnSceneLoaded 를 입력하고 ctrl+. 을 눌러 메서드를 생성해주면 아래 OnSceneLoaded 메서드가 자동으로 만들어짐
        

        loadSceneName = sceneName;
        StartCoroutine(LoadSceneProcess()); // 씬 불러와서 진행시킬 코루틴 작성.

    }
    private IEnumerator LoadSceneProcess()
    {
        progressBar.fillAmount = 0f;
        yield return StartCoroutine(Fade()); // Fade 메서드를 불러 실행하고 그동안 아랫줄은 멈춤, 끝나야 다음 줄로 넘어감.

        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName); // 비동기 방식으로 loadSceneName의 씬을 호출
        op.allowSceneActivation = false; // 씬을 다 부르면 자동으로 씬 전환하는것을 false로 함.

        float timer = 0f;
        while(!op.isDone) // 씬 로딩이 끝나지 않을때
        {
            yield return null; // 유니티에 제어권을 넘김
            if(op.progress < 0.9f) // 진행도가 0.9보다 낮을때
            {
                progressBar.fillAmount = op.progress;
            }
            else // 진행도가 0.9 즉 90%넘겼을때 페이크 로딩을 만듬.
            {
                timer += Time.unscaledDeltaTime; // time scale에 의존하지 않는 델타 타임을 더함.
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer); // 선형으로 
                if(progressBar.fillAmount >= 1f)
                {
                    yield return new WaitForSecondsRealtime(0.2f);
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
    private IEnumerator Fade() // 패널의 알파값 조절함
    {
        float a = 1f;
        panle.alpha = a;
        yield return new WaitForSecondsRealtime(0.5f); // 0.3초간 대기

        while(a >= 0)
        {
            panle.alpha = a;
            a -= 0.07f;
            yield return new WaitForSecondsRealtime(0.1f); // yield return null 함으로 유니티에 제어권을 넘김.
        }
        yield return new WaitForSecondsRealtime(0.3f);
    }
    
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(arg0.name == loadSceneName)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded; // 콜백을 제거하는 이유는 씬 로딩될때 등록한 콜백이 중첩되서 문제가 발생하는것을 막기 위함.
        }
    }
    
}
