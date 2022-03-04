using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
public class Upgrade : MonoBehaviour
{
    [SerializeField]
    public string buildingName;
    public Text buildingPriceText;

    public Text PerMoney; // police, hospital에만 상점에서 Lv 밑에 텍스트 넣어주기

    BigInteger Price;
    BigInteger[] priceArray = new BigInteger[26];
    BigInteger[] changedPrice = new BigInteger[26];
    BigInteger perMoney;
    BigInteger[] perArray = new BigInteger[26];
    BigInteger[] changedPerMoney = new BigInteger[26];

    //LevelManager lvm = new LevelManager();

    private void LoadData()
    {
        if(buildingName == "CityHall")
        {
            Price = GameDataManager.gamedata.CityHallPrice[GameDataManager.gamedata.CityHallLevel];
            priceArray[0] = Price;
            changedPrice = GameDataManager.gamedata.UpdateMoney2(priceArray);
            buildingPriceText.text = GameDataManager.gamedata.ChangeMoneyToString(changedPrice);
        }
        if(buildingName == "Police")
        {
            Price = GameDataManager.gamedata.PolicePrice[GameDataManager.gamedata.PoliceLevel];
            priceArray[0] = Price;
            changedPrice = GameDataManager.gamedata.UpdateMoney2(priceArray);
            buildingPriceText.text = GameDataManager.gamedata.ChangeMoneyToString(changedPrice);

            if(GameDataManager.gamedata.PoliceLevel == 0)
            {
                PerMoney.text = "0";
            }
            else
            {
                perMoney = GameDataManager.gamedata.PolicePerTimeChart[GameDataManager.gamedata.PoliceLevel - 1];
                perArray[0] = perMoney;
                changedPerMoney = GameDataManager.gamedata.UpdateMoney2(perArray);
                PerMoney.text = GameDataManager.gamedata.ChangeMoneyToString(changedPerMoney);
            }
        }
        if(buildingName == "Hospital")
        {
            Price = GameDataManager.gamedata.HospitalPrice[GameDataManager.gamedata.HospitalLevel];
            priceArray[0] = Price;
            changedPrice = GameDataManager.gamedata.UpdateMoney2(priceArray);
            buildingPriceText.text = GameDataManager.gamedata.ChangeMoneyToString(changedPrice);

            if(GameDataManager.gamedata.HospitalLevel == 0)
            {
                PerMoney.text = "0";
            }
            else
            {
                perMoney = GameDataManager.gamedata.HospitalPerTimeChart[GameDataManager.gamedata.HospitalLevel - 1];
                perArray[0] = perMoney;
                changedPerMoney = GameDataManager.gamedata.UpdateMoney2(perArray);
                PerMoney.text = GameDataManager.gamedata.ChangeMoneyToString(changedPerMoney);
            }
        }
    }
    void BuildingCheck()
    {
        GameObject building;
        int level = GameDataManager.gamedata.CityHallLevel;
        if (level % 5 == 0)
        {
            switch (level)
            {
                case 5:
                    building = GameObject.Find("Five");
                    for (int i = 0; i < building.transform.childCount; i++)
                    {
                        building.transform.GetChild(i).gameObject.SetActive(true);
                        //building.transform.GetChild(i).GetComponent<MainObjectGrowUp>().enabled = true;
                    }
                        
                    //StartCoroutine(Growup(building));
                    break;
                case 10:
                    building = GameObject.Find("Ten");
                    StartCoroutine(Growup(building));
                    break;
                case 15:
                    building = GameObject.Find("Fifteen");
                    StartCoroutine(Growup(building));
                    break;
                case 20:
                    building = GameObject.Find("Twenty");
                    StartCoroutine(Growup(building));
                    break;
                case 25:
                    building = GameObject.Find("Twenty_Five");
                    StartCoroutine(Growup(building));
                    break;
                case 30:
                    building = GameObject.Find("Thirty");
                    StartCoroutine(Growup(building));
                    break;
                case 35:
                    building = GameObject.Find("Thirty_Five");
                    StartCoroutine(Growup(building));
                    break;
                case 40:
                    building = GameObject.Find("Forty");
                    StartCoroutine(Growup(building));
                    break;
                case 45:
                    building = GameObject.Find("Forty_Five");
                    StartCoroutine(Growup(building));
                    break;
                case 50:
                    building = GameObject.Find("Fifty");
                    StartCoroutine(Growup(building));
                    break;
                case 55:
                    building = GameObject.Find("Fifty_Five");
                    StartCoroutine(Growup(building));
                    break;
                case 60:
                    building = GameObject.Find("Sixty");
                    StartCoroutine(Growup(building));
                    break;
                case 65:
                    building = GameObject.Find("Sixty_Five");
                    StartCoroutine(Growup(building));
                    break;
                case 70:
                    building = GameObject.Find("Seventy");
                    StartCoroutine(Growup(building));
                    break;
                case 75:
                    building = GameObject.Find("Seventy_Five");
                    StartCoroutine(Growup(building));
                    break;
                case 80:
                    building = GameObject.Find("Eighty");
                    StartCoroutine(Growup(building));
                    break;
                case 85:
                    building = GameObject.Find("Eighty_Five");
                    StartCoroutine(Growup(building));
                    break;
                case 90:
                    building = GameObject.Find("Ninety");
                    StartCoroutine(Growup(building));
                    break;
                case 95:
                    building = GameObject.Find("Ninety_Five");
                    StartCoroutine(Growup(building));
                    break;
                case 100:
                    building = GameObject.Find("Hundred");
                    StartCoroutine(Growup(building));
                    break;
            }
        }
    }
    IEnumerator Growup(GameObject building)
    {
        float max = 1.6f, average = 1f, x = 0f;
        while (x <= max)
        {
            building.gameObject.transform.localScale = new UnityEngine.Vector3(1, x, x);
            x += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        while (x >= average)
        {
            building.gameObject.transform.localScale = new UnityEngine.Vector3(1, x, x);
            x -= 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    public void UpgradeBuilding()
    {
        if(GameDataManager.gamedata.money >= Price && buildingName == "CityHall")
        {
            if (GameDataManager.gamedata.CityHallLevel == 1)
            {
                GameDataManager.gamedata.timemoney += 1;
                GameDataManager.gamedata.clickmoney += 1;
            }
            else
            {
                GameDataManager.gamedata.timemoney += (GameDataManager.gamedata.CityHallPerTimeChart[GameDataManager.gamedata.CityHallLevel] - GameDataManager.gamedata.CityHallPerTimeChart[GameDataManager.gamedata.CityHallLevel - 1]);
                GameDataManager.gamedata.clickmoney += (GameDataManager.gamedata.CityHallPerClickChart[GameDataManager.gamedata.CityHallLevel] - GameDataManager.gamedata.CityHallPerClickChart[GameDataManager.gamedata.CityHallLevel - 1]);
            }
            GameDataManager.gamedata.money -= Price;
            GameDataManager.gamedata.CityHallLevel += 1;
            BuildingCheck();
        }
        else if(GameDataManager.gamedata.money >= Price && buildingName == "Police")
        {
            if (GameDataManager.gamedata.PoliceLevel == 0)
            {
                GameDataManager.gamedata.timemoney += 10;
                GameDataManager.gamedata.clickmoney += 15;
            }
            else
            {
                GameDataManager.gamedata.timemoney += (GameDataManager.gamedata.PolicePerTimeChart[GameDataManager.gamedata.PoliceLevel] - GameDataManager.gamedata.PolicePerTimeChart[GameDataManager.gamedata.PoliceLevel - 1]);
                GameDataManager.gamedata.clickmoney += (GameDataManager.gamedata.PolicePerClickChart[GameDataManager.gamedata.PoliceLevel] - GameDataManager.gamedata.PolicePerClickChart[GameDataManager.gamedata.PoliceLevel - 1]);
            }
            GameDataManager.gamedata.money -= Price;
            GameDataManager.gamedata.PoliceLevel += 1;
        }
        else if (GameDataManager.gamedata.money >= Price && buildingName == "Hospital")
        {
            if (GameDataManager.gamedata.HospitalLevel == 0)
            {
                GameDataManager.gamedata.timemoney += 50;
                GameDataManager.gamedata.clickmoney += 55;
            }
            else
            {
                GameDataManager.gamedata.timemoney += (GameDataManager.gamedata.HospitalPerTimeChart[GameDataManager.gamedata.HospitalLevel] - GameDataManager.gamedata.CityHallPerTimeChart[GameDataManager.gamedata.HospitalLevel - 1]);
                GameDataManager.gamedata.clickmoney += (GameDataManager.gamedata.HospitalPerClickChart[GameDataManager.gamedata.HospitalLevel] - GameDataManager.gamedata.CityHallPerClickChart[GameDataManager.gamedata.HospitalLevel - 1]);
            }
            GameDataManager.gamedata.money -= Price;
            GameDataManager.gamedata.HospitalLevel += 1;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(buildingName+" "+name);
    }

    // Update is called once per frame
    void Update()
    {
        LoadData();
    }
}
