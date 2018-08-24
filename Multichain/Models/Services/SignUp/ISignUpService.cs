using Multichain.Models.Database;

namespace Multichain.Models.Services.SignUp
{
    interface ISignUpService
    {
        object SignUp(SignUpInput input);
        string CreateOTP();
        bool SaveDatabase(Account account);
        bool isAccountValid(Account account);
    }
}
