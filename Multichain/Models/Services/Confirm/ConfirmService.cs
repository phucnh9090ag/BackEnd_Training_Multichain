using Multichain.Models.Database;
using Multichain.Models.InputControler;
using Multichain.Models.Services.Multichain;
using System;

namespace Multichain.Models.Services.Confirm
{
    public class ConfirmService:IConfirmService
    {

        private readonly IDatabase database;
        private Address address;
        private IMultichainServices multichainServices;

        public ConfirmService()
        {
            database = new Database.Database();
            multichainServices = new MultichainServices();
        }

        public object Confirm(Input.ComfirmInput input)
        {
            var account = database.FindAccountWithOTP(input.OTP);
            if (account != null)
            {
                try
                {
                    address = multichainServices.CreateAddress();
                    address.email = account.email;

                    database.SaveAddress(address);

                    account.OTP = null;
                    database.getDatabase().SaveChanges();

                    return new { email = account.email, address = address.addr };
                }
                catch (Exception ex)
                {
                    return new { result = ex.Message };
                }
            }
            else
                return new { result = false };
        }
    }
}