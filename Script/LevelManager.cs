using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    GameObject building;

    public void CityHallBuildingLevelCheck()
    {
        int level = GameDataManager.gamedata.CityHallLevel;
        if (level % 5 == 0)
        {
            switch(level)
            {
                case 5:
                    building = GameObject.Find("Five");
                    StartCoroutine(Growup());
                    break;
                case 10:
                    building = GameObject.Find("Ten");
                    StartCoroutine(Growup());
                    break;
                case 15:
                    building = GameObject.Find("Fifteen");
                    StartCoroutine(Growup());
                    break;
                case 20:
                    building = GameObject.Find("Twenty");
                    StartCoroutine(Growup());
                    break;
                case 25:
                    building = GameObject.Find("Twenty_Five");
                    StartCoroutine(Growup());
                    break;
                case 30:
                    building = GameObject.Find("Thirty");
                    StartCoroutine(Growup());
                    break;
                case 35:
                    building = GameObject.Find("Thirty_Five");
                    StartCoroutine(Growup());
                    break;
                case 40:
                    building = GameObject.Find("Forty");
                    StartCoroutine(Growup());
                    break;
                case 45:
                    building = GameObject.Find("Forty_Five");
                    StartCoroutine(Growup());
                    break;
                case 50:
                    building = GameObject.Find("Fifty");
                    StartCoroutine(Growup());
                    break;
                case 55:
                    building = GameObject.Find("Fifty_Five");
                    StartCoroutine(Growup());
                    break;
                case 60:
                    building = GameObject.Find("Sixty");
                    StartCoroutine(Growup());
                    break;
                case 65:
                    building = GameObject.Find("Sixty_Five");
                    StartCoroutine(Growup());
                    break;
                case 70:
                    building = GameObject.Find("Seventy");
                    StartCoroutine(Growup());
                    break;
                case 75:
                    building = GameObject.Find("Seventy_Five");
                    StartCoroutine(Growup());
                    break;
                case 80:
                    building = GameObject.Find("Eighty");
                    StartCoroutine(Growup());
                    break;
                case 85:
                    building = GameObject.Find("Eighty_Five");
                    StartCoroutine(Growup());
                    break;
                case 90:
                    building = GameObject.Find("Ninety");
                    StartCoroutine(Growup());
                    break;
                case 95:
                    building = GameObject.Find("Ninety_Five");
                    StartCoroutine(Growup());
                    break;
                case 100:
                    building = GameObject.Find("Hundred");
                    StartCoroutine(Growup());
                    break;
            }
        }
    }
    IEnumerator Growup()
    {
        float max = 1.6f, average = 1f, x = 0f;
        while (x <= max)
        {
            //building.gameObject.transform.localScale = new Vector3(x, x, x);
            x += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        while (x >= average)
        {
            //building.gameObject.transform.localScale = new Vector3(x, x, x);
            x -= 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    /*
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
