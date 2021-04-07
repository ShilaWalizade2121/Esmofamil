using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using Photon.Pun;
public class PlayfabManager : MonoBehaviour
{
    public Text messageText;
    public Text playerNameText;
    public InputField emailInputFieldLogin;
    public InputField nameInput;
    public InputField emailInputFieldReg;
    public InputField passwordInputFieldReg;
    public InputField passwordInputFieldLogin;

    public GameObject leaderList;
    public GameObject leaderListParent;
    // Start is called before the first frame update
    void Start()
    {
        //Login();
    }

    public void RegisterBtn()
    {

        if (emailInputFieldReg.text.Length <= 0 || passwordInputFieldReg.text.Length <= 0 || nameInput.text.Length <= 0)
        {
            messageText.text = "ایمیل، نام یا پسورد نمیتواند خالی باشد".faConvert();
            return;
        }
        if (passwordInputFieldReg.text.Length < 6)
        {
            messageText.text = "پسورد باید بین 6-100 حرف باشد".faConvert();
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            Email = emailInputFieldReg.text,
            Password = passwordInputFieldReg.text,
            DisplayName = nameInput.text.faConvert(),
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "اکونت ایجاد شد".faConvert();

        PhotonNetwork.LocalPlayer.NickName = nameInput.text.faConvert();
        playerNameText.text = nameInput.text.faConvert();
        PhotonNetwork.ConnectUsingSettings();
       
    }

    public void LoginBtn()
    {
        if (emailInputFieldLogin.text.Length <= 0 || passwordInputFieldLogin.text.Length <= 0)
        {
            messageText.text = "ایمیل یا پسورد نمیتواند خالی باشد".faConvert();
            return;
        }
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInputFieldLogin.text,

            Password = passwordInputFieldLogin.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnSuccess, OnError);
    }
    public void RessetBtn()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInputFieldLogin.text,

            TitleId = "2910F"
        };
    }
    void OnPasswordReset(SendAccountRecoveryEmailResult result) {
        messageText.text = "Password reset mail sent!";
    }
    void Login()
    {
        var request = new LoginWithCustomIDRequest { CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }
    void OnSuccess(LoginResult result)
    {
        if (messageText != null)
        {
            messageText.text = "وارد شد".faConvert();

            PhotonNetwork.LocalPlayer.NickName = nameInput.text.faConvert();
            PhotonNetwork.ConnectUsingSettings();
           
            GetUserName();
            //UpdateDisplayName();
        }
        Debug.Log("Successfully login / Account created");

    }

    void GetUserName()
    {
        var request = new GetAccountInfoRequest
        {
            Email = emailInputFieldLogin.text
        };
        PlayFabClientAPI.GetAccountInfo(request, OnSuccessRetrive, OnError);
    }
    void OnSuccessRetrive(GetAccountInfoResult result)
    {
        playerNameText.text = result.AccountInfo.TitleInfo.DisplayName;
        PhotonNetwork.LocalPlayer.NickName = playerNameText.text;
    }

    void OnError(PlayFabError error)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            messageText.text = "انترنت تان را وصل نمایید!".faConvert();
            return;
        }
            if (PlayFabErrorCode.EmailAddressNotAvailable == error.Error || PlayFabErrorCode.InvalidEmailAddress == error.Error || PlayFabErrorCode.DuplicateEmail == error.Error)
        {
            messageText.text = "ایمیل آدرس در دسترس نیست".faConvert();
            return;
        }
        if (PlayFabErrorCode.InvalidParams == error.Error || PlayFabErrorCode.InvalidEmailOrPassword == error.Error || PlayFabErrorCode.InvalidContentType == error.Error)
        {
            messageText.text = "ایمیل یا پسورد درست نیست".faConvert();
                return;
        }
        if (PlayFabErrorCode.AccountNotFound == error.Error)
        {
            messageText.text = "ایمیل در سیستم ثبت نیست!".faConvert();
            return;
        }
        Debug.Log("Error while login"+error.GenerateErrorReport());
       
    }

    public void Logout()
    {
        PlayFabClientAPI.ForgetAllCredentials();
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Score",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Leaderboard updated!");
    }

    public void GetLeaderboardData()
    {
        
        var request = new GetLeaderboardRequest
        {
           StartPosition=0, StatisticName = "Score", MaxResultsCount=30
        };
        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnError);
     
    }
    void OnGetLeaderboardSuccess(GetLeaderboardResult result)
    {
        foreach (PlayerLeaderboardEntry player in result.Leaderboard)
        {
            GameObject leList = Instantiate(leaderList);
            leList.transform.SetParent(leaderListParent.transform);
            leList.transform.localScale = Vector3.one;
            leList.transform.Find("name-txt").GetComponent<Text>().text = player.DisplayName;
            leList.transform.Find("score-txt").GetComponent<Text>().text = player.StatValue.ToString();
            Debug.Log(player.DisplayName +" ==== "+ player.StatValue +" ===== "+player.Position);
        }
    }
}
