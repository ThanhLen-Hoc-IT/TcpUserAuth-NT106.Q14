using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharedModels;

namespace ServerApp
{
    internal class Server
    {
        private TcpListener listener;
        private CancellationTokenSource cts;
        private readonly ConcurrentDictionary<string, ClientHandler> clients = new ConcurrentDictionary<string, ClientHandler>();
        private readonly FrmServerMain ui;

        public bool IsRunning { get; private set; }

        public Server(FrmServerMain form)
        {
            ui = form;
        }

        public void Start(int port = 8888)
        {
            if (IsRunning) return;

            try
            {
                cts = new CancellationTokenSource();
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                IsRunning = true;
                ui.AddLog($"✅ Server started on port {port}");
                _ = AcceptClientsAsync(cts.Token);
            }
            catch (Exception ex)
            {
                ui.AddLog($"❌ Error starting server: {ex.Message}");
            }
        }

        public void Stop()
        {
            if (!IsRunning) return;

            try
            {
                cts.Cancel();
                listener.Stop();

                foreach (var kvp in clients)
                {
                    kvp.Value.Disconnect();
                }
                clients.Clear();

                IsRunning = false;
                ui.AddLog("🛑 Server stopped.");
            }
            catch (Exception ex)
            {
                ui.AddLog($"❌ Error stopping server: {ex.Message}");
            }
        }

        private async Task AcceptClientsAsync(CancellationToken token)
        {
            ui.AddLog("⌛ Waiting for clients...");
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var tcpClient = await listener.AcceptTcpClientAsync();
                    string clientId = Guid.NewGuid().ToString();
                    ui.AddLog($"📥 Client connected: {clientId}");

                    var handler = new ClientHandler(clientId, tcpClient, this, ui);
                    clients[clientId] = handler;

                    _ = Task.Run(() => handler.ProcessAsync(token));
                    ui.UpdateClientList(clients.Keys);
                }
            }
            catch (OperationCanceledException)
            {
                ui.AddLog("🌀 Server stopping...");
            }
            catch (Exception ex)
            {
                ui.AddLog($"❌ Accept error: {ex.Message}");
            }
        }

        public void RemoveClient(string clientId)
        {
            if (clients.TryRemove(clientId, out _))
            {
                ui.AddLog($"🚪 Client disconnected: {clientId}");
                ui.UpdateClientList(clients.Keys);
            }
        }
    }
}
