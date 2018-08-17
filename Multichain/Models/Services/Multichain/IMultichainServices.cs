using Multichain.Controllers;
using Multichain.Models.Database;
using Multichain.Models.InputControler;
using MultiChain.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multichain.Models.Services.Multichain
{
    interface IMultichainServices
    {
        void SetRequset(MultichainController multichainControler);
        object ImportAddress();
        Address CreateAddress();
        object GrantPermisstion(Input.GrantPermissionInput input);
        object IssueAsset(Input.IssueAssetInput input);
        object CreateTransaction(Input.CreateTransactionInput input);
        object SignTransaction(Input.SignTransactionInput input);
        object SendTransaction(Input.SendTransactionInput input);
    }
}
