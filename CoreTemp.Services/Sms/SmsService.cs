using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreTemp.Services.Sms
{
    public class SmsService : ISmsService
    {
        public SmsService()
        {
        }
        #region Auth
        public bool SendFastVerificationCode(string mobile, string code, string token)
        {
            try
            {
                var client = new RestClient("http://api.smsapp.ir/v2/send/verify");
                var request = new RestRequest(Method.POST);
                request.AddHeader("apikey", "ZxNgP69qCfh8JBqH2lGn8oxchBpYo3B7rEUxmEcrARY");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("param1", code);
                request.AddParameter("type", 1);
                request.AddParameter("receptor", mobile);
                request.AddParameter("template", token);


                IRestResponse response = client.Execute(request);
                var s = response.ErrorMessage;
                var ss = response.StatusDescription;

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool SendWithParams(string mobile, string[] message, string token)
        {
            try
            {
                int count = message.Count();

                var client = new RestClient("http://api.smsapp.ir/v2/send/verify");
                var request = new RestRequest(Method.POST);
                request.AddHeader("apikey", "ZxNgP69qCfh8JBqH2lGn8oxchBpYo3B7rEUxmEcrARY");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                foreach (var item in message)
                {
                    int i = 1;
                    string param = "param" + i;
                    request.AddParameter(param, item);
                }
                //request.AddParameter("param1", message1);
                //request.AddParameter("param2", message2);
                //request.AddParameter("param3", message3);
                request.AddParameter("type", 1);
                request.AddParameter("receptor", mobile);
                request.AddParameter("template", token);


                IRestResponse response = client.Execute(request);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        #region KavenegarApi
        //public static void SendSms2(string number, string message)
        //{
        //    try
        //    {
        //        Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi("544151395731697A5A6441685555497A796A494D516F5078674E764F34745365");
        //        var result = api.Send("1000596446", number, message);
        //        if (result.Status == 200)
        //        {
        //            Console.Write("Message : " + "انجام شد");
        //        }

        //    }
        //    catch (Kavenegar.Exceptions.ApiException ex)
        //    {
        //        // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.
        //        Console.Write("Message : " + ex.Message);
        //    }
        //    catch (Kavenegar.Exceptions.HttpException ex)
        //    {
        //        // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد
        //        Console.Write("Message : " + ex.Message);
        //    }
        //}

        //public static void SendSms3(string number, string message, string token)
        //{
        //    try
        //    {
        //        var client = new RestClient("https://api.kavenegar.com/v1/544151395731697A5A6441685555497A796A494D516F5078674E764F34745365/verify/lookup.json");
        //        var request = new RestRequest(Method.GET);
        //        //request.AddHeader("apikey", "41e59a64b0ae281dfb72ac45aeb2bcb2c8857ddfa5b408b677c8b2e503561cf3");
        //        //request.AddParameter("message", message);
        //        request.AddParameter("receptor", number);
        //        //request.AddParameter("senddate", " 1508144471 ");
        //        //request.AddParameter("linenumber", "10008566");
        //        //request.AddParameter("checkid", " 2020 ");
        //        //request.AddParameter("type", 1);
        //        request.AddParameter("template", token);
        //        request.AddParameter("token", message);

        //        IRestResponse response = client.Execute(request);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public static void SendSms3(string number, string message, string token, string message2 = "" )
        //{
        //    try
        //    {
        //        var client = new RestClient("https://api.kavenegar.com/v1/544151395731697A5A6441685555497A796A494D516F5078674E764F34745365/verify/lookup.json");
        //        var request = new RestRequest(Method.GET);
        //        //request.AddHeader("apikey", "41e59a64b0ae281dfb72ac45aeb2bcb2c8857ddfa5b408b677c8b2e503561cf3");
        //        //request.AddParameter("message", message);
        //        request.AddParameter("receptor", number);
        //        //request.AddParameter("senddate", " 1508144471 ");
        //        //request.AddParameter("linenumber", "10008566");
        //        //request.AddParameter("checkid", " 2020 ");
        //        //request.AddParameter("type", 1);
        //        request.AddParameter("template", token);
        //        request.AddParameter("token", message);

        //        request.AddParameter("token2", message2);
        //        IRestResponse response = client.Execute(request);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public static void SendSms3(string number, string message, string token, string message2, string message3)
        //{
        //    try
        //    {
        //        var client = new RestClient("https://api.kavenegar.com/v1/544151395731697A5A6441685555497A796A494D516F5078674E764F34745365/verify/lookup.json");
        //        var request = new RestRequest(Method.GET);
        //        //request.AddHeader("apikey", "41e59a64b0ae281dfb72ac45aeb2bcb2c8857ddfa5b408b677c8b2e503561cf3");
        //        //request.AddParameter("message", message);
        //        request.AddParameter("receptor", number);
        //        //request.AddParameter("senddate", " 1508144471 ");
        //        //request.AddParameter("linenumber", "10008566");
        //        //request.AddParameter("checkid", " 2020 ");
        //        //request.AddParameter("type", 1);
        //        request.AddParameter("template", token);
        //        request.AddParameter("token", message);

        //        request.AddParameter("token2", message2);
        //        request.AddParameter("token3", message3);
        //        IRestResponse response = client.Execute(request);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        #endregion



    }
}
