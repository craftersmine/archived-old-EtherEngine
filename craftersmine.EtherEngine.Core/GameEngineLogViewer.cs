using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using craftersmine.EtherEngine.Utils;

namespace craftersmine.EtherEngine.Core
{
    public partial class GameEngineLogViewer : Form
    {
        private Logger _logger { get; set; }

        public GameEngineLogViewer(Logger logger)
        {
            InitializeComponent();
            _logger = logger;
            this.updateTimer.Start();
        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() => {
                this.Close();
            }));
        }

        private void UpdateLog()
        {
            log.Items.Clear();
            this.SuspendLayout();
            for (int i = 0; i < _logger.LogEntries.Count; i++)
            {
                log.Invoke(new Action(() => { log.Items.Add(new ListViewItem(new string[] { null, _logger.LogEntries[i].EntryDateTime, _logger.LogEntries[i].Type.ToString(), _logger.LogEntries[i].Contents }, (int)_logger.LogEntries[i].Type)); }));
            }

            if (autoscroll.Checked)
                log.Items[log.Items.Count - 1].EnsureVisible();
            this.ResumeLayout();
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            this.Invoke(new Action(() => { UpdateLog(); }));
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() => { UpdateLog(); }));
        }

        private void GameEngineLogViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            GameUpdater.IsLogViewerVisible = false;
        }
    }
}
