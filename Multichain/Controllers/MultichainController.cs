using Multichain.Models.Services.Multichain;
using System.Web.Http;

namespace Multichain.Controllers
{
    [Authorize]
    public class MultichainController : ApiController
    {
        private readonly IMultichainServices _multichainServices;

        public MultichainController()
        {
            _multichainServices = new MultichainServices();
        }

        [Route("api/Multichain/ImportAddress")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult ImportAddress()
        {
            _multichainServices.SetRequset(this);
            var result = _multichainServices.ImportAddress();
            return Ok(result);
        }

        [Route("api/Multichain/GrantPermission")]
        [Authorize]
        [HttpPut]
        public IHttpActionResult GrantPermission([FromBody]GrantPermissionInput input)
        {
            _multichainServices.SetRequset(this);
            var result = _multichainServices.GrantPermisstion(input);
            return Ok(result);
        }

        [Route("api/Multichain/IssueAsset")]
        [Authorize]
        [HttpPut]
        public IHttpActionResult IssueAsset([FromBody]IssueAssetInput input)
        {
            _multichainServices.SetRequset(this);
            var result = _multichainServices.IssueAsset(input);
            return Ok(result);
        }

        [Route("api/Multichain/Transaction")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult CreateTransaction([FromBody]CreateTransactionInput input)
        {
            _multichainServices.SetRequset(this);
            var result = _multichainServices.CreateTransaction(input);
            return Ok(result);
        }

        [Route("api/Multichain/Transaction")]
        [Authorize]
        [HttpPut]
        public IHttpActionResult SignTransaction([FromBody]SignTransactionInput input)
        {
            _multichainServices.SetRequset(this);
            var result = _multichainServices.SignTransaction(input);
            return Ok(result);
        }

        [Route("api/Multichain/Transaction")]
        [Authorize]
        [HttpPatch]
        public IHttpActionResult SendTransaction([FromBody]SendTransactionInput input)
        {
            _multichainServices.SetRequset(this);
            var result = _multichainServices.SendTransaction(input);
            return Ok(result);
        }
    }
}
