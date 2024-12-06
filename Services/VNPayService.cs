using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ABCDMall.Services
{
    public class VNPayService
    {
        private static readonly string VNP_URL = ConfigurationManager.AppSettings["vnp_url"];
        private static readonly string VNP_API = ConfigurationManager.AppSettings["vnp_api"];
        private static readonly string VNP_TMNCODE = ConfigurationManager.AppSettings["vnp_tmncode"];
        private static readonly string VNP_HASHSECRET = ConfigurationManager.AppSettings["vnp_hashsecret"];
        private static readonly string VNP_VERSION = ConfigurationManager.AppSettings["vnp_version"];
        private SortedList<String, String> _requestData = new SortedList<String, String>(new VnPayCompare());
        private SortedList<String, String> _responseData = new SortedList<String, String>(new VnPayCompare());

        private void AddRequestData(string key, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        private void AddResponseData(string key, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        private string GetResponseData(string key)
        {
            string retValue;
            if (_responseData.TryGetValue(key, out retValue))
            {
                return retValue;
            }
            else
            {
                return string.Empty;
            }
        }

        public string CreateRequestUrl(decimal amount, string orderInfo, string returnUrl)
        {
            AddRequestData("vnp_Version", VNP_VERSION);
            AddRequestData("vnp_Command", "pay");
            AddRequestData("vnp_TmnCode", VNP_TMNCODE);
            AddRequestData("vnp_Amount", (Math.Round(amount * 100)).ToString());
            AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            AddRequestData("vnp_CurrCode", "VND");
            AddRequestData("vnp_IpAddr", GetIpAddress());
            AddRequestData("vnp_Locale", "en");
            AddRequestData("vnp_OrderInfo", orderInfo);
            AddRequestData("vnp_OrderType", "other");
            AddRequestData("vnp_ReturnUrl", returnUrl);
            AddRequestData("vnp_TxnRef", DateTime.Now.ToString("yyyyMMddHHmmss"));
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in _requestData)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            string queryString = data.ToString();
            string baseUrl = VNP_URL;
            baseUrl += "?" + queryString;
            String signData = queryString;
            if (signData.Length > 0)
            {
                signData = signData.Remove(data.Length - 1, 1);
            }
            string vnp_SecureHash = HmacSHA512(VNP_HASHSECRET, signData);
            baseUrl += "vnp_SecureHash=" + vnp_SecureHash;
            return baseUrl;
        }

        public bool ValidateSignature(HttpRequestBase request)
        {
            if (request.QueryString.Count <= 0)
            {
                return false;
            }
            NameValueCollection vnpayData = request.QueryString;
            foreach (string s in vnpayData)
            {
                if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                {
                    AddResponseData(s, vnpayData[s]);
                }
            }
            String vnp_SecureHash = request.QueryString["vnp_SecureHash"];
            string rspRaw = GetResponseData();
            string myChecksum = HmacSHA512(VNP_HASHSECRET, rspRaw);
            if (!myChecksum.Equals(vnp_SecureHash, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            string vnp_ResponseCode = GetResponseData("vnp_ResponseCode");
            string vnp_TransactionStatus = GetResponseData("vnp_TransactionStatus");
            if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
            {
                return true;
            }
            return false;
        }

        private string GetResponseData()
        {

            StringBuilder data = new StringBuilder();
            if (_responseData.ContainsKey("vnp_SecureHashType"))
            {
                _responseData.Remove("vnp_SecureHashType");
            }
            if (_responseData.ContainsKey("vnp_SecureHash"))
            {
                _responseData.Remove("vnp_SecureHash");
            }
            foreach (KeyValuePair<string, string> kv in _responseData)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }
            return data.ToString();
        }

        private string GetIpAddress()
        {
            string ipAddress;
            try
            {
                ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(ipAddress) || (ipAddress.ToLower() == "unknown") || ipAddress.Length > 45)
                    ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            catch (Exception ex)
            {
                ipAddress = "Invalid IP:" + ex.Message;
            }

            return ipAddress;
        }

        private String HmacSHA512(string key, String inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }
    }
}