using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemp.Services.Sms
{
    public interface ISmsService
    {
        #region Auth
        bool SendFastVerificationCode(string mobile, string code, string token);

        bool SendWithParams(string mobile, string[] message, string token);
        #endregion

    }
}
