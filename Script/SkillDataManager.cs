using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BackEnd;
using LitJson;
using UnityEngine.UI;
public class SkillDataManager : MonoBehaviour
{
    public Button TimeButton;
    public Button ClickButton;
    public Button PoliceButton;
    public Button HospitalButton;

    public Image TimeCoolImg;
    public Image ClickCoolImg;
    public Image PoliceCoolImg;
    public Image HospitalCoolImg;

    public static SkillDataManager skillgamedata = null; // static으로 SkillDataManager 클래스의 객체 skillgamedata를 생성( 싱글톤 )

    public Nullable<DateTime> TimePerSkill_Start; // 시간당 돈 증가 스킬 시작 시간 저장
    public Nullable<DateTime> TimePerSkill_Finish; // 시간당 돈 증가 스킬 끝나는 시간 저장
    public Nullable<DateTime> ClickPerSkill_Start; // 터치당 돈 증가 스킬 시작 시간 저장
    public Nullable<DateTime> ClickPerSkill_Finish; // 터치당 돈 증가 스킬 끝나는 시간 저장
    public Nullable<DateTime> PoliceSkill_Start; // 치안율 증가 스킬 시작 시간 저장
    public Nullable<DateTime> PoliceSkill_Finish; // 치안율 증가 스킬 끝나는 시간 저장
    public Nullable<DateTime> HospitalSkill_Start; // 의료율 증가 스킬 시작 시간 저장
    public Nullable<DateTime> HospitalSkill_Finish; // 의료율 증가 스킬 끝나는 시간 저장

    bool Istimeper = false; // 시간당 스킬을 눌렀을때 체크할 bool타입
    bool Isclickper = false; // 터치당 스킬 눌렀을때 체크할 bool 타입
    bool Ispolice = false;
    bool Ishosptal = false;

    public bool Isrunningtime = false;
    public bool Isrunningclick = false;

    double TimeFillAmount = 0.0f;
    double ClickFillAmount = 0.0f;
    double PoliceFillAmount = 0.0f;
    double HospitalFillAmount = 0.0f;

    private string timeindate;

    int time_time=0; // 스킬 유지시간 저장할 int형
    int click_time=0;
    int police_time=0;
    int hospital_time=0;

    int runningtime = 0; // 스킬 유지시간(초당)
    int runningclick = 0; // 스킬 유지시간(터치당)

