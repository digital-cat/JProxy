using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace JProxy
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 同じパスでの二重起動防止（他のフォルダーに配置した同じexeを起動するのはOK）
            Mutex mutex = null;

            try
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var name = new Uri(codeBase).LocalPath.Replace("\\", "_").ToLower();

                mutex = new Mutex(true, name, out bool create);

                if (!create)
                {
                    mutex.Close();
                    MessageBox.Show("JProxyは既に起動しています。", "JProxy", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("システムエラーにより起動できませんでした。" + Environment.NewLine + e.Message, "JProxy", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new FormMain());
                new FormMain();
                Application.Run();
            }
            catch (Exception e)
            {
                MessageBox.Show("システムエラーが発生しました。" + Environment.NewLine + e.Message, "JProxy", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                mutex.ReleaseMutex();
                mutex.Close();
            }
        }
    }
}
