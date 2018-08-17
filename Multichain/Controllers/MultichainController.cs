using Multichain.Models.InputControler;
using Multichain.Models.Services.Multichain;
using System.Web.Http;

namespace Multichain.Controllers
{
    [Authorize]
    public class MultichainController : ApiController
    {
        private readonly IMultichainServices mult;

        public MultichainController()
        {
            mult = new MultichainServices();
        }

        [Route("api/Multichain/ImportAddress")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult ImportAddress()
        {
            mult.SetRequset(this);
            var result = mult.ImportAddress();
            return Ok(result);
        }

        [Route("api/Multichain/GrantPermission")]
        [Authorize]
        [HttpPut]
        public IHttpActionResult GrantPermission([FromBody]Input.GrantPermissionInput input)
        {
            mult.SetRequset(this);
            var result = mult.GrantPermisstion(input);
            return Ok(result);
        }

        [Route("api/Multichain/IssueAsset")]
        [Authorize]
        [HttpPut]
        public IHttpActionResult IssueAsset([FromBody]Input.IssueAssetInput input)
        {
            mult.SetRequset(this);
            var result = mult.IssueAsset(input);
            return Ok(result);
        }

        [Route("api/Multichain/Transaction")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult CreateTransaction([FromBody]Input.CreateTransactionInput input)
        {
            mult.SetRequset(this);
            var result = mult.CreateTransaction(input);
            return Ok(result);
        }

        [Route("api/Multichain/Transaction")]
        [Authorize]
        [HttpPut]
        public IHttpActionResult SignTransaction([FromBody]Input.SignTransactionInput input)
        {
            mult.SetRequset(this);
            var result = mult.SignTransaction(input);
            return Ok(result);
        }

        [Route("api/Multichain/Transaction")]
        [Authorize]
        [HttpPatch]
        public IHttpActionResult SendTransaction([FromBody]Input.SendTransactionInput input)
        {
            mult.SetRequset(this);
            var result = mult.SendTransaction(input);
            return Ok(result);
        }
    }
}
