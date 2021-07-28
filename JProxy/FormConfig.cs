using System;
using System.Windows.Forms;
using System.Reflection;

namespace JProxy
{
    /// <summary>
    /// 設定フォーム
    /// </summary>
    internal partial class FormConfig : Form
    {
        /// <summary>数字文字</summary>
        private const string NUM_CHAR = "0123456789";

        /// <summary>設定情報</summary>
        internal Config config = new Config();

        /// <summary>サーバー稼働</summary>
        internal bool working = false;

        internal FormConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォーム初期化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormConfig_Load(object sender, EventArgs e)
        {
            textBoxPort.Text = config.port.ToString();
            textBoxPort.Enabled = !working;     // サーバー稼働時はポート番号変更不可
            labelWorking.Visible = working;

            checkBoxAutoStart.Checked = config.autoStart;

            // バージョン情報
            Assembly asm = Assembly.GetExecutingAssembly();
            AssemblyName name = asm.GetName();
            labelVersion.Text = $"{name.Name} version {name.Version.ToString()}";
        }

        /// <summary>
        /// ポート番号テキストボックス キー押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && NUM_CHAR.IndexOf(e.KeyChar) < 0)
            {
                e.Handled = true;   // 制御と数字以外は入力不可
            }
        }

        /// <summary>
        /// ポート番号テキストボックス 変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxPort_TextChanged(object sender, EventArgs e)
        {
            // 貼り付け対策
            var text = textBoxPort.Text;
            if (!string.IsNullOrEmpty(text) && !int.TryParse(text, out int num))
            {
                textBoxPort.Text = "";
            }
        }

        /// <summary>
        /// OKボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOk_Click(object sender, EventArgs e)
        {
            var pnm = textBoxPort.Text;
            if (!Config.TryToPort(pnm, out ushort portnum))
            {
                MessageBox.Show("ポート番号が不正です。", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            config.port = portnum;
            config.autoStart = checkBoxAutoStart.Checked;

            DialogResult = DialogResult.OK;
        }
    }
}
