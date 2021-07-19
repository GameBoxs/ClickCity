using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
public class ReadUserInfo : MonoBehaviour
{
    void InitalizeUserInfo()
    {
        Backend.BMember.GetUserInfo((callback) =>
        {
            string nickname = callback.GetReturnValuetoJSON()["row"]["gamerId"].ToString();
            Debug.Log(nickname);
        });
    }
    void Start()
    {
        InitalizeUserInfo();
    }
    public void OnclickLogout()
    {
        Backend.BMember.Logout();
        Debug.Log("로그아웃 되었음");
        InitalizeUserInfo();
    }
}
