using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;


namespace EasyRESTClient
{
    public enum httpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }
    class RestClient
    {
        public string endPoint { get; set; }
        public httpVerb httpMethod { get; set; }

        
        public RestClient()
        {
            endPoint = string.Empty;
            httpMethod = httpVerb.GET;

        }
        public void httpMethodSec(string strHttpMetodu)
        {
            switch(strHttpMetodu)
            {
                case "GET":
                    httpMethod = httpVerb.GET;
                    break;
                case "POST":
                    httpMethod = httpVerb.POST;
                    break;
                case "PUT":
                    httpMethod = httpVerb.PUT;
                    break;
                case "DELETE":
                    httpMethod = httpVerb.DELETE;
                    break;
                default:
                    throw new Exception("HTTP metodu geçerli değil:  "+ strHttpMetodu);
                   
            }
        }
        public void addHeaders(HttpWebRequest rq,List<KeyValuePair<string,string>> headers)
        {
            foreach(var element in headers)
            {
                rq.Headers.Add(element.Key, element.Value);
            }
        }
        public string makeRequest(List<KeyValuePair<string, string>> requestHeaders, List<KeyValuePair<string, string>> dataList)
        {
            string strResponseValue = string.Empty;
            string HTTPRequestMethod = httpMethod.ToString();
            string RequestURL = endPoint;
            if(HTTPRequestMethod=="GET")
            {
                RequestURL += httpGetData(dataList);
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(RequestURL);
            request.Method = HTTPRequestMethod;
            request.ContentType = "application/x-www-form-urlencoded";
            string pdata = postData(dataList);

            if (HTTPRequestMethod!="GET")
            {

                var data = Encoding.UTF8.GetBytes(pdata);
                request.ContentLength = data.Length;
            }

            addHeaders(request, requestHeaders);
            if(HTTPRequestMethod != "GET")
            {
                using (var stream = request.GetRequestStream())
                {
                    var data = Encoding.ASCII.GetBytes(pdata);
                    stream.Write(data, 0, data.Length);
                }
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException("Hata Kodu:" + response.StatusCode.ToString());
                }

                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                        }
                    }
                }
            }

            return strResponseValue;
        }
        private string postData(List<KeyValuePair<string, string>> KeyValue)
        {
            string postString = "";
            //request.ContentLength = data.Length;

            foreach (var element in KeyValue)
            {
                if(element.Key.Trim()!=string.Empty)
                {
                    
                    if(postString!="") { postString += "&"; }
                    postString += element.Key + "=" + HttpUtility.UrlEncode(element.Value);
                }

            }
            return postString;
        }
        private string httpGetData(List<KeyValuePair<string, string>> KeyValue)
        {
            string querystring = "?";
            //request.ContentLength = data.Length;

            foreach (var element in KeyValue)
            {
                if (element.Key.Trim() != string.Empty)
                {

                    if (querystring != "?") { querystring += "&"; }
                    querystring += element.Key + "=";
                    querystring += HttpUtility.UrlEncode(element.Value);
                }

            }
            return querystring;
        }
    }
}
