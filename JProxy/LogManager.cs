using System;
using System.IO;
using System.Text.RegularExpressions;

namespace JProxy
{
    /// <summary>
    /// ログ出力管理
    /// </summary>
    class LogManager
    {
        /// <summary>ログファイル保管期限（日数）</summary>
        private readonly int STORAGE_PERIOD = 10;

        /// <summary>排他制御オブジェクト</summary>
        private object exclObject = new object();

        /// <summary>ログファイルフォルダーパス</summary>
        public string logDir = string.Empty;


        internal LogManager()
        {

        }

        /// <summary>
        /// 保管期限切れログファイル削除
        /// </summary>
        /// <returns>正常／異常</returns>
        internal bool RemoveOldFiles()
        {
            if (string.IsNullOrEmpty(logDir))
            {
                return false;
            }

            bool status = true;

            try
            {
                var dirinfo = new DirectoryInfo(logDir);
                var files = dirinfo.GetFiles("????????.log");               // ログファイル一覧(yyyyMMdd.log)
                var pattern = new Regex(@"^[0-9]{8}.log$", RegexOptions.IgnoreCase);   // ファイル名パターン
                var limitDate = DateTime.Now.AddDays(-STORAGE_PERIOD);      // 保存期限内最古の日付
                var limit = int.Parse(limitDate.ToString("yyyyMMdd"));

                foreach (var file in files)
                {
                    if (pattern.IsMatch(file.Name) && int.Parse(file.Name.Substring(0, 8)) < limit)
                    {
                        try
                        {
                            File.Delete(file.FullName);
                        }
                        catch (Exception)
                        {
                            status = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return status;
        }

        /// <summary>
        /// エラーログ出力
        /// </summary>
        /// <param name="log">ログ出力文字列</param>
        /// <returns>正常／異常</returns>
        internal bool Error(string log)
        {
            return Output(" [ERROR] " + log);
        }

        /// <summary>
        /// デバッグログ出力（デバッグビルド時のみ出力）
        /// </summary>
        /// <param name="log">ログ出力文字列</param>
        /// <returns>正常／異常</returns>
        internal bool Debug(string log)
        {
#if DEBUG

            return Output(" [DEBUG] " + log);
#else
            return true;
#endif
        }

        /// <summary>
        /// ログ出力処理
        /// </summary>
        /// <param name="log">ログ出力文字列</param>
        /// <returns>正常／異常</returns>
        protected bool Output(string log)
        {
            if (string.IsNullOrEmpty(logDir))
            {
                return false;
            }

            try
            {
                var now = DateTime.Now;
                var path = Path.Combine(logDir, now.ToString("yyyyMMdd") + ".log");

                lock(exclObject)
                {
                    File.AppendAllText(path, now.ToString("yyyy/MM/dd HH:mm:ss") + log + Environment.NewLine);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
