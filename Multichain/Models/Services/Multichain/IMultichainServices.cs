using Multichain.Controllers;
using Multichain.Models.Database;

namespace Multichain.Models.Services.Multichain
{
    interface IMultichainServices
    {
        void SetRequset(MultichainController multichainControler);
        object ImportAddress();
        Address CreateAddress();
        object GrantPermisstion(GrantPermissionInput input);
        object IssueAsset(IssueAssetInput input);
        object CreateTransaction(CreateTransactionInput input);
        object SignTransaction(SignTransactionInput input);
        object SendTransaction(SendTransactionInput input);
    }
}
