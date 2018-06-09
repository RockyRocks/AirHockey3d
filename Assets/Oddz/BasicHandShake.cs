using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
//using NDde.Client;
//https://sb.oddz.com/cas/login?service=http://182.18.176.6/webbuild.html

public class BasicHandShake : MonoBehaviour
{
	private WWW url;
	private string CASUrltoParse;
	[HideInInspector]
	public string CAS;
	[HideInInspector]
	public static bool IsCASread=false;
	private bool SessionKeyValid=false;
    public static bool GamePlayReveal_flag = false;
    public int minTokenExpiration = 30;
	private Request request;
    private double RevealingAmount;
    private List<string> gamePlayScript_Collection;

    # region Oddz_variables

    public OddzInteract Oddz;
	private Properties.RequestType OddzType;
	private OddzInteract.Session session;
	private OddzInteract.Account account;
	private OddzInteract.Balances balances;
	private OddzInteract.gameplay gameplay;
    private OddzInteract.gameplay_events gameplay_events;
	private OddzInteract.Play play;
	private OddzInteract.RootBalancesInfo balanceInfo;
	private OddzInteract.GetTokenRootInfo TokenInfo;

    #endregion Oddz_variables

    #region NewOddzstuff

    public string statusMessage = "";
    public string webContentMessage = "";
    public string igwfUrl = "http://sb.oddz.com/web";
    [HideInInspector]
    public static string loginUrl;
    public static string apiUrl = "http://sb.oddz.com/api/v1.2";

    // The page hosting the webplayer
    public static string thisPage = "http://www.tikiinteractive.com/airhockey/webbuild.html";
    // The game's apiKey
    public static string apiKey;
    private bool SessionRequested = false;
    private string jsonString;
    #endregion NewOddzstuff

	//Use this for initialization.
	void Awake()
	{

        Application.targetFrameRate = -1;
   
        Debug.Log("we are inside the basic HandShake script");
        apiKey = Oddz.ApiKey;
        loginUrl = igwfUrl + "/player_login";
       // Application.OpenURL(igwfUrl + "/menu?apiKey=" + apiKey + "&successUrl=" + thisPage + "?igwf_flow=success&failure_url=" + thisPage + "?igwf_flow=fail");
		//request = GameObject.Find ("Oddz").GetComponentInChildren<Request> ();
        OddzType = Properties.RequestType.none;
	}

	public void Update()
	{
#if UNITY_WEBPLAYER
        
            this.CheckCASUrl();
            this.GetSessionID_WEB();
        if (Properties.IsLoggedIn)
        {
            this.PlayWithTokens_WEB(Properties.SelectedDenomination);
            this.UpdateGamePlayBalance();
            // GameWonState is not changing have to find an another way to solve this....
            if (Properties.GameWonState == "Win" && GamePlayReveal_flag == true && GamePlayEvent_script!=null)
            {
                //foreach (string script in gamePlayScript_Collection)
                //{
                //    Application.ExternalEval(script);
                //    Debug.Log("i am explicitly calling GamePlayEvent here:" + script);
                //}
                Application.ExternalEval(GamePlayEvent_script);
                GamePlayReveal_flag = false;
                this.GamePlayEvent_script = null;
                this.gamePlayScript_Collection.Clear();
            }
        }
        if (Properties.IsLoggedIn && Properties.DeleteSession)
        {
            this.DeleteSession();
            Properties.DeleteSession = false;
        }
        if(!string.IsNullOrEmpty(jsonString))
            this.Switching_OddzCalls(OddzType, jsonString);
#endif

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        this.GetSessionId_WWW ();
        this.PlayWithTokens (Properties.SelectedDenomination);
        if (Oddz.Geturl != null && 
		    Oddz.Geturl.error == null && 
		    !SessionKeyValid && request.isDone==true) {
            // TO DO: Have to pass the real oddzType
			this.Switching_OddzCalls(OddzType,jsonString); 
        }
#endif
	}
    /// <summary>
    /// Checking for the Url in the webpage and passing it to Swagger API to get the session ID
    /// </summary>
    /// <returns></returns>
    private bool CheckCASUrl()
    {
        if (IsCASread)
            return true;
		if(ApiValidated)
        Application.ExternalEval(
            "unity.getUnity().SendMessage('Oddz', 'ReceiveURL', window.location.href);");

        if (CASUrltoParse != null && !IsCASread)
        {
            if (CASUrltoParse.Contains("ticket"))
            {
                CAS = getBetween(CASUrltoParse, "ticket=", "cas");
                CASUrltoParse = null;
                IsCASread = true;
                Properties.IsLogoTransitionsAllowed = false;
                return true;
            }
        }
        Properties.IsLogoTransitionsAllowed = true;
        return false;
    }
       
