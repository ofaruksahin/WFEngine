using System;
using System.Threading.Tasks;
using WFEngineCore.Models;
using WFEngineCore.Models.Search;

namespace WFEngineCore.Interface
{
    public interface ISearchIndex
    {
        Task IndexWorkflow(WorkflowInstance workflow);

        Task<Page<WorkflowSearchResult>> Search(string terms, int skip, int take, params SearchFilter[] filters);

        Task Start();

        Task Stop();
    }
}
