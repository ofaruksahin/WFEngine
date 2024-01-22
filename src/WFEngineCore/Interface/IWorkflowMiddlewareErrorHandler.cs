using System;
using System.Threading.Tasks;

namespace WFEngineCore.Interface
{
    /// <summary>
    /// Handles exceptions within workflow middleware.
    /// </summary>
    public interface IWorkflowMiddlewareErrorHandler
    {
        /// <summary>
        /// Asynchronously handle the given exception.
        /// </summary>
        /// <param name="ex">The exception to handle</param>
        /// <returns>A task that completes when handling is done.</returns>
        Task HandleAsync(Exception ex);
    }
}
