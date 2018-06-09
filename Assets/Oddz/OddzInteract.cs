using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Serialization;
/******JsonLib headers*********/
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
/********************************/

public class OddzInteract : MonoBehaviour
{

    public WWW Geturl;
    public WWW postDataurl;
    private string BaseUrl = "https://sb.oddz.com/api/v1.2";
    public static string Version = "v1.2";
    public string ApiKey = "d57b8596-ec91-4ee0-93c5-6f1f693db788";
    [HideInInspector]
    public string sessionId;
    private string userHandle = "tikiinteractive";
    private string accessToken;
    private string CAS;
    private static OddzInteract Instance;
    private string ServerResponse_json;
    private RootBalancesInfo balanceInfo;
    private GetTokenRootInfo TokenInfo;
    [HideInInspector]
    public bool isFetchingDone = false;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }

    #region OldCode
    // changed from start to test
    public IEnumerator test()
    {
        Account account = new Account
        {
            apiKey = ApiKey,
            sessionId = sessionId,
        };
        yield return StartCoroutine(GetData(CompleteUrl(Account.url, account.RequestParams)));
        // to post the json data to the server
        //yield return StartCoroutine (PostData ());

        if (Geturl.error == null)
        {
            //JObject json= this.simpleParse(Geturl.text);
            //Debug.Log(json.ToString());
            balanceInfo = JsonConvert.DeserializeObject<RootBalancesInfo>(Geturl.text);
            Debug.Log("this is deserialized response:  " + balanceInfo.sessionKey);
        }
        //		if (postDataurl.error== null) {
        //			TokenInfo=JsonConvert.DeserializeObject<GetTokenRootInfo>(postDataurl.text);
        //			Debug.Log("balances info after posting json: "+TokenInfo.tokens[0].tokenType);
        //				}
    }

    public IEnumerator GET(string url)
    {
        yield return StartCoroutine(GetData(url));
        Debug.Log("hurray we are in: GET");
        if (Geturl.text != "")
        {
            JObject json = this.simpleParse(Geturl.text);
            Debug.Log(json.ToString());
            var j = JsonConvert.DeserializeObject<RootBalancesInfo>(Geturl.text);
            Debug.Log("this is deserialized response" + j.sessionKey);
        }
    }

    // even though it says get data we can still add values in the request url.-> to DataBase.cs
    ///<summary>
    /// This is in testing now just pass the url using Combinepaths method which returns string
    /// </summary>
    public IEnumerator GetData(string url)
    {
        Geturl = new WWW(url);
        yield return Geturl;
        this.isFetchingDone = true;
        DebugConsole.Log(isFetchingDone.ToString());
    }
    ///<summary>
    /// Example of how we can build a json using class Object.
    /// Balances bal = new Balances{
    ///cashBalance=1.0,
    ///heldCashBalance=1.0};
    /// </summary>
    void buildJson()
    {
        Debug.Log("we are building a json from object");
        Balances bal = new Balances
        {
            cashBalance = 1.0,
            heldCashBalance = 1.0,
            availableCashBalance = 1.0,
            availableGameplayBalance = 1.0,
            pendingGameplayBalance = 1.0
        };
        string json = JsonConvert.SerializeObject(bal, Formatting.Indented);
        Debug.Log(json);
    }

    public JObject simpleParse(string text)
    {
        var jobject = JObject.Parse(text);
        return jobject;
    }

    // have to send this to DataBase.cs and wwwforms should be called instead of hashtable or a string builder.
    [Obsolete("Please use POST in request-> all the requests has been transferred to the Request class")]
    public IEnumerator PostData(object Class = null, object url = null)
    {
        Hashtable headers = new Hashtable();
        headers.Add("Content-Type", "application/json");
        string jsonString = JsonConvert.SerializeObject(Class, Formatting.Indented);
        Debug.Log(jsonString);
        byte[] pData = Encoding.UTF8.GetBytes(jsonString.ToCharArray());
        postDataurl = new WWW(url as String, pData, headers);
        Debug.Log(postDataurl.url);
        yield return postDataurl;
    }

    #endregion OldCode

    ///<summary>
    /// use namevaluecollection to build a query string with variables please ignore the params for now thank you. 
    /// For QueryParms use individual class type namevaluecollection : 
    /// for example: account.RequestParmas will automatically fill in all the assigned parameters to that url.
    /// </summary>
    public static string CompleteUrl(string BaseUrl, NameValueCollection QueryParams, params string[] TheRest)
    {
        return BaseUrl + ToQueryString(QueryParams);
    }
    /// <summary>
    /// Build the collection of parameters into a query string.
    /// </summary>
    /// <returns>The query string.</returns>
    /// <param name="nvc">list of parameters</param>
    public static string ToQueryString(NameValueCollection nvc)
    {
        StringBuilder sb = new StringBuilder("?");

        bool first = true;

        foreach (string key in nvc.AllKeys)
        {
            foreach (string value in nvc.GetValues(key))
            {
                if (!first)
                {
                    sb.Append("&");
                }

                sb.AppendFormat("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(value));

                first = false;
            }
        }

        return sb.ToString();
    }

    public static string m_BaseUrl(string url1, string url2)
    {
        return Path.Combine(url1, url2).Replace(@"\", "/");
    }

    #region OddzRequests

    /****************************** Oddz API calls**********************/
    [Serializable]
    internal class Account
    {
        public static string url { get { return "https://sb.oddz.com/api/v1.1/account"; } }
        public JObject json { get; set; }
        public string apiKey { get; set; }
        public string sessionId { get; set; }
        public static string Type { get { return "account"; } }
        public double availableGameplayBalance { get; set; }
        public double heldCashBalance { get; set; }
        public double availableCashBalance { get; set; }
        public double pendingGameplayBalance { get; set; }
        public double cashBalance { get; set; }
        public NameValueCollection RequestParams
        {
            get
            {
                var QueryStrings = new NameValueCollection()
				{
					{"apiKey",apiKey},
					{"sessionId",sessionId},
					{"api_key",apiKey},
				};
                return QueryStrings;
            }
        }
    }

    [Serializable]
    internal class gameplay
    {
        [JsonIgnore]
        public string url { get { return "https://sb.oddz.com/api/v1.1/gameplay"; } }
        [JsonIgnore]
        public string Type { get { return "gameplay"; } }

        public string description { get; set; }

        public string occurredAt { get; set; }

        public string action { get { return "reveal"; } } // set needs to be changed

        public int tokenId { get; set; }
        [JsonIgnore]
        public string apiKey { get; set; }

        public string sessionId { get; set; }
        [JsonIgnore]
        public List<Token> Tokens { get; set; }
        [JsonIgnore]
        public NameValueCollection RequestParams
        {
            get
            {
                var QueryStrings = new NameValueCollection()
				{
					{"description",apiKey.ToString()},
					{"tokenId",tokenId.ToString()},
					{"occurredAt",occurredAt},
					{"action",action},
					{"apiKey",apiKey},
					{"sessionId",sessionId},
					{"api_key",apiKey},
				};
                return QueryStrings;
            }
        }
    }

    [Serializable]
    internal class gameplay_events
    {
        [JsonIgnore]
        public static string url { get { return "https://sb.oddz.com/api/v1.1/gameplay_events"; } }
        //public static JObject json{ get; set;}
        [JsonIgnore]
        public static string Type { get { return "gameplay_events"; } }
        public List<gameplay> gameplay { get; set; }
        //public  string Description { get; set;}
        //public  string OccuredAt { get; set;}
        //public  string Action { get; set;}
        //public  int tokenId { get; set;}
        [JsonIgnore]
        public string apiKey { get; set; }
        [JsonIgnore]
        public string sessionId { get; set; }
        [JsonIgnore]
        public NameValueCollection RequestParams
        {
            get
            {
                var QueryStrings = new NameValueCollection()
				{
					{"apiKey",apiKey},
					{"sessionId",sessionId},
					{"api_key",apiKey},
				};
                return QueryStrings;
            }
        }
    }

    [Serializable]
    internal class Balances
    {
        public static string Type { get { return "balances"; } }
        public double availableGameplayBalance { get; set; }
        public double heldCashBalance { get; set; }
        public double availableCashBalance { get; set; }
        public double pendingGameplayBalance { get; set; }
        public double cashBalance { get; set; }

        public double reservedGameplayBalance { get; set; }
    }

    [Serializable]
    internal class RootBalancesInfo
    {
        public string userHandle { get; set; }
        public string sessionKey { get; set; }
        public Balances balances { get; set; }
    }

    #region Deserialization doesn't work on internal class types in WebPlayer build works for stand alone since they are private so changed to Public
    [Serializable]
    internal class GetTokenRootInfo
    {

        public Totals totals { get; set; }

        public List<Token> tokens { get; set; }

        public string correlationCode { get; set; }

        public string sessionKey { get; set; }
    }

    [Serializable]
    internal class Totals
    {
        public string tokenType { get; set; }
        public double tokenDenomination { get; set; }
        public int count { get; set; }
        public static string Type { get { return "totals"; } }
    }

    [Serializable]
    public class Token
    {
        public string tokenType { get; set; }
        public int id { get; set; }
        public double denomination { get; set; }
        public string status { get; set; }
        public string expiration { get; set; }
        public double value { get; set; }
        public static string Type { get { return "tokens"; } }
    }

    # endregion
    // unfinished still have to figure out how to retrieve the cas ticket :/
    [Serializable]
    internal class Session
    {
        public static string url { get { return "https://sb.oddz.com/api/v1.1/session"; } }
        public static string Type { get { return "session"; } }
        public string sessionKey { get; set; }
        public string userHandle { get; set; }
        public Balances balances { get; set; }
        public string successUrl { get; set; }
        public string accessToken { get; set; }
        public string apiKey { get; set; }
        public NameValueCollection RequestParams
        {
            get
            {
                var QueryStrings = new NameValueCollection()
				{
					{"apiKey",apiKey},
					{"accessToken",accessToken},
					{"api_key",apiKey},
				};
                return QueryStrings;
            }
        }
    }

    [Serializable]
    internal class Cancel
    {
        public static string url { get { return "https://sb.oddz.com/api/v1.1/token/cancel"; } }
        public static string Type { get { return "cancel"; } }
        public string[] cancel_tokens { get; set; }
        public string description { get; set; }
        public string occuredAt { get; set; }
        public int tokenId { get; set; }
        public string apiKey { get; set; }
        public string sessionId { get; set; }
        public NameValueCollection RequestParams
        {
            get
            {
                var QueryStrings = new NameValueCollection()
				{
					{"apiKey",apiKey},
					{"sessionId",sessionId},
					{"api_key",apiKey},
				};
                return QueryStrings;
            }
        }
    }

    [Serializable]
    internal class PlayTokens
    {
        public string tokenType { get; set; }
        public double tokenDenomination { get; set; }
        public int count { get; set; }
    }

    [Serializable]
    internal class Play
    {
        public static string url { get { return "https://sb.oddz.com/api/v1.1/token/play"; } }
        public JObject json { get; set; }
        public static string Type { get { return "play"; } }
        public List<Token> tokens { get; set; }
        public string tokenType { get { return "TOKENA"; } }
        public double tokenDenomination { get; set; }
        public int count { get; set; }
        public string apiKey { get; set; }
        public string sessionId { get; set; }
        public int minToExpiration { get; set; }
        public PlayTokens jsonToken { get; set; }
        public NameValueCollection RequestParams
        {
            get
            {
                var QueryStrings = new NameValueCollection()
				{
					{"minToExpiration",minToExpiration.ToString()},
					{"apiKey",apiKey},
					{"sessionId",sessionId},
					{"api_key",apiKey},
				};
                return QueryStrings;
            }
        }
    }
    #endregion OddzRequests

    #region TestOddzRequest
    /*********************************************************************/
    /*To Test buy and create packages*/
    [Serializable]
    internal class Deposits
    {
        public static string url { get { return "https://sb.oddz.com/api/v1.1/test/deposit"; } }
        public JObject json { get; set; }
        public static string Type { get { return "deposit"; } }
        public double availableGameplayBalance { get; set; }
        public double heldCashBalance { get; set; }
        public double availableCashBalance { get; set; }
        public double pendingGameplayBalance { get; set; }
        public double cashBalance { get; set; }
        //	public NameValueCollection CompleteRequestUrl{get {
        //			var QueryStrings= new NameValueCollection()
        //			{
        //				{"apiKey",apiKey},
        //				{"sessionId",sessionId},
        //				{"api_key",apiKey},
        //			};
        //			return QueryStrings;
        //		}}
    }

    [Serializable]
    internal class Buy
    {
        public static string url { get { return "https://sb.oddz.com/api/v1.1/test/package/buy"; } }
        public static string Type { get { return "buy"; } }
        public double expectedTotalCost { get; set; }
        public int quantity { get; set; }
        public int packageTypeId { get; set; }
        //	public NameValueCollection CompleteRequestUrl{get {
        //			var QueryStrings= new NameValueCollection()
        //			{
        //				{"apiKey",apiKey},
        //				{"sessionId",sessionId},
        //				{"api_key",apiKey},
        //			};
        //			return QueryStrings;
        //		}}
    }

    [Serializable]
    internal class Status
    {
        public static string url { get { return "https://sb.oddz.com/api/v1.1/test/status"; } }
        public static string Type { get { return "status"; } }
        //	public NameValueCollection CompleteRequestUrl{get {
        //			var QueryStrings= new NameValueCollection()
        //			{
        //				{"apiKey",apiKey},
        //				{"sessionId",sessionId},
        //				{"api_key",apiKey},
        //			};
        //			return QueryStrings;
        //		}}
    }

    [Serializable]
    internal class User
    {
        public static string url { get { return "https://sb.oddz.com/api/v1.1/test/user"; } }
        public static string Type { get { return "user"; } }
        //	public NameValueCollection CompleteRequestUrl{get {
        //			var QueryStrings= new NameValueCollection()
        //			{
        //				{"apiKey",apiKey},
        //				{"sessionId",sessionId},
        //				{"api_key",apiKey},
        //			};
        //			return QueryStrings;
        //		}}
    }

    [Serializable]
    internal class Create
    {
        public static string url { get { return "https://sb.oddz.com/api/v1.1/test/user/create"; } }
        public static string Type { get { return "create"; } }
        public string email { get; set; }
        //	public NameValueCollection CompleteRequestUrl{get {
        //			var QueryStrings= new NameValueCollection()
        //			{
        //				{"apiKey",apiKey},
        //				{"sessionId",sessionId},
        //				{"api_key",apiKey},
        //			};
        //			return QueryStrings;
        //		}}
    }

    [Serializable]
    internal class Withdraw
    {
        public static string url { get { return "https://sb.oddz.com/api/v1.1/test/withdraw"; } }
        public static string Type { get { return "withdraw"; } }
        public double availableGameplayBalance { get; set; }
        public double heldCashBalance { get; set; }
        public double availableCashBalance { get; set; }
        public double pendingGameplayBalance { get; set; }
        public double cashBalance { get; set; }
        //	public NameValueCollection CompleteRequestUrl{get {
        //			var QueryStrings= new NameValueCollection()
        //			{
        //				{"apiKey",apiKey},
        //				{"sessionId",sessionId},
        //				{"api_key",apiKey},
        //			};
        //			return QueryStrings;
        //		}}
    }
    #endregion TestOddzRequest
}

