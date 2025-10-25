using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ServerApp
{
    public partial class FrmServerMain : Form
    {
        private Server server;

        public FrmServerMain()
        {
            InitializeComponent();
            server = new Server(this);
        }

        private void FrmServerMain_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "Server starting...";
            server.Start(8080);
            AddLog("✅ Server đang chạy trên cổng 8080...");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            server.Start(8080);
            AddLog("✅ Server đang chạy trên cổng 8080...");
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            server.Stop();
            lblStatus.Text = "🛑 Server stopped";
        }

        public void AddLog(string msg)
        {
            if (txtLog.InvokeRequired)
                txtLog.Invoke(new Action(() => AddLog(msg)));
            else
                txtLog.AppendText($"{DateTime.Now:HH:mm:ss} - {msg}{Environment.NewLine}");
        }

        public void UpdateClientList(IEnumerable<string> clientIds)
        {
            if (lvClients.InvokeRequired)
                lvClients.Invoke(new Action(() => UpdateClientList(clientIds)));
            else
            {
                lvClients.Items.Clear();
                foreach (var id in clientIds)
                    lvClients.Items.Add(id);

                lblClients.Text = $"Clients: {lvClients.Items.Count}";
            }
        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
