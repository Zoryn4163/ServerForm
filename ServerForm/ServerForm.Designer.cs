namespace ServerForm
{
    partial class ServerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerForm));
            this.txtbxLog = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtbxConsole = new System.Windows.Forms.RichTextBox();
            this.notifyTray = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtbxLog
            // 
            this.txtbxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtbxLog.Location = new System.Drawing.Point(0, 0);
            this.txtbxLog.Name = "txtbxLog";
            this.txtbxLog.ReadOnly = true;
            this.txtbxLog.Size = new System.Drawing.Size(548, 381);
            this.txtbxLog.TabIndex = 1;
            this.txtbxLog.Text = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtbxLog);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtbxConsole);
            this.splitContainer1.Size = new System.Drawing.Size(548, 429);
            this.splitContainer1.SplitterDistance = 381;
            this.splitContainer1.TabIndex = 999;
            this.splitContainer1.TabStop = false;
            // 
            // txtbxConsole
            // 
            this.txtbxConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtbxConsole.Location = new System.Drawing.Point(0, 0);
            this.txtbxConsole.Name = "txtbxConsole";
            this.txtbxConsole.ReadOnly = true;
            this.txtbxConsole.Size = new System.Drawing.Size(548, 44);
            this.txtbxConsole.TabIndex = 0;
            this.txtbxConsole.Text = "";
            this.txtbxConsole.Enter += new System.EventHandler(this.txtbxConsole_Enter);
            this.txtbxConsole.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtbxConsole_KeyDown);
            this.txtbxConsole.Leave += new System.EventHandler(this.txtbxConsole_Leave);
            // 
            // notifyTray
            // 
            this.notifyTray.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyTray.BalloonTipText = "The console is running here.";
            this.notifyTray.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyTray.Icon")));
            this.notifyTray.Text = "notifyIcon1";
            this.notifyTray.Visible = true;
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 429);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ServerForm";
            this.Text = "Minecraft Forge Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.ServerForm_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtbxLog;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox txtbxConsole;
        private System.Windows.Forms.NotifyIcon notifyTray;
    }
}

