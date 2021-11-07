using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class BackEndAuthentication : MonoBehaviour
{
    public InputField customid;
    public InputField custompw;
    public InputField customemail;
    public InputField customidsign;
    public InputField custompwsign;
    // 회원가입1 - 동기 방식
    void SignUpEmail()
    {
        string emailerror = Backend.BMember.UpdateCustomEmail(customemail.text).GetMessage();
        switch (emailerror)
        {
            case "Success":
                Debug.Log("이메일 등록 완료");
                break;
            default:
                Debug.Log("에러");
                break;
        }
    }
    public void OnClickSignUp()
    {
        // 회원 가입을 한뒤 결과를 BackEndReturnObject 타입으로 반환한다.
        string signdate = "가입일: " + DateTime.Now.ToString("yyyy-MM-dd");
        string error = Backend.BMember.CustomSignUp(customidsign.text, custompwsign.text, signdate).GetErrorCode();
        

        // 회원 가입 실패 처리
        switch (error)
        {
            case "DuplicatedParameterException":
                Debug.Log("중복된 customId 가 존재");
                break;

            default:
                Debug.Log("회원 가입 완료");
                SignUpEmail();
                break;
        }
        Debug.Log("동기 방식============================================= ");

    }
    public void OnClickLogin()
    {
        BackendReturnObject bro = Backend.BMember.CustomLogin(customid.text, custompw.text);
        if (bro.IsSuccess())
        {
            Debug.Log("로그인에 성공했습니다");
            LoadingSceneController.lname = "MainScene";
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            Debug.Log(bro.GetStatusCode().ToString());
            Debug.Log(bro.GetErrorCode().ToString());
            Debug.Log(bro.GetMessage().ToString());
        }
        /*
        string error = Backend.BMember.CustomLogin(customid.text, custompw.text).GetErrorCode();
        Debug.Log("아이디:" + customid.text);
        Debug.Log("비번:" + custompw.text);
        // 로그인 실패 처리
        switch (error)
        {
            // 아이디 또는 비밀번호가 틀렸을 경우
            case "BadUnauthorizedException":
                Debug.Log("아이디 또는 비밀번호가 틀렸다.");
                break;


            case "BadPlayer":  //  이 경우 콘솔에서 입력한 차단된 사유가 에러코드가 된다.
                Debug.Log("차단된 유저");
                break;

            default:
                Debug.Log("로그인 완료");
                LoadingSceneController.lname = "MainScene";
                SceneManager.LoadScene("LoadingScene");
                //SceneManager.LoadScene("MainScene");
                break;
        }
        Debug.Log("동기 방식============================================= ");
        */
    }
}
