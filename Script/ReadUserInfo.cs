using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System.Numerics;
using System;
using LitJson;

public class ReadUserInfo : MonoBehaviour
{
    void CheckUserData() // 서버 유저 데이터 체크 하는 함수
    {
        var UserInfo = Backend.GameData.GetMyData("UserInfo", new Where(), 10); // 나의 정보를 불러오는데 UserInfo 테이블에서 조건없이 10개 로우를 검색함.
        if(UserInfo.IsSuccess()) // 성공했다면
        {
            if (UserInfo.GetReturnValuetoJSON()["rows"].Count <= 0) // 만약 rows의 카운트 갯수가 0이하라면 데이터가 없기에 유저 정보를 서버에 삽입하는 함수 실행.
            {
                InitalizeUser();
            }
            else
            {
                var rows = UserInfo.GetReturnValuetoJSON()["rows"]; // json밸류 rows에 대한 정보를 var rows 변수에 저장.
                GameDataManager.gamedata.money = BigInteger.Parse(rows[0]["Money"][0].ToString());
                GameDataManager.gamedata.clickmoney = BigInteger.Parse(rows[0]["ClickMoney"][0].ToString());
                GameDataManager.gamedata.timemoney = BigInteger.Parse(rows[0]["TimePerMoney"][0].ToString());
                GameDataManager.gamedata.police = float.Parse(rows[0]["Police"][0].ToString());
                GameDataManager.gamedata.medic = float.Parse(rows[0]["Medic"][0].ToString());
                GameDataManager.gamedata.policeui.text = rows[0]["Police"][0].ToString() + " %";
                GameDataManager.gamedata.medicui.text = rows[0]["Medic"][0].ToString() + " %";

                GameDataManager.gamedata.CityHallLevel = int.Parse(rows[0]["BuildingLevel"][0]["CityHall"][0].ToString());
                GameDataManager.gamedata.PoliceLevel = int.Parse(rows[0]["BuildingLevel"][0]["PoliceStation"][0].ToString());
                GameDataManager.gamedata.HospitalLevel = int.Parse(rows[0]["BuildingLevel"][0]["Hospital"][0].ToString());

                GameDataManager.gamedata.indate = rows[0]["inDate"][0].ToString();

                Debug.Log(rows[0]["BuildingLevel"][0]["CityHall"][0].ToString()); // rows의 0번째 테이블중 BuildingLevel 컬럼의 0번째 중 cityhall의 0번째 값을 출력.
                //만약 clickMoney를 찾을려면 rows[0]["Money][0].ToString(); 하면 됨.
            }
        }
        else // 실패했다면
        {
            Debug.Log("서버 공통 에러 발생: " + UserInfo.GetMessage()); // 실패 메세지를 더해서 출력해줌.
        }
    }
    void InitalizeUser() // UserInfo테이블에 데이터가 없는 신규 유저일시 정보를 생성해주는 함수.
    {
        string ClickMoney = "1"; // 터치당 돈 1원
        string TimePerMoney = "1"; // 초당 돈 0원
        string Money = "0"; // 가지고 있는 돈 0원
        float Police = 100f; // 치안율 0.0
        float Medic = 100f; // 보건율 0.0
        Dictionary<string, int> BuildingLevel = new Dictionary<string, int> // 빌딩들의 레벨은 딕셔너리로 저장함.
        {
            {"CityHall", 1 },
            {"Bank", 1 },
            {"PoliceStation", 0 },
            {"Hospital", 0 },
            {"FireStation", 1 },
            {"Skyscraper", 1 },
            {"Factory", 1 },
            {"Airport", 1 },
            {"TrainStation", 1 },
            {"Port", 1 }
        };
        Param param = new Param(); // Param은 서버 저장 하는 변수
        param.Add("ClickMoney", ClickMoney); // ClickMoney컬럼에 ClickMoney변수의 값을 레코드로 저장.
        param.Add("TimePerMoney", TimePerMoney);
        param.Add("Money", Money);
        param.Add("Police", Police);
        param.Add("Medic", Medic);
        param.Add("BuildingLevel", BuildingLevel);

        BackendReturnObject InitialUser = Backend.GameData.Insert("UserInfo", param); // Backend.GameData.Insert("UserInfo", param);은 UserInfo테이블에 param을 생성시킴.
        if (InitialUser.IsSuccess())
        {
            Debug.Log("유저데이터 서버 생성완료.");
        }
        else
        {
            switch (InitialUser.GetStatusCode())
            {
                case "404":
                    Debug.Log("존재하지 않는 tableName인 경우");
                    break;

                case "412":
                    Debug.Log("비활성화 된 tableName인 경우");
                    break;

                case "413":
                    Debug.Log("하나의 row( column들의 집합 )이 400KB를 넘는 경우");
                    break;

                default:
                    Debug.Log("서버 공통 에러 발생: " + InitialUser.GetMessage());
                    break;
            }
        }
    }
    void ReadBuilding() // 차트에서 내역 불러오기.
    {
        BackendReturnObject ChartCityHall = Backend.Chart.GetChartContents("25093");
        BackendReturnObject ChartPolice = Backend.Chart.GetChartContents("32669");
        BackendReturnObject ChartHospital = Backend.Chart.GetChartContents("32670");
        if (ChartCityHall.IsSuccess())
        {
            JsonData rows = ChartCityHall.GetReturnValuetoJSON()["rows"];
            for(int i=0;i<rows.Count;i++)
            {
                GameDataManager.gamedata.CityHallPrice[i] = BigInteger.Parse(rows[i]["Price"][0].ToString());
                GameDataManager.gamedata.CityHallPerClickChart[i] = BigInteger.Parse(rows[i]["ClickPerMoney"][0].ToString());
                GameDataManager.gamedata.CityHallPerTimeChart[i] = BigInteger.Parse(rows[i]["TimePerMoney"][0].ToString());
            }
        }
        else
        {
            switch (ChartCityHall.GetStatusCode())
            {

                case "400":
                    Debug.Log("올바르지 못한 { uuid | id } 를 입력");
                    break;

                default:
                    Debug.Log("서버 공통 에러 발생: " + ChartCityHall.GetMessage());
                    break;
            }
        }
        if (ChartPolice.IsSuccess())
        {
            JsonData rows = ChartPolice.GetReturnValuetoJSON()["rows"];
            for (int i = 0; i < rows.Count; i++)
            {
                GameDataManager.gamedata.PolicePrice[i] = BigInteger.Parse(rows[i]["Price"][0].ToString());
                GameDataManager.gamedata.PolicePerClickChart[i] = BigInteger.Parse(rows[i]["ClickPerMoney"][0].ToString());
                GameDataManager.gamedata.PolicePerTimeChart[i] = BigInteger.Parse(rows[i]["TimePerMoney"][0].ToString());
            }
        }
        if (ChartHospital.IsSuccess())
        {
            JsonData rows = ChartHospital.GetReturnValuetoJSON()["rows"];
            for (int i = 0; i < rows.Count; i++)
            {
                GameDataManager.gamedata.HospitalPrice[i] = BigInteger.Parse(rows[i]["Price"][0].ToString());
                GameDataManager.gamedata.HospitalPerClickChart[i] = BigInteger.Parse(rows[i]["ClickPerMoney"][0].ToString());
                GameDataManager.gamedata.HospitalPerTimeChart[i] = BigInteger.Parse(rows[i]["TimePerMoney"][0].ToString());
            }
        }
    }
    void Start()
    {
        CheckUserData();
        ReadBuilding();
    }
    public void OnclickLogout()
    {
        Backend.BMember.Logout();
        Debug.Log("로그아웃 되었음");
    }
}
