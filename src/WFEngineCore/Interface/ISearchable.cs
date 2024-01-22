using System;
using System.Collections.Generic;

namespace WFEngineCore.Interface
{
    public interface ISearchable
    {
        IEnumerable<string> GetSearchTokens();
    }
}
