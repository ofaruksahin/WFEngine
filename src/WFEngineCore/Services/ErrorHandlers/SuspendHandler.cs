﻿using System;
using System.Collections.Generic;
using WFEngineCore.Interface;
using WFEngineCore.Models;
using WFEngineCore.Models.LifeCycleEvents;

namespace WFEngineCore.Services.ErrorHandlers
{
    public class SuspendHandler : IWorkflowErrorHandler
    {
        private readonly ILifeCycleEventPublisher _eventPublisher;
        private readonly IDateTimeProvider _datetimeProvider;
        public WorkflowErrorHandling Type => WorkflowErrorHandling.Suspend;

        public SuspendHandler(ILifeCycleEventPublisher eventPublisher, IDateTimeProvider datetimeProvider)
        {
            _eventPublisher = eventPublisher;
            _datetimeProvider = datetimeProvider;
        }

        public void Handle(WorkflowInstance workflow, WorkflowDefinition def, ExecutionPointer pointer, WorkflowStep step, Exception exception, Queue<ExecutionPointer> bubbleUpQueue)
        {
            workflow.Status = WorkflowStatus.Suspended;
            _eventPublisher.PublishNotification(new WorkflowSuspended
            {
                EventTimeUtc = _datetimeProvider.UtcNow,
                Reference = workflow.Reference,
                WorkflowInstanceId = workflow.Id,
                WorkflowDefinitionId = workflow.WorkflowDefinitionId,
                Version = workflow.Version
            });

            step.PrimeForRetry(pointer);
        }
    }
}
