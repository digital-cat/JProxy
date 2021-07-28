
namespace JProxy
{
    partial class FormMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemStart = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemStop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemLogDir = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "JProxy";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseMove += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseMove);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemStatus,
            this.menuItemStart,
            this.menuItemStop,
            this.toolStripSeparator2,
            this.menuItemConfig,
            this.menuItemLogDir,
            this.toolStripSeparator1,
            this.menuItemExit});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(219, 170);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // menuItemStatus
            // 
            this.menuItemStatus.Enabled = false;
            this.menuItemStatus.Name = "menuItemStatus";
            this.menuItemStatus.Size = new System.Drawing.Size(218, 22);
            this.menuItemStatus.Text = "JProxyサーバー停止中(99999)";
            // 
            // menuItemStart
            // 
            this.menuItemStart.Image = global::JProxy.Properties.Resources.start;
            this.menuItemStart.ImageTransparentColor = System.Drawing.Color.White;
            this.menuItemStart.Name = "menuItemStart";
            this.menuItemStart.Size = new System.Drawing.Size(218, 22);
            this.menuItemStart.Text = "　開始(&S)";
            this.menuItemStart.Click += new System.EventHandler(this.menuItemStart_Click);
            // 
            // menuItemStop
            // 
            this.menuItemStop.Image = global::JProxy.Properties.Resources.stop;
            this.menuItemStop.ImageTransparentColor = System.Drawing.Color.White;
            this.menuItemStop.Name = "menuItemStop";
            this.menuItemStop.Size = new System.Drawing.Size(218, 22);
            this.menuItemStop.Text = "　停止(&T)";
            this.menuItemStop.Click += new System.EventHandler(this.menuItemStop_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(215, 6);
            // 
            // menuItemConfig
            // 
            this.menuItemConfig.Image = global::JProxy.Properties.Resources.config;
            this.menuItemConfig.Name = "menuItemConfig";
            this.menuItemConfig.Size = new System.Drawing.Size(218, 22);
            this.menuItemConfig.Text = "設定(&C)...";
            this.menuItemConfig.Click += new System.EventHandler(this.menuItemConfig_Click);
            // 
            // menuItemLogDir
            // 
            this.menuItemLogDir.Image = global::JProxy.Properties.Resources.log;
            this.menuItemLogDir.Name = "menuItemLogDir";
            this.menuItemLogDir.Size = new System.Drawing.Size(218, 22);
            this.menuItemLogDir.Text = "ログフォルダーを開く(&L)...";
            this.menuItemLogDir.Click += new System.EventHandler(this.menuItemLogDir_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(215, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Image = global::JProxy.Properties.Resources.exit;
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.Size = new System.Drawing.Size(218, 22);
            this.menuItemExit.Text = "JProxy終了(&X)";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 267);
            this.Name = "FormMain";
            this.ShowInTaskbar = false;
            this.Text = "JProxy";
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfig;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuItemStart;
        private System.Windows.Forms.ToolStripMenuItem menuItemStop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemStatus;
        private System.Windows.Forms.ToolStripMenuItem menuItemLogDir;
    }
}

