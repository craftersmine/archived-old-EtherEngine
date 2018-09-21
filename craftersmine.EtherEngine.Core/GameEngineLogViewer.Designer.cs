namespace craftersmine.EtherEngine.Core
{
    partial class GameEngineLogViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameEngineLogViewer));
            this.log = new System.Windows.Forms.ListView();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.refresh = new System.Windows.Forms.Button();
            this.close = new System.Windows.Forms.Button();
            this.iconCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.typeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contentsCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.datetimeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.logIcons = new System.Windows.Forms.ImageList(this.components);
            this.autoscroll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // log
            // 
            this.log.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.iconCol,
            this.datetimeCol,
            this.typeCol,
            this.contentsCol});
            this.log.FullRowSelect = true;
            this.log.Location = new System.Drawing.Point(0, 0);
            this.log.MultiSelect = false;
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(713, 454);
            this.log.SmallImageList = this.logIcons;
            this.log.TabIndex = 0;
            this.log.UseCompatibleStateImageBehavior = false;
            this.log.View = System.Windows.Forms.View.Details;
            // 
            // updateTimer
            // 
            this.updateTimer.Interval = 500;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(12, 460);
            this.refresh.Name = "refresh";
            this.refresh.Size = new System.Drawing.Size(146, 23);
            this.refresh.TabIndex = 2;
            this.refresh.Text = "Refresh";
            this.refresh.UseVisualStyleBackColor = true;
            this.refresh.Click += new System.EventHandler(this.refresh_Click);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(626, 460);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 3;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // iconCol
            // 
            this.iconCol.Text = "";
            this.iconCol.Width = 25;
            // 
            // typeCol
            // 
            this.typeCol.Text = "Type";
            this.typeCol.Width = 100;
            // 
            // contentsCol
            // 
            this.contentsCol.Text = "Contents";
            this.contentsCol.Width = 450;
            // 
            // datetimeCol
            // 
            this.datetimeCol.Text = "Date/Time";
            this.datetimeCol.Width = 100;
            // 
            // logIcons
            // 
            this.logIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("logIcons.ImageStream")));
            this.logIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.logIcons.Images.SetKeyName(0, "info.png");
            this.logIcons.Images.SetKeyName(1, "error.png");
            this.logIcons.Images.SetKeyName(2, "warning.png");
            this.logIcons.Images.SetKeyName(3, "done.png");
            this.logIcons.Images.SetKeyName(4, "stacktrace.png");
            this.logIcons.Images.SetKeyName(5, "critical.png");
            this.logIcons.Images.SetKeyName(6, "success.png");
            this.logIcons.Images.SetKeyName(7, "connection.png");
            this.logIcons.Images.SetKeyName(8, "crash.png");
            this.logIcons.Images.SetKeyName(9, "unknown.png");
            this.logIcons.Images.SetKeyName(10, "debug.png");
            // 
            // autoscroll
            // 
            this.autoscroll.AutoSize = true;
            this.autoscroll.Checked = true;
            this.autoscroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoscroll.Location = new System.Drawing.Point(164, 464);
            this.autoscroll.Name = "autoscroll";
            this.autoscroll.Size = new System.Drawing.Size(119, 17);
            this.autoscroll.TabIndex = 4;
            this.autoscroll.Text = "Autoscroll to bottom";
            this.autoscroll.UseVisualStyleBackColor = true;
            // 
            // GameEngineLogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 493);
            this.Controls.Add(this.autoscroll);
            this.Controls.Add(this.close);
            this.Controls.Add(this.refresh);
            this.Controls.Add(this.log);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameEngineLogViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EtherEngine Debug Realtime Log Viewer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GameEngineLogViewer_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView log;
        private System.Windows.Forms.ColumnHeader iconCol;
        private System.Windows.Forms.ColumnHeader typeCol;
        private System.Windows.Forms.ColumnHeader contentsCol;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Button refresh;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.ColumnHeader datetimeCol;
        private System.Windows.Forms.ImageList logIcons;
        private System.Windows.Forms.CheckBox autoscroll;
    }
}