    /// <summary>
    /// Making the fake data from token info and balances.
    /// </summary>
    private void UpdateGamePlayBalance()
    {
        if (balanceInfo!=null && 
            balanceInfo.balances.availableGameplayBalance != Properties.GamePlayBalance && 
            SessionKeyValid)
        {
            balanceInfo.balances.availableGameplayBalance=Properties.GamePlayBalance;
           
        }
        if(TokenInfo!=null)
            Properties.WonAmount = TokenInfo.tokens[0].value.ToString();
    }

    #region Unity_Web

    private void GetSessionID_WEB()
    {
        if (IsCASread && !SessionRequested && ApiValidated)
        {
            var javaScript =
                "owc.Oddz.session({accessToken: '" + CAS + "',apiKey: '" + apiKey + "'},function(data) { unity.getUnity().SendMessage('Oddz', 'OnGetSession_WEB', JSON.stringify(data) )},function(error){unity.getUnity().SendMessage('Oddz', 'OnFailSession_WEB', JSON.stringify(error))});";
            Application.ExternalEval(javaScript);    
            Debug.Log(javaScript);
            SessionRequested = true;
        }
    }

    public void OnGetSession_WEB(string jsondata)
    {
        jsonString = jsondata;
        OddzType = Properties.RequestType.Session;
        Properties.AcessKey = CAS;
        Debug.Log("we are inside the OnGETSESSION_WEB" + jsondata);
        Properties.IsLoggedIn = true;
    }
    public void OnFailSession_WEB(string data)
    {
        Debug.Log("Session Failed");
        Properties.IsLogoTransitionsAllowed = true;
        OddzType = Properties.RequestType.none;
    }
    bool ApiValidated = false;
    public void SessionValidate_WEB(string result)
    {
		Debug.Log (result);
        if (result == "true")
            ApiValidated = true;
    }

    private void GetAccount_WEB(string SessionKey)
    {
        var javaScript = 
            string.Format(@"owc.Oddz.getAccount({{apiKey: '{0}', sessionId: '{1}'}}, function(data) {{unity.getUnity().SendMessage('Oddz', 'OnGetAccount_WEB', JSON.stringify(data));}},function(error) {{console.log( 'GetAccount error: '+error );}});",
                                                                session.apiKey,session.sessionKey);
        Application.ExternalEval(javaScript);
        Debug.Log(javaScript);
    }

    public void OnGetAccount_WEB(string jsondata)
    {
        Debug.Log("i am inside account:" + jsondata);
        jsonString = jsondata;
        OddzType = Properties.RequestType.Account;
    }

    private void DeleteSession()
    {
		//if (session != null) 
		{
			Debug.Log ("I am inside the DeleteSession:");
			var javaScript =
                string.Format(@"owc.Oddz.deleteSession({{apiKey:'{0}',sessionId:'{1}'}},function(data){{unity.getUnity().SendMessage('Oddz', 'OnDeleteSession', 'data');}},function(error){{console.log('delete Session: '+ error);}});",
                apiKey, Properties.SessionKey);
			Application.ExternalEval (javaScript);
			Debug.Log (javaScript);
		}
    }

