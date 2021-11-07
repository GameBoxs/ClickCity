using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using TMPro;
using UnityEngine.UI;
using LitJson;
using BackEnd;
using UnityEngine.EventSystems;

public class GameDataManager : MonoBehaviour
{
    public string indate; // 데이터 입력날짜, 서버에서 가져와서 저장.

    public static GameDataManager gamedata = null; // static으로 GameDataManager 클래스의 객체 gamedata를 생성( 싱글톤 )
    public BigInteger money; // 가지고 있는 돈
    BigInteger[] printmoney = new BigInteger[26]; // 단위로 바꿔서 UI에 표시할 돈 변수
    public BigInteger clickmoney; // 터치당 돈
    BigInteger[] printclickmoney = new BigInteger[26]; // 단위로 바꿔서 UI에 표시할 돈 변수
    public BigInteger timemoney; // 초당 돈
    BigInteger[] printtimemoney = new BigInteger[26]; // 단위로 바꿔서 UI에 표시할 돈 변수
    public float police; // 치안율
    public float medic; // 보건율
    //------건물 업그레이드 비용, 클릭, 초당 차트에서 불러온 내역 저장------
    //시청
    public BigInteger[] CityHallPrice = new BigInteger[26];
    public BigInteger[] CityHallPerClick = new BigInteger[26];
    public BigInteger[] CityHallPerTime = new BigInteger[26];
    public int CityHallLevel;
    public Text CityHallPerMoney;
    public Text cityhallLeveltext;
    //----------------------------------------------------------------------
    public TextMeshProUGUI moneyui;
    public TextMeshProUGUI clickmoneyui;
    public TextMeshProUGUI timemoneyui;
    public TextMeshProUGUI policeui;
    public TextMeshProUGUI medicui;

    

    //-------------------------------차트-----------------------------------
    BackendReturnObject cityhallChart;

    string[] units = new string[26] // 단위 A~Z가 들어가있는 string 배열.
    {
        "A","B","C","D","E","F","G","H","I","J",
        "K","L","M","N","O","P","Q","R","S","T",
        "U","V","W","X","Y","Z"
    };
    void UpdateMoney()
    {
        JsonData cityrows = cityhallChart.GetReturnValuetoJSON()["rows"];
        BigInteger temp = BigInteger.Parse(cityrows[CityHallLevel - 1]["TimePerMoney"][0].ToString());
        CityHallPerTime[0] = temp;
        printmoney[0] = money;
        printclickmoney[0] = clickmoney;
        printtimemoney[0] = timemoney;
        for(int i=0; i<25; i++)
        {
            if(printmoney[i]>999)
            {
                printmoney[i + 1] = printmoney[i] / 1000;
                printmoney[i] -= printmoney[i + 1] * 1000;
            }
            if (printclickmoney[i] > 999)
            {
                printclickmoney[i + 1] = printclickmoney[i] / 1000;
                printclickmoney[i] -= printclickmoney[i + 1] * 1000;
            }
            if (printtimemoney[i] > 999)
            {
                printtimemoney[i + 1] = printtimemoney[i] / 1000;
                printtimemoney[i] -= printtimemoney[i + 1] * 1000;
            }
            if(CityHallPerTime[i] > 999)
            {
                CityHallPerTime[i + 1] = CityHallPerTime[i] / 1000;
                CityHallPerTime[i] -= CityHallPerTime[i + 1] * 1000;
            }
        }
        moneyui.text = ChangeMoneyToString(printmoney);
        clickmoneyui.text = ChangeMoneyToString(printclickmoney);
        timemoneyui.text = ChangeMoneyToString(printtimemoney);
        CityHallPerMoney.text = ChangeMoneyToString(CityHallPerTime);
    }
    string ChangeMoneyToString(BigInteger[] value)
    {
        string result="";
        int maxindex = 0;
        for(int i=25; i>=0; i--)
        {
            if(value[i] > 0 && i > 0)
            {
                maxindex = i;
                result = value[i].ToString() + "." + value[i - 1].ToString() +" "+units[i];
                break;
            }
            else if(value[i] > 0 && i == 0)
            {
                result = value[i].ToString();
                break;
            }
            else if(value[i]==0 && i == 0)
            {
                result = "0";
                break;
            }
        }
        return result;
    }
    private void Awake()
    {
        if(gamedata == null) // 만약 gamedata가 null이라면
        {
            gamedata = this;
        }
        else
        {
            if (gamedata != this)
                Destroy(this.gameObject);
        }
    }
    void printDataText()
    {
        cityhallLeveltext.text = "LV " + CityHallLevel.ToString();
    }
    void clickPerMoney()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) // 터치한곳이 UI가 아니라면
            {
                if(touch.phase == TouchPhase.Ended)
                    money += clickmoney;
            }
        }
        else if (Input.GetMouseButtonDown(0)) // 마우스 클릭했을때(컴퓨터에서 테스트하기 위해)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                money += clickmoney;
            }
        }
    }
    IEnumerator startTimePerMoney()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            money += timemoney;
        }
    }
    IEnumerator autoSave()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(60f);
            saveData();
        }
    }
    void saveData()
    {
        Param param = new Param(); // Param은 서버 저장 하는 변수
        param.Add("Money", money.ToString());
        param.Add("ClickMoney", clickmoney.ToString());
        param.Add("TimePerMoney", timemoney.ToString());
        param.Add("Police", police.ToString());
        param.Add("Medic", medic.ToString());
        Backend.GameData.Update("UserInfo", indate, param);
        param.Clear();
    }
    // Start is called before the first frame update
    void Start()
    {
        cityhallChart = Backend.Chart.GetChartContents("25093");
        StartCoroutine(startTimePerMoney());
        StartCoroutine(autoSave());
    }

    // Update is called once per frame
    void Update()
    {
        printDataText(); // 메뉴에 개발쪽 건물 레벨 텍스트 변경
        UpdateMoney(); // 돈 증가 텍스트 update로 변경 확인 및 변경.
        clickPerMoney(); // 터치당 돈 증가.
    }

    private void OnApplicationQuit()
    {
        saveData();
    }
}
