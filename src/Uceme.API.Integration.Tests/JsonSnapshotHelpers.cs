namespace Uceme.API.Integration.Tests;

using System.Text.Json;
using System.Text.Json.Nodes;

internal static class JsonSnapshotHelpers
{
    /// <summary>
    /// Removes volatile members (e.g. trace identifiers) before snapshotting ASP.NET validation problem JSON.
    /// </summary>
    /// <param name="json">Raw JSON body from the HTTP response.</param>
    /// <returns>Indented JSON without a root <c>traceId</c> property when the payload was an object; otherwise the original string.</returns>
    internal static string RemoveRootTraceId(string json)
    {
        JsonNode? node = JsonNode.Parse(json);
        if (node is JsonObject obj)
        {
            obj.Remove("traceId");
            return obj.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
        }

        return json;
    }
}
