using Multichain.Models.Services.Multichain;
using System.Web.Http;

namespace Multichain.Controllers
{
    [Authorize]
    public class MultichainController : ApiController
    {
        private readonly IMultichainServices multichainServices;

        public MultichainController()
        {
            multichainServices = new MultichainServices();
        }

        [Route("api/Multichain/ImportAddress")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult ImportAddress()
        {
            multichainServices.SetRequset(this);
            var result = multichainServices.ImportAddress();
            return Ok(result);
        }

        [Route("api/Multichain/GrantPermission")]
        [Authorize]
        [HttpPut]
        public IHttpActionResult GrantPermission([FromBody]GrantPermissionInput input)
        {
            multichainServices.SetRequset(this);
            var result = multichainServices.GrantPermisstion(input);
            return Ok(result);
        }

        [Route("api/Multichain/IssueAsset")]
        [Authorize]
        [HttpPut]
        public IHttpActionResult IssueAsset([FromBody]IssueAssetInput input)
        {
            multichainServices.SetRequset(this);
            var result = multichainServices.IssueAsset(input);
            return Ok(result);
        }

        [Route("api/Multichain/Transaction")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult CreateTransaction([FromBody]CreateTransactionInput input)
        {
            multichainServices.SetRequset(this);
            var result = multichainServices.CreateTransaction(input);
            return Ok(result);
        }

        [Route("api/Multichain/Transaction")]
        [Authorize]
        [HttpPut]
        public IHttpActionResult SignTransaction([FromBody]SignTransactionInput input)
        {
            multichainServices.SetRequset(this);
            var result = multichainServices.SignTransaction(input);
            return Ok(result);
        }

        [Route("api/Multichain/Transaction")]
        [Authorize]
        [HttpPatch]
        public IHttpActionResult SendTransaction([FromBody]SendTransactionInput input)
        {
            multichainServices.SetRequset(this);
            var result = multichainServices.SendTransaction(input);
            return Ok(result);
        }
    }
}
