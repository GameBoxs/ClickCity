using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrowUp : MonoBehaviour
{
    public float delay=0.05f;
    List<Transform> childs;

    // Use this for initialization
    void Start()
    {

        childs = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            childs.Add(transform.GetChild(i));
            transform.GetChild(i).gameObject.transform.localScale = new Vector3(0, 0, 0);
        }

        StartCoroutine(Enable());
    }

    IEnumerator Enable()
    {
        int i = 0, iterations = Mathf.RoundToInt(delay / Time.deltaTime) * 200;
        float max = 1.6f, average = 1f, x = 0f;
        if (iterations == 0)
            iterations = 1;
        while (childs.Count > 0)
        {
            yield return new WaitForSeconds(delay);
            for (int a = 0; a < iterations; a++)
            {
                if (childs.Count == 0)
                    yield break;
                i = Random.Range(0, childs.Count);
                while (x <= max)
                {
                    childs[i].gameObject.transform.localScale = new Vector3(x, x, x);
                    x += 0.1f;
                    yield return new WaitForSeconds(0.01f);
                }
                while(x >= average)
                {
                    childs[i].gameObject.transform.localScale = new Vector3(x, x, x);
                    x -= 0.1f;
                    yield return new WaitForSeconds(0.01f);
                }
                childs.RemoveAt(i);
                Random.InitState(a);
            }
        }
    }
}
