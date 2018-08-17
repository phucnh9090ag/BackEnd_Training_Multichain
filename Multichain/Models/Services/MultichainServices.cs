using Multichain.Controllers;
using MultiChain.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Web.Configuration;

namespace Multichain.Models
{
    public class MultichainServices:IMultichainServices
    {

        MultichainController MultCtrl;
        DBEntity database;

        string chainname = WebConfigurationManager.AppSettings["chainname"];
        string rpc_userame = WebConfigurationManager.AppSettings["rpc_userame"];
        string rpc_password = WebConfigurationManager.AppSettings["rpc_password"];
        string networdid = WebConfigurationManager.AppSettings["networdid"];
        int port = Convert.ToInt32(WebConfigurationManager.AppSettings["port"]);

        Account _acc;


        JsonRpcClient jsonRPCClient;
        AddressManager addrMng;
        PermissionManager permissonMng;
        AssetManager assetMng;
        TransactionManager transactionMng;

        public MultichainServices()
        {
            this.database = new DBEntity();

            jsonRPCClient = new JsonRpcClient(rpc_userame, rpc_password, networdid, port, chainname);

            addrMng = new AddressManager(jsonRPCClient);
            permissonMng = new PermissionManager(jsonRPCClient);
            assetMng = new AssetManager(jsonRPCClient);
            transactionMng = new TransactionManager(jsonRPCClient);
        }

        public void SetRequset(MultichainController multichainControler)
        {
            this.MultCtrl = multichainControler;
            this._acc = AccountRequest();
        }


        private Account AccountRequest()
        {
            var _bearerToken = MultCtrl.Request.Headers.Authorization.ToString().Substring(7);
            var _acc = database.Accounts.SingleOrDefault(acc => acc.beartoken == _bearerToken);
            return 
                _acc;
        }

        public object ImportAddress()
        {
            var _addr = CreateAddress();
            if (_acc != null)
            {
                _addr.email = _acc.email;
                database.Addresses.Add(_addr);
                database.SaveChanges();
                return JObject.FromObject(new { email = _addr.email, address = _addr.addr});
            }
            else
                return 
                    JObject.FromObject(new { error = Properties.Resources.AccountNotFound });
        }

        internal Address CreateAddress()
        {
            var json = JObject.Parse(addrMng.CreateKeypairs());

            var _address = json["result"][0]["address"].ToString();
            var _privateKey = json["result"][0]["privkey"].ToString();

            addrMng.ImportAddress(_address);
            addrMng.ImportPrivateKey(_privateKey);

            return
                new Address { addr = _address, privateKey = _privateKey };
        }

        public object GrantPermisstion(Input.GrantPermissionInput input)
        { 
            if (!AuthAccountWithBearerToken(input.address))
                return
                    JObject.FromObject(new { address = input.address, message = Properties.Resources.UnValidAddress });

            var _permission = ReadGrantFromBody(input);

            var json = permissonMng.Grant(input.address, _permission);

            var isError = (JObject.Parse(json).GetValue("error").ToString() == "");

            if (isError)
                return
                    JObject.FromObject(new { address = input.address, permission = _permission, message = Properties.Resources.GrantSucess });
            else
                return
                    JObject.FromObject(new { address = input.address, permission = _permission, message = Properties.Resources.GrantFail });
        }

        private string ReadGrantFromBody(Input.GrantPermissionInput input)
        {
            var permission = "";

            if (input.isAdmin)
                permission += Permission.admin.ToString();

            if (input.isReceive)
            {
                if (permission.Length > 0)
                    permission += ",";
                permission += Permission.receive.ToString();
            }

            if (input.isSend)
            {
                if (permission.Length > 0)
                    permission += ",";
                permission += Permission.send.ToString();
            }

            if (input.isConnect)
            {
                if (permission.Length > 0)
                    permission += ",";else
                permission += Permission.connect.ToString();
            }

            if (input.isCreate)
            {
                if (permission.Length > 0)
                    permission += ",";
                permission += Permission.create.ToString();
            }

            if (input.isIssue)
            {
                if (permission.Length > 0)
                    permission += ",";
                permission += Permission.issue.ToString();
            }

            if (input.isMine)
            {
                if (permission.Length > 0)
                    permission += ",";
                
                permission += Permission.mine.ToString();
            }

            if (input.isActivate)
            {
                if (permission.Length > 0)
                    permission += ",";
                permission += Permission.activate.ToString();
            }
            return permission;
        }

        public object IssueAsset(Input.IssueAssetInput input)
        {
            if (!AuthAccountWithBearerToken(input.address))
                return 
                    JObject.FromObject(new { error = Properties.Resources.AccountNotFound });

            var json = assetMng.IssueAsset(input.address, input.assetName, input.qty, input.unit, input.note);

            var isError = (JObject.Parse(json).GetValue("error").ToString() == "");

            if (isError)
                return
                    JObject.FromObject(new { message = Properties.Resources.IssueSucess, assetName = input.assetName, qty = input.qty, unit = input.unit, note = input.note });
            else
                return
                    JObject.FromObject(new { message = JObject.Parse(json).GetValue("error").ToString() });
        }

        public object CreateTransaction(Input.CreateTransactionInput input)
        {
            if (!AuthAccountWithBearerToken(input.addressFrom))
                return 
                    JObject.FromObject(new { error = Properties.Resources.AccountNotFound });

            var result = transactionMng.CreateRawSendFrom(input.addressFrom, input.addressTo, input.assetName, input.qty);

            return 
                JObject.Parse(result);
        }

        public object SignTransaction(Input.SignTransactionInput input)
        {
            var t = database.Addresses.SingleOrDefault(address => address.email == _acc.email && address.addr == input.addressSign);
            if (t == null)
                return 
                    JObject.FromObject(new { error = Properties.Resources.AccountNotFound });

            var privateKey = t.privateKey;

            var result = transactionMng.SignRawTransaction(input.hexValue, privateKey);
            return 
                JObject.Parse(result);
        }

        public object SendTransaction(Input.SendTransactionInput input)
        {
            var result = transactionMng.SendRawTransaction(input.hexValue);
            return 
                JObject.Parse(result);
        }

        private bool AuthAccountWithBearerToken(string inputAddress)
        {
            var t = database.Addresses.SingleOrDefault(address => address.email == _acc.email && address.addr == inputAddress);
            if (t == null)
                return false;
            return true;
        }
    }
}