using Multichain.Controllers;
using MultiChain.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multichain.Models
{
    interface IMultichainServices
    {
        void SetRequset(MultichainController multichainControler);
        object ImportAddress();
        object GrantPermisstion(Input.GrantPermissionInput input);
        object IssueAsset(Input.IssueAssetInput input);
        object CreateTransaction(Input.CreateTransactionInput input);
        object SignTransaction(Input.SignTransactionInput input);
        object SendTransaction(Input.SendTransactionInput input);
    }
}
