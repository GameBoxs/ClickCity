using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using TMPro;
public class GameDataManager : MonoBehaviour
{
    public static GameDataManager gamedata = null; // static으로 GameDataManager 클래스의 객체 gamedata를 생성( 싱글톤 )
    public BigInteger money; // 가지고 있는 돈
    BigInteger[] printmoney = new BigInteger[26]; // 단위로 바꿔서 UI에 표시할 돈 변수
    public BigInteger clickmoney; // 터치당 돈
    BigInteger[] printclickmoney = new BigInteger[26]; // 단위로 바꿔서 UI에 표시할 돈 변수
    public BigInteger timemoney; // 초당 돈
    BigInteger[] printtimemoney = new BigInteger[26]; // 단위로 바꿔서 UI에 표시할 돈 변수
    float police; // 치안율
    float medic; // 보건율
    //------건물 업그레이드 비용, 클릭, 초당 차트에서 불러온 내역 저장------
    //시청
    public BigInteger[] CityHallPrice = new BigInteger[100];
    public BigInteger[] CityHallPerClick = new BigInteger[100];
    public BigInteger[] CityHallPerTime = new BigInteger[100];
    //----------------------------------------------------------------------
    public TextMeshProUGUI moneyui;
    public TextMeshProUGUI clickmoneyui;
    public TextMeshProUGUI timemoneyui;
    public TextMeshProUGUI policeui;
    public TextMeshProUGUI medicui;

    string[] units = new string[26] // 단위 A~Z가 들어가있는 string 배열.
    {
        "A","B","C","D","E","F","G","H","I","J",
        "K","L","M","N","O","P","Q","R","S","T",
        "U","V","W","X","Y","Z"
    };
    void UpdateMoney()
    {
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
        }
        moneyui.text = ChangeMoneyToString(printmoney);
        clickmoneyui.text = ChangeMoneyToString(printclickmoney);
        timemoneyui.text = ChangeMoneyToString(printtimemoney);
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoney();
    }
}