    private void Awake() // 싱글톤 skillgamedata 중복 생성 방지
    {
        if (skillgamedata == null) // 만약 gamedata가 null이라면
        {
            skillgamedata = this;
        }
        else
        {
            if (skillgamedata != this)
                Destroy(this.gameObject);
        }
    }
    void MakeCoolData()
    {
        DateTime? dt = DateTime.Now;
        Param param = new Param(); // Param은 서버 저장 하는 변수
        param.Add("TimeStart", dt); 
        param.Add("TimeFinish", dt);
        param.Add("ClickStart", dt);
        param.Add("ClickFinish", dt);
        param.Add("PoliceStart", dt);
        param.Add("PoliceFinish", dt);
        param.Add("HospitalStart", dt);
        param.Add("HospitalFinish", dt);
        BackendReturnObject AddCool = Backend.GameData.Insert("SkillCool", param); // Backend.GameData.Insert("UserInfo", param);은 SkillCool테이블에 param을 생성시킴.
    }
    void ReadTime()
    {
        var cool = Backend.GameData.GetMyData("SkillCool", new Where()); // 스킬쿨 테이블을 전부 불러옴(내 아이디만 해당하는 것)
        if(cool.IsSuccess())
        {
            if (cool.GetReturnValuetoJSON()["rows"].Count <= 0) // 불려온 테이블의 row가 0개 이하일때, 즉 없을때
            {
                MakeCoolData(); // 데이터를 만들어줌.
            }
            else
            {
                DateTime? dtnow = DateTime.Now;
                var rows = cool.GetReturnValuetoJSON()["rows"]; // json밸류 rows에 대한 정보를 var rows 변수에 저장.
                TimePerSkill_Start = DateTime.Parse(rows[0]["TimeStart"][0].ToString());
                TimePerSkill_Finish = DateTime.Parse(rows[0]["TimeFinish"][0].ToString());
                ClickPerSkill_Start = DateTime.Parse(rows[0]["ClickStart"][0].ToString());
                ClickPerSkill_Finish = DateTime.Parse(rows[0]["ClickFinish"][0].ToString());
                PoliceSkill_Start = DateTime.Parse(rows[0]["PoliceStart"][0].ToString());
                PoliceSkill_Finish = DateTime.Parse(rows[0]["PoliceFinish"][0].ToString());
                HospitalSkill_Start = DateTime.Parse(rows[0]["HospitalStart"][0].ToString());
                HospitalSkill_Finish = DateTime.Parse(rows[0]["HospitalFinish"][0].ToString());
                timeindate = rows[0]["inDate"][0].ToString();

                TimeFillAmount = Convert.ToDouble(rows[0]["TimeFillAmount"][0].ToString());
                ClickFillAmount = Convert.ToDouble(rows[0]["ClickFillAmount"][0].ToString());
                PoliceFillAmount = Convert.ToDouble(rows[0]["PoliceFillAmount"][0].ToString());
                HospitalFillAmount = Convert.ToDouble(rows[0]["HospitalFillAmount"][0].ToString());

                if (TimePerSkill_Finish > dtnow)
                {
                    time_time = CalTimePer(TimePerSkill_Finish, dtnow);
                    int temp = CalTimePer(TimePerSkill_Finish, TimePerSkill_Start);
                    runningtime = time_time / 2;
                    Isrunningtime = true;
                    Istimeper = true;
                    TimeButton.interactable = false;
                    TimeCoolImg.enabled = true;
                    TimeCoolImg.fillAmount = (float)time_time / (float)CalTimePer(TimePerSkill_Finish, TimePerSkill_Start);
                    StartCoroutine(CoolImgAlpha(temp, TimeCoolImg));
                    StartCoroutine(CoolTimeSub(1));
                }
                if (ClickPerSkill_Finish > dtnow)
                {
                    click_time = CalTimePer(ClickPerSkill_Finish, dtnow);
                    int temp = CalTimePer(ClickPerSkill_Finish, ClickPerSkill_Start);
                    runningclick = click_time / 2;
                    Isclickper = true;
                    Isrunningclick = true;
                    ClickButton.interactable = false;
                    ClickCoolImg.enabled = true;
                    ClickCoolImg.fillAmount = (float)click_time / (float)CalTimePer(ClickPerSkill_Finish, ClickPerSkill_Start);
                    StartCoroutine(CoolImgAlpha(click_time, ClickCoolImg));
                    StartCoroutine(CoolTimeSub(2));
                }
                if (PoliceSkill_Finish > dtnow)
                {
                    police_time = CalTimePer(PoliceSkill_Finish, dtnow);
                    int temp = CalTimePer(PoliceSkill_Finish, PoliceSkill_Start);
                    Ispolice = true;
                    PoliceButton.interactable = false;
                    PoliceCoolImg.enabled = true;
                    PoliceCoolImg.fillAmount = (float)police_time / (float)CalTimePer(PoliceSkill_Finish, PoliceSkill_Start);
                    StartCoroutine(CoolImgAlpha(police_time, PoliceCoolImg));
                    StartCoroutine(CoolTimeSub(3));
                }
                if (HospitalSkill_Finish > dtnow)
                {
                    hospital_time = CalTimePer(HospitalSkill_Finish, dtnow);
                    int temp = CalTimePer(HospitalSkill_Finish, HospitalSkill_Start);
                    Ishosptal = true;
                    HospitalButton.interactable = false;
                    HospitalCoolImg.enabled = true;
                    HospitalCoolImg.fillAmount = (float)hospital_time / (float)CalTimePer(HospitalSkill_Finish, HospitalSkill_Start);
                    StartCoroutine(CoolImgAlpha(hospital_time, HospitalCoolImg));
                    StartCoroutine(CoolTimeSub(4));
                }
            }
        }
    }
    IEnumerator CoolImgAlpha(int t, Image img)
    {
        while(img.fillAmount > 0)
        {
            Debug.Log(t+" "+img.fillAmount);
            img.fillAmount -= 1 * Time.smoothDeltaTime / t;
            yield return null;
        }
        yield break;
    }
    IEnumerator CoolTimeSub(int n) // 1번:time_time / 2번: click_time / 3번: police_time / 4번: hospital_time
    {
        int t = 0;
        if (n == 1)
            t = time_time;
        else if (n == 2)
            t = click_time;
        else if (n == 3)
            t = police_time;
        else
            t = hospital_time;
        while(t>0)
        {
            t -= 1;
            if (n == 1)
            {
                time_time -= 1;
                runningtime -= 1;
                if (runningtime <= 0)
                    Isrunningtime = false;
            }
            else if (n == 2)
            {
                click_time -= 1;
                runningclick -= 1;
                if (runningclick <= 0)
                    Isrunningclick = false;
            }
            else if (n == 3)
                police_time -= 1;
            else
                hospital_time -= 1;
            yield return new WaitForSeconds(1.0f);
        }
        FinishCool(n);
    }
    void FinishCool(int n) // 1번:time_time / 2번: click_time / 3번: police_time / 4번: hospital_time
    {
        if(n==1)
        {
            Istimeper = false;
            TimeButton.interactable = true;
            TimeCoolImg.enabled = false;
            TimeCoolImg.fillAmount = 1;
        }
        else if(n==2)
        {
            Isclickper = false;
            ClickButton.interactable = true;
            ClickCoolImg.enabled = false;
            ClickCoolImg.fillAmount = 1;
        }
        else if(n==3)
        {
            Ispolice = false;
            PoliceButton.interactable = true;
            PoliceCoolImg.enabled = false;
            PoliceCoolImg.fillAmount = 1;
        }
        else
        {
            Ishosptal = false;
            HospitalButton.interactable = true;
            HospitalCoolImg.enabled = false;
            HospitalCoolImg.fillAmount = 1;
        }
    }
    int CalTimePer(Nullable<DateTime> target, Nullable<DateTime> now)
    {
        TimeSpan? timeDifferent = target - now;
        Debug.Log(timeDifferent.Value.TotalSeconds);
        return Convert.ToInt32(timeDifferent.Value.TotalSeconds); // 타겟 시간(=종료될 시간) - 현재시간 의 남은 총 시간(초)나누기 2를 int형으로 반환시킴.
    }
    public void TimeSkillClick()
    {
        TimePerSkill_Start = DateTime.Now;
        TimePerSkill_Finish = DateTime.Now.AddMinutes(10);
        TimeSave();
        ReadTime();
    }
    public void ClickSkillClick()
    {
        ClickPerSkill_Start = DateTime.Now;
        ClickPerSkill_Finish = DateTime.Now.AddMinutes(10);
        TimeSave();
        ReadTime();
    }
    public void PoliceSkillClick()
    {
        PoliceSkill_Start = DateTime.Now;
        PoliceSkill_Finish = DateTime.Now.AddMinutes(10);
        TimeSave();
        ReadTime();
        GameDataManager.gamedata.police = 100f;
    }
    public void HospitalSkillClick()
    {
        HospitalSkill_Start = DateTime.Now;
        HospitalSkill_Finish = DateTime.Now.AddMinutes(10);
        TimeSave();
        ReadTime();
        GameDataManager.gamedata.medic = 100f;
    }
    void TimeSave()
    {
        Param param = new Param(); // Param은 서버 저장 하는 변수
        param.Add("TimeStart", TimePerSkill_Start.ToString());
        param.Add("TimeFinish", TimePerSkill_Finish.ToString());
        param.Add("ClickStart", ClickPerSkill_Start.ToString());
        param.Add("ClickFinish", ClickPerSkill_Finish.ToString());
        param.Add("PoliceStart", PoliceSkill_Start.ToString());
        param.Add("PoliceFinish", PoliceSkill_Finish.ToString());
        param.Add("HospitalStart", HospitalSkill_Start.ToString());
        param.Add("HospitalFinish", HospitalSkill_Finish.ToString());
        param.Add("TimeFillAmount", TimeCoolImg.fillAmount);
        param.Add("ClickFillAmount", ClickCoolImg.fillAmount);
        param.Add("PoliceFillAmount", PoliceCoolImg.fillAmount);
        param.Add("HospitalFillAmount", HospitalCoolImg.fillAmount);
        Backend.GameData.Update("SkillCool", timeindate, param);
        param.Clear();
    }
    private void OnApplicationQuit()
    {
        TimeSave();
    }
    // Start is called before the first frame update
    void Start()
    {
        ReadTime();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
