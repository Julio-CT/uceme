using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Uceme.API.Controllers;

public abstract class BaseController : Controller
{
#pragma warning disable CA1051 // Do not declare visible instance fields
#pragma warning disable SA1401 // Fields should be private
    protected readonly ILogger logger;
#pragma warning restore SA1401 // Fields should be private
#pragma warning restore CA1051 // Do not declare visible instance fields

    protected BaseController(ILogger logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected T HandleControllerOperation<T>(Func<T> operation, string errorContext, object? contextId = null)
    {
        try
        {
            return operation();
        }
        catch (Exception ex)
        {
            // Sanitize contextId to avoid log injection from user-provided values
            static string Sanitize(string input)
            {
                if (string.IsNullOrEmpty(input))
                {
                    return "null";
                }

                // Remove CR/LF, braces and other control characters to avoid log injection
                var cleaned = new string(input.Where(c => c != '\r' && c != '\n' && c != '{' && c != '}' && !char.IsControl(c)).ToArray());
                return string.IsNullOrEmpty(cleaned) ? "null" : cleaned;
            }

            string safeContextId = contextId switch
            {
                null => "null",
                string s => Sanitize(s),
                _ => Sanitize(contextId.ToString() ?? "null"),
            };

            // Truncate to a reasonable length to avoid excessively long log entries
            if (safeContextId.Length > 200)
            {
                safeContextId = string.Concat(safeContextId.AsSpan(0, 200), "...");
            }

            this.logger.LogError(ex, "Error {ErrorContext} - ContextId: {ContextId}", errorContext, safeContextId);
            throw;
        }
    }

    protected async Task<T> HandleControllerOperationAsync<T>(Func<Task<T>> operation, string errorContext, object? contextId = null)
    {
        try
        {
            return await operation().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Sanitize contextId to avoid log injection from user-provided values
            static string Sanitize(string input)
            {
                if (string.IsNullOrEmpty(input))
                {
                    return "null";
                }

                // Remove CR/LF, braces and other control characters to avoid log injection
                var cleaned = new string(input.Where(c => c != '\r' && c != '\n' && c != '{' && c != '}' && !char.IsControl(c)).ToArray());
                return string.IsNullOrEmpty(cleaned) ? "null" : cleaned;
            }

            string safeContextId = contextId switch
            {
                null => "null",
                string s => Sanitize(s),
                _ => Sanitize(contextId.ToString() ?? "null"),
            };

            // Truncate to a reasonable length to avoid excessively long log entries
            if (safeContextId.Length > 200)
            {
                safeContextId = string.Concat(safeContextId.AsSpan(0, 200), "...");
            }

            this.logger.LogError(ex, "Error {ErrorContext} - ContextId: {ContextId}", errorContext, safeContextId);
            throw;
        }
    }
}
