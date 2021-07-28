using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace JProxy
{
    /// <summary>
    /// メインフォーム
    /// ※ フォームは非表示でアプリケーションのメイン処理を行う
    /// </summary>
    public partial class FormMain : Form
    {
        // 定数
        private const string STATUS_WORK = "JProxyサーバー稼働中";
        private const string STATUS_STOP = "JProxyサーバー停止中";
        private readonly Color COLOR_WORK = Color.Red;
        private readonly Color COLOR_STOP = Color.Black;

        /// <summary>コンテキストメニューラベル（サーバー稼働状態）</summary>
        private ToolStripLabel statusLabel = new ToolStripLabel(STATUS_STOP);

        /// <summary>設定画面表示状態</summary>
        private bool showConfig = false;

        /// <summary>アプリケーションのデータフォルダーパス</summary>
        private string dataDir = string.Empty;

        /// <summary>アプリケーションの設定ファイルパス</summary>
        private string cfgPath = string.Empty;

        /// <summary>アプリケーションのログフォルダーパス</summary>
        private string logDir = string.Empty;

        /// <summary>設定情報</summary>
        private Config config = new Config();

        /// <summary>ログ出力管理</summary>
        private LogManager log = new LogManager();

        /// <summary>HTTPサーバー</summary>
        private HttpServer server = new HttpServer();


        public FormMain()
        {
            InitializeComponent();

            init();
        }

        /// <summary>
        /// 初期処理
        /// </summary>
        private void init()
        {
            try
            {
                // exeパスをベースに各パスを作成
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var dir = Path.GetDirectoryName(new Uri(codeBase).LocalPath);
                dataDir = Path.Combine(dir, "AppData");
                cfgPath = Path.Combine(dataDir, "Config.xml");
                logDir = Path.Combine(dataDir, "Log");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                // 存在しないフォルダーは作成
                if (!Directory.Exists(dataDir))
                {
                    Directory.CreateDirectory(dataDir);
                }
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }

                // ログ出力設定
                log.logDir = logDir;
                log.RemoveOldFiles();
                server.SetLog(ref log);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                // コンテキストメニューのサーバー状態表示をラベルに差し替え（メニュー項目はコンテキストメニュー幅確保のためのダミー）
                contextMenuStrip.Items.Insert(0, statusLabel);
                menuItemStatus.Visible = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                // 設定ファイル読み込み
                config.path = cfgPath;
                config.Load();

                // 起動時自動開始
                if (config.autoStart)
                {
                    server.Start(config.port);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// タスクトレイアイコンのコンテキストメニュー オープン時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (showConfig)
            {
                e.Cancel = true;  // 設定画面表示中はメニュー表示不可
            }
            else
            {
                var working = server.working;
                statusLabel.Text = $"{(working ? STATUS_WORK : STATUS_STOP)}({config.port})";
                statusLabel.ForeColor = working ? COLOR_WORK : COLOR_STOP;
                menuItemStart.Enabled = !working;
                menuItemStop.Enabled = working;
            }
        }

        /// <summary>
        /// メニュー項目「設定」クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemConfig_Click(object sender, EventArgs e)
        {
            showConfig = true;

            var dlg = new FormConfig();
            dlg.config = config;
            dlg.working = server.working;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                config = dlg.config;

                try
                {
                    config.Save();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            showConfig = false;
        }

        /// <summary>
        /// メニュー項目「終了」クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemExit_Click(object sender, EventArgs e)
        {
            notifyIcon.Dispose();
            server.Stop();
            Application.Exit();
        }

        /// <summary>
        /// メニュー項目「開始」クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemStart_Click(object sender, EventArgs e)
        {
            server.Start(config.port);
        }

        /// <summary>
        /// メニュー項目「停止」クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemStop_Click(object sender, EventArgs e)
        {
            server.Stop();
        }

        /// <summary>
        /// メニュー項目「ログフォルダーを開く」クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemLogDir_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(logDir))
            {
                try
                {
                    Process.Start($"\"{logDir}\"");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// タスクトレイアイコン マウス移動イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_MouseMove(object sender, MouseEventArgs e)
        {
            // アイコンのツールチップ文字列設定
            var working = server.working;
            notifyIcon.Text = $"{(working ? STATUS_WORK : STATUS_STOP)}({config.port})";
        }
    }
}