    public void OnDeleteSession(string jsondata)
    {
        Debug.Log("i am inside OndeleteSession: "+jsondata);
        Properties.IsLoggedIn = false;
        Properties.DeleteSession = false;
        OddzType = Properties.RequestType.DeleteSession;
    }

    private void PlayWithTokens_WEB(double Denomination)
    {
        if (SessionRequested && Properties.S_GoingtoPlay)
        {
            Debug.Log("Calling play tokens javascript");
            play = new OddzInteract.Play()
            {
                minToExpiration = minTokenExpiration,
                apiKey = Oddz.ApiKey,
                sessionId = Properties.SessionKey,
                count=1,
            };
            play.jsonToken = new OddzInteract.PlayTokens()
                {
                    tokenType = play.tokenType,
                    tokenDenomination = Denomination,
                    count = play.count,
                };

            var javaScript =
			string.Format(@"owc.Oddz.playTokens({{sessionId:'{0}',minToExpiration:{1},body:{2},apiKey:'{3}'}},function(data) {{ unity.getUnity().SendMessage('Oddz', 'OnGetPlay_WEB', JSON.stringify(data))}})",
                                                              play.sessionId,
                                                              play.minToExpiration,
                                                              JsonConvert.SerializeObject(play.jsonToken),
                                                              apiKey);
            Debug.Log(javaScript);

            Application.ExternalEval(javaScript);

            Properties.S_GoingtoPlay = false;
        }
    }

    public void OnGetPlay_WEB(string jsondata)
    {
        Debug.Log("i am inside play:" + jsondata);
        jsonString = jsondata;
        OddzType = Properties.RequestType.Play;
    }
    string GamePlayEvent_script;
    private void GamePlay_RevealToken_WEB()
    {
        if (play != null)
        {
			if(play.tokens!=null)
			{
                gameplay = new OddzInteract.gameplay()
                {
                    apiKey = Oddz.ApiKey,
                    description = Properties.ChallengeMode,
                    tokenId = play.tokens[0].id,
                    occurredAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    sessionId = Properties.SessionKey,
                    Tokens=play.tokens,
                };
                RevealingAmount += TokenInfo.tokens[0].value;

                Properties.TotalWinnings += TokenInfo.tokens[0].value;

                gameplay_events.gameplay.Add(this.gameplay);

                var javaScript = string.Format(@"owc.Oddz.gameplay_events({{body:{0},sessionId:'{1}',apiKey:'{2}'}},function(data){{unity.getUnity().SendMessage('Oddz', 'OnGamePlay_RevealToken_WEB', JSON.stringify(data))}})",
                                                        JsonConvert.SerializeObject(gameplay_events),
                                                        gameplay_events.sessionId,
                                                        gameplay_events.apiKey);
                //var javaScript =
                //    string.Format(@"owc.Oddz.gameplay({{sessionId:'{0}',description:'{1}',tokenId:{2},occurredAt:'{3}',action:'{4}',apiKey:'{5}'}},function(data){{unity.getUnity().SendMessage('Oddz', 'OnGamePlay_RevealToken_WEB', JSON.stringify(data))}})",
                //                                                  gameplay.sessionId,
                //                                                  gameplay.description,
                //                                                  gameplay.tokenId,
                //                                                  gameplay.occurredAt,
                //                                                  gameplay.action,
                //                                                  gameplay.apiKey);                                                      
                GamePlayEvent_script = javaScript;

                // Adding the collection of the gameplay events into the list so that we can reveal them all once the player wins.
                //gamePlayScript_Collection.Add(javaScript);

                //Application.ExternalEval(javaScript);
               // Debug.Log(javaScript);
               // Debug.Log(javaScript_events);
			}
        }                                                                         
    }

    public void OnGamePlay_RevealToken_WEB(string jsondata)
    {
        Debug.Log("i am inside gameplay || gameplay_events :"+jsondata);
        jsonString = jsondata;	
        OddzType = Properties.RequestType.GamePlay_Events;
    }

