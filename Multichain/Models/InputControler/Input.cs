using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multichain.Models.InputControler
{
    public class Input
    {
        public class SignUpInput
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        public class ComfirmInput
        {
            public string OTP { get; set; }
        }

        public class LoginInput
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        public class GrantPermissionInput
        {
            public string address { get; set; }
            public bool isAdmin { get; set; }
            public bool isReceive { get; set; }
            public bool isSend { get; set; }
            public bool isConnect { get; set; }
            public bool isCreate { get; set; }
            public bool isIssue { get; set; }
            public bool isMine { get; set; }
            public bool isActivate { get; set; }
        }

        public class IssueAssetInput
        {
            public string address { get; set; }
            public string assetName { get; set; }
            public string note { get; set; }
            public int qty { get; set; }
            public double unit { get; set; }
        }

        public class CreateTransactionInput
        {
            public string addressFrom { get; set; }
            public string addressTo { get; set; }
            public string assetName { get; set; }
            public int qty { get; set; }
        }

        public class SignTransactionInput
        {
            public string addressSign { get; set; }
            public string hexValue { get; set; }
        }

        public class SendTransactionInput
        {
            public string hexValue { get; set; }
        }
    }
}