using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System.Numerics;
using System;
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
                GameDataManager.gamedata.policeui.text = rows[0]["Police"][0].ToString() + " %";
                GameDataManager.gamedata.medicui.text = rows[0]["Medic"][0].ToString() + " %";
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
        string TimePerMoney = "0"; // 초당 돈 0원
        string Money = "0"; // 가지고 있는 돈 0원
        float Police = 100f; // 치안율 0.0
        float Medic = 100f; // 보건율 0.0
        Dictionary<string, int> BuildingLevel = new Dictionary<string, int> // 빌딩들의 레벨은 딕셔너리로 저장함.
        {
            {"CityHall", 1 },
            {"Bank", 1 },
            {"PoliceStation", 1 },
            {"Hospital", 1 },
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
    void Start()
    {
        CheckUserData();
    }
    public void OnclickLogout()
    {
        Backend.BMember.Logout();
        Debug.Log("로그아웃 되었음");
    }
}
