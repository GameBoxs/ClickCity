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
        // 회원가입한뒤 인풋 필드 이메일의 string대로 뒤끝의 서버로 전송후 결과를 받아옴
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
    public void OnClickLogin() // 구글이 아닌 커스텀 계정 로그인 버튼 누를시
    {
        //인풋필드 id, pw를 뒤끝 서버로 전송하여 로그인 결과를 받아옴
        BackendReturnObject bro = Backend.BMember.CustomLogin(customid.text, custompw.text);
        if (bro.IsSuccess())
        {
            Debug.Log("로그인에 성공했습니다");
            LoadingSceneController.lname = "MainScene";
            SceneManager.LoadScene("LoadingScene");
        }
        else //로그인 실패시 디버그 로그 출력.
        {
            Debug.Log(bro.GetStatusCode().ToString());
            Debug.Log(bro.GetErrorCode().ToString());
            Debug.Log(bro.GetMessage().ToString());
        }
    }
}
