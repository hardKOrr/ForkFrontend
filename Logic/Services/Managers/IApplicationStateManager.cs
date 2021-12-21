﻿using ProjectAveryCommon.Model.Application;
using ProjectAveryFrontend.Model;

namespace ProjectAveryFrontend.Logic.Services.Managers;

public interface IApplicationStateManager
{
    public delegate void HandleAppStateChanged();

    public delegate void HandleAppStatusChanged();

    public bool IsApplicationReady { get; }
    public State ApplicationState { get; }
    public WebsocketStatus WebsocketStatus { get; set; }
    public ApplicationStatus ApplicationStatus { get; }

    public event HandleAppStatusChanged AppStatusChanged;
    public event HandleAppStateChanged AppStateChanged;

    public Task UpdateState();
}