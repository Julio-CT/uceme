namespace Uceme.UI.Controllers;

using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

public class OidcConfigurationController : Controller
{
    private readonly ILogger<OidcConfigurationController> logger;
    private readonly IClientRequestParametersProvider clientRequestParametersProvider;

    public OidcConfigurationController(
        IClientRequestParametersProvider clientRequestParametersProvider,
        ILogger<OidcConfigurationController> logger)
    {
        this.clientRequestParametersProvider = clientRequestParametersProvider;
        this.logger = logger;
    }

    [HttpGet("_configuration/{clientId}")]
    public IActionResult GetClientRequestParameters([FromRoute] string clientId)
    {
        System.Collections.Generic.IDictionary<string, string> parameters = this.clientRequestParametersProvider.GetClientParameters(this.HttpContext, clientId);
        return this.Ok(parameters);
    }
}
