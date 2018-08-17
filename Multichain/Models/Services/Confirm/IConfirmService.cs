using Multichain.Models.InputControler;

namespace Multichain.Models.Services.Confirm
{
    interface IConfirmService
    {
        object Confirm(Input.ComfirmInput input);
    }
}