    public void RevealAllGameplay_Events_WEB()
    {
        var javaScript = string.Format(@"owc.Oddz.gameplay_events({{sessionId:'{0}',apiKey:'{1}',body:{2})",
            gameplay_events.sessionId,
            gameplay_events.apiKey,JsonConvert.SerializeObject(gameplay_events.gameplay));
    }


    public void OnSessionSuccess(string result)
    {
        Properties.SessionKey = result;
        //debugconsole.log(result);
        Debug.Log("We are inside OnsessionSuccess"+result);
    }

    public void OnSessionFail(string failureReason)
    {
        statusMessage = "Uhh oh. We failed because of: " + failureReason;
        //debugconsole.log(statusMessage);
    }

    public void WebContentFlowCompleted(string result)
    {
        webContentMessage = "Here it is" + result;
        Debug.Log(webContentMessage);
    }

    public string GetApiKey(string result)
    {
        Debug.Log(result);
        Debug.Log("we are inside the getApiKey");
        return apiKey;
    }

    #endregion Unity_Web

    /// <summary>
    /// Calling the OddzType to de serialize the json data into local objects.
    /// </summary>
    /// <param name="_OddzType"></param>
    private void Switching_OddzCalls(Properties.RequestType _OddzType, string jsonString)
	{
        switch (_OddzType)
		{
            case Properties.RequestType.Session:

            Debug.Log("oddztype:" + _OddzType.ToString());
            Debug.Log(jsonString);
            balanceInfo = JsonConvert.DeserializeObject<OddzInteract.RootBalancesInfo>(jsonString);
            Oddz.sessionId = balanceInfo.sessionKey;
            Properties.SessionKey = balanceInfo.sessionKey;
            Properties.GamePlayBalance=balanceInfo.balances.availableGameplayBalance;
    	    Properties.CashBalance=balanceInfo.balances.cashBalance;
            SessionKeyValid = true;
            gameplay_events = new OddzInteract.gameplay_events {sessionId=Properties.SessionKey,apiKey=apiKey,gameplay=new List<OddzInteract.gameplay>(),};
            gamePlayScript_Collection = new List<string>();
            Debug.Log(balanceInfo.sessionKey);
            Debug.Log(balanceInfo.balances.cashBalance);
            Debug.Log(balanceInfo.balances.availableGameplayBalance);
            //request.isDone = false;
            OddzType = Properties.RequestType.none;

			break;
		
		    case Properties.RequestType.Account:

            balanceInfo = JsonConvert.DeserializeObject<OddzInteract.RootBalancesInfo>(jsonString);
            //debugconsole.log( balanceInfo.balances.availableGameplayBalance.ToString());
            Properties.CashBalance = balanceInfo.balances.cashBalance;
            OddzType = Properties.RequestType.none;
            //request.isDone = false;

			break;

		    case Properties.RequestType.GamePlay:

            Debug.Log("oddztype:" + _OddzType.ToString());
            //debugconsole.log(jsonString);
            //debugconsole.log(TokenInfo.tokens[0].value.ToString());
            OddzType = Properties.RequestType.none;
            //request.isDone = false;

			break;

            case Properties.RequestType.GamePlay_Events:

            Debug.Log("oddztype:" + _OddzType.ToString());
            gameplay_events.gameplay.Clear();
            OddzType = Properties.RequestType.none;

            break;

		    case Properties.RequestType.Play:

            Debug.Log("oddztype:"+_OddzType.ToString());
            var defaultresolver = new DefaultContractResolver();
            // Doing this because Webplayer build doesn't support deserialization to private/protected members of List<T>
            defaultresolver.DefaultMembersSearchFlags|=System.Reflection.BindingFlags.NonPublic;
            var settings = new JsonSerializerSettings { ContractResolver=defaultresolver};
            TokenInfo = JsonConvert.DeserializeObject<OddzInteract.GetTokenRootInfo>(jsonString, settings);
            Properties.WonAmount = TokenInfo.tokens[0].value.ToString();
			Debug.Log(Properties.WonAmount);
			Debug.Log("got the won amount");
		    play.tokens=TokenInfo.tokens;             
            this.GamePlay_RevealToken_WEB();
            OddzType = Properties.RequestType.none;
            //request.isDone = false;

			break;

		    case Properties.RequestType.Balances:

            OddzType = Properties.RequestType.none;
            //request.isDone = false;
		
            break;

            case Properties.RequestType.DeleteSession:

            //request.isDone = false;
            Properties.SessionKey = null;
            Properties.CashBalance = 0;
            Properties.GamePlayBalance = 0;
            Properties.TotalWinnings = 0;
            Properties.WonAmount = null;
            Properties.IsLoggedIn = false;
            IsCASread = false;
            session = null;
            TokenInfo = null;
            play = null;
            gameplay = null;
            gameplay_events = null;
            gamePlayScript_Collection = null;
            OddzType = Properties.RequestType.none;
            WorkAround.LoadLevelWorkaround("ModesScene");

            break;

		    case Properties.RequestType.none:
            
            //request.isDone = false;
			
            break;
		}
	}

