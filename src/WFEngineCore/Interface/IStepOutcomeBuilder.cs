﻿using System;
using WFEngineCore.Models;
using WFEngineCore.Primitives;

namespace WFEngineCore.Interface
{   
    public interface IStepOutcomeBuilder<TData>
    {
        IWorkflowBuilder<TData> WorkflowBuilder { get; }

        ValueOutcome Outcome { get; }

        IStepBuilder<TData, TStep> Then<TStep>(Action<IStepBuilder<TData, TStep>> stepSetup = null) where TStep : IStepBody;

        IStepBuilder<TData, TStep> Then<TStep>(IStepBuilder<TData, TStep> step) where TStep : IStepBody;

        IStepBuilder<TData, InlineStepBody> Then(Func<IStepExecutionContext, ExecutionResult> body);

        void EndWorkflow();
    }
}