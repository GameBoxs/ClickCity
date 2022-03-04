using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainObjectGrowUp : MonoBehaviour
{
    IEnumerator Growup()
    {
        float max = 1.6f, average = 1f, x = 0f;
        while (x <= max)
        {
            gameObject.transform.localScale = new UnityEngine.Vector3(x, x, x);
            x += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        while (x >= average)
        {
            gameObject.transform.localScale = new UnityEngine.Vector3(x, x, x);
            x -= 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    Vector3 max_scale;
    Vector3 avg_scale;
    bool step1 = false;
    bool step2 = false;
    bool step3 = false;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Growup());
        avg_scale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        max_scale = new Vector3(avg_scale.x + (avg_scale.x * 1f), avg_scale.y + (avg_scale.y * 1f), avg_scale.z + (avg_scale.z * 1f));
        gameObject.transform.localScale = Vector3.zero;
        step1 = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(avg_scale.x >= 10f || avg_scale.y >= 10f || avg_scale.z >= 10f)
        {
            if(avg_scale.x >= 500f || avg_scale.y >= 500f || avg_scale.z >= 500f)
            {
                gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, avg_scale, Time.deltaTime * 10);
            }
            else
            {
                if (step1 == true)
                {
                    gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, max_scale, Time.deltaTime * 10);
                    if (Mathf.Round(gameObject.transform.localScale.x) == max_scale.x && Mathf.Round(gameObject.transform.localScale.y) == max_scale.y && Mathf.Round(gameObject.transform.localScale.z) == max_scale.z)
                    {
                        step1 = false;
                        step2 = true;
                    }
                    else if(gameObject.transform.localScale.x-0.1<=max_scale.x&& gameObject.transform.localScale.x + 0.1 >= max_scale.x)
                    {
                        step1 = false;
                        step2 = true;
                    }
                }
                if (step2 == true)
                {
                    gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, avg_scale, Time.deltaTime * 10);
                    if (gameObject.transform.localScale.x - 0.1 <= avg_scale.x && gameObject.transform.localScale.x + 0.1 >= avg_scale.x)
                        step3 = true;
                }
            }
        }
        else
        {
            if (step1 == true)
            {
                gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, max_scale, Time.deltaTime * 4);
                if (Mathf.Round(gameObject.transform.localScale.x) == max_scale.x && Mathf.Round(gameObject.transform.localScale.y) == max_scale.y && Mathf.Round(gameObject.transform.localScale.z) == max_scale.z)
                {
                    step1 = false;
                    step2 = true;
                }
                else if (gameObject.transform.localScale.x - 0.1 <= max_scale.x && gameObject.transform.localScale.x + 0.1 >= max_scale.x)
                {
                    step1 = false;
                    step2 = true;
                }
            }
            if (step2 == true)
            {
                gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, avg_scale, Time.deltaTime * 4);
            }
        }
        if (step2 == true)
        {
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, avg_scale, Time.deltaTime * 10);
            if (gameObject.transform.localScale.x - 0.1 <= avg_scale.x && gameObject.transform.localScale.x + 0.1 >= avg_scale.x)
                step3 = true;
        }
        if (step3 == true)
        {
            gameObject.transform.localScale = new Vector3(avg_scale.x, avg_scale.y, avg_scale.z);
        }
    }
}
