﻿using ProjectAveryCommon.Model.Application;
using ProjectAveryFrontend.Logic.Services.Connections;
using ProjectAveryFrontend.Model;

namespace ProjectAveryFrontend.Logic.Services.Managers;

public class ApplicationStateManager : IApplicationStateManager
{
    private readonly ILogger<ApplicationStateManager> _logger;
    private readonly IApplicationConnectionService _applicationConnection;

    public ApplicationStateManager(ILogger<ApplicationStateManager> logger, IApplicationConnectionService applicationConnection)
    {
        _logger = logger;
        _applicationConnection = applicationConnection;
    }

    private bool _isStateReady = false;
    private WebsocketStatus _websocketStatus = WebsocketStatus.Disconnected;

    public event IApplicationStateManager.HandleAppStatusChanged? AppStatusChanged;
    public bool IsApplicationReady => _isStateReady && WebsocketStatus == WebsocketStatus.Connected;
    public State ApplicationState { get; private set; }

    public WebsocketStatus WebsocketStatus
    {
        get => _websocketStatus;
        set
        {
            _websocketStatus = value;
            AppStatusChanged?.Invoke();
        }
    }

    public ApplicationStatus ApplicationStatus {
        get
        {
            if (!_isStateReady)
            {
                return ApplicationStatus.RetrievingState;
            }
            if (WebsocketStatus != WebsocketStatus.Connected)
            {
                return ApplicationStatus.WaitingForWebsocket;
            }
            return ApplicationStatus.Ready;
        }
    }
    
    public async Task UpdateState()
    {
        _isStateReady = false;
        _logger.LogInformation("Refreshing application state...");
        ApplicationState = await _applicationConnection.GetApplicationState();
        _isStateReady = true;
        AppStatusChanged?.Invoke();
    }
}