	/// <summary>
	/// Gets the session identifier Once the CAS is retrieved.
	/// </summary>
	/// <returns><c>true</c>, if session identifier was gotten, <c>false</c> otherwise.</returns>
    private bool GetSessionId_WWW()
	{
		if (IsCASread) {
						IsCASread = false;
					    session = new OddzInteract.Session ()
						{
							apiKey=Oddz.ApiKey,
							accessToken=CAS,
						};
						Properties.AcessKey=CAS;
						var url = OddzInteract.CompleteUrl (OddzInteract.Session.url, session.RequestParams);
						Debug.Log (url);
                        //debugconsole.log(url);
                        var _params = new Dictionary<string, string>();
                        _params.Add(session.RequestParams.GetKey(0), session.apiKey);
                        _params.Add(session.RequestParams.GetKey(1), session.accessToken);
                        _params.Add(session.RequestParams.GetKey(2), session.apiKey);
                        this.Oddz.Geturl = request.POST(url, _params);
						OddzType = Properties.RequestType.Session;
						return true;
				}
		return false;
	}

	/// <summary>
	/// start the game with tokens and its denominations.
	/// </summary>
	/// <param name="Denomination">Denomination.</param>
    private void PlayWithTokens(double Denomination)
	{
		if (GetSessionId_WWW () && SessionKeyValid && Denomination!=0) {
		    play= new OddzInteract.Play()
			{
				minToExpiration=20,
				apiKey=Oddz.ApiKey,
				sessionId=Properties.SessionKey,
			};
			var url=OddzInteract.CompleteUrl(OddzInteract.Play.url,play.RequestParams);
            //debugconsole.log(url);
			play.jsonToken= new OddzInteract.PlayTokens()
			{
				tokenType=play.tokenType,
				tokenDenomination=Denomination,
				count=play.count,
			};
			this.Oddz.Geturl=request.PostDataWithJson(url,play.jsonToken,"Content-Type","application/json");
			OddzType = Properties.RequestType.Play;
		}
	}

	/// <summary>
	/// Winnings in the Current Round.
	/// REVEALS the token of the current bet.
    /// We should do this as soon as the Play is requested else it gets auto expires after some time.
	/// </summary>
    private void GamePlay_RevealToken()
	{
		gameplay = new OddzInteract.gameplay ()
		{
			apiKey=Oddz.ApiKey,
			description=Properties.ChallengeMode,
			tokenId=play.tokens[0].id,
            occurredAt = DateTime.UtcNow.ToString("s"),
			sessionId=Properties.SessionKey,
		};
		var url = OddzInteract.CompleteUrl (gameplay.url, gameplay.RequestParams);
		Debug.Log (url);
        //debugconsole.log(url);
		var _params=new Dictionary<string,string>();
		_params.Add(gameplay.RequestParams.GetKey(0),gameplay.description);
		_params.Add(gameplay.RequestParams.GetKey(1),gameplay.tokenId.ToString());
		_params.Add(gameplay.RequestParams.GetKey(2),gameplay.occurredAt);
		_params.Add(gameplay.RequestParams.GetKey(3),gameplay.action);
		_params.Add(gameplay.RequestParams.GetKey(4),gameplay.apiKey);
		_params.Add(gameplay.RequestParams.GetKey(5),gameplay.sessionId);

		this.Oddz.Geturl=request.POST(url,_params);
		OddzType = Properties.RequestType.GamePlay;
	}
                                                                                           
