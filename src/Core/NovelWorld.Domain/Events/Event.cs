﻿using MediatR;

namespace NovelWorld.Domain.Events
{
    public abstract class Event: INotification, ICanSwallowException
    {
        public bool SwallowException { get; set; } = true;
    }
}
