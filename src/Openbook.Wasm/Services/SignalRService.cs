using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Openbook.Wasm.Services;

public class SignalRService : IAsyncDisposable
{
    private HubConnection? _connection;
    private readonly Dictionary<string, IDisposable> _handlers = new();
    public event Action? Connected;
    public event Action? Disconnected;
    public bool IsConnected => _connection?.State == HubConnectionState.Connected;

    public async Task ConnectAsync(string url, string accessToken)
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
        }
        _connection = new HubConnectionBuilder()
            .WithUrl(url, options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(accessToken)!;
                // options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
                // options.SkipNegotiation = true;
            })
            .WithAutomaticReconnect()
            .Build();
        _connection.Closed += async (error) =>
        {
            Disconnected?.Invoke();
            await Task.CompletedTask;
        };
        await _connection.StartAsync();
        Connected?.Invoke();
    }

    public void On<T>(string method, Action<T> handler)
    {
        if (_connection == null) return;
        Off(method);
        _handlers[method] = _connection.On(method, handler);
    }

    public void Off(string method)
    {
        if (_handlers.TryGetValue(method, out var disp))
        {
            disp.Dispose();
            _handlers.Remove(method);
        }
    }

    public async Task DisconnectAsync()
    {
        if (_connection != null)
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();
            _connection = null;
            foreach (var disp in _handlers.Values) disp.Dispose();
            _handlers.Clear();
            Disconnected?.Invoke();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
        }
        foreach (var disp in _handlers.Values) disp.Dispose();
        _handlers.Clear();
    }
}