	private  void ReceiveURL(string url) {
		CASUrltoParse = url;
	}

	public static string getBetween(string strSource, string strStart, string strEnd)
	{
		int Start, End;
		if (strSource.Contains(strStart) && strSource.Contains(strEnd))
		{
			Start = strSource.IndexOf(strStart, 0) + strStart.Length;
			End = strSource.IndexOf(strEnd, Start);
			strSource = strSource.Substring(Start, End - Start);
			return String.Concat(strSource,"cas.com");
		}
		else
		{
			//debugconsole.log("Error No CAS Key Found");
			return "Error No CAS key found";
		}
	}

	#region CodedforTesting

    [Obsolete("We do not need location anymore here in checking stage")]
    IEnumerator Test()
    {
        yield return StartCoroutine(CheckforCAS());

        if (url.error == null)
        {
            foreach (KeyValuePair<string, string> i in url.responseHeaders)
            {
                Debug.Log(i.Key.ToString() + ":" + i.Value);
                if (i.Key.Trim() == "LOCATION")
                {
                    Debug.Log("key:" + i.Key.Trim() + "value:" + i.Value);
                }
            }
        }
        else
        {
            Debug.LogError("error: " + url.error);
        }
    }

    IEnumerator CheckforCAS()
    {
        Application.ExternalEval(
            "u.getUnity().SendMessage('testing', 'ReceiveURL', window.location.href);");
        url = new WWW("https://sb.oddz.com/cas/login?service=http://182.18.176.6/webbuild.html");
        yield return url;
    }

    public string m_BaseUrl(string url1, string url2)
    {
        return Path.Combine(url1, url2);
    }

//	public static string RedirectPath(string url)
//	{ 
//		StringBuilder sb = new StringBuilder();
//		string location = string.Copy(url);
//		while (!string.IsNullOrEmpty(location))
//		{
//			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(location);
//			request.AllowAutoRedirect = false;
//			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
//			{
//				location = response.GetResponseHeader("Location");
//			}
//		}
//		return sb.ToString();
//	}
//
//	string cookie="";
//	IEnumerator LoadData(){
//		var url_test = "https://sb.oddz.com/web/player_login?apiKey=285dcdb0-9e8b-4e17-8a2f-f72551a16f3d&successUrl=http://testtiki.s3-website-us-east-1.amazonaws.com/";
//		Debug.Log (url_test);
//		WWWForm form= new WWWForm();
//		
//		string time = System.DateTime.Now.Ticks.ToString();
//		form.AddField("time", time);
//		// construct your header calls
//		var headers = form.headers;
//		if(cookie!="")
//			headers["Cookie"] = cookie;
//		
//		var www = new WWW(url_test, form.data, headers);
//		
//		yield return www;
//		
//		if(www.error==null){
//			Debug.Log(www.text);
//			
//			// get the cookie and keep it
//			if(www.responseHeaders.ContainsKey("SET-COOKIE")){
//				String[] data= www.responseHeaders["SET-COOKIE"].Split(";"[0]);
//				if(data.Length>0){
//					cookie = data[0];
//				}
//			}
//			
//		}else
//			Debug.LogError(www.error);
//		// debug
//		foreach(KeyValuePair<String, String> header in www.responseHeaders)
//			Debug.Log(header.Key+" "+header.Value);
//	}
	#endregion CodedforTesting
}

