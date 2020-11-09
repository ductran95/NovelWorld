﻿using System.Threading.Tasks;

namespace NovelWorld.EventBus.EventHandlers
{
    public interface DynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
