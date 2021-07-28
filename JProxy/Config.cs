using System;
using System.Linq;
using System.Xml.Linq;
using System.IO;

namespace JProxy
{
    /// <summary>
    /// 設定情報
    /// </summary>
    internal class Config
    {
        /// <summary>ポート番号デフォルト値</summary>
        private const ushort DEF_PORT = 60080;

        /// <summary>ポート番号最小値</summary>
        private const int MIN_PORT = (int)UInt16.MinValue;

        /// <summary>ポート番号最大値</summary>
        private const int MAX_PORT = (int)UInt16.MaxValue;

        /// <summary>ポート番号</summary>
        public ushort port = DEF_PORT;

        /// <summary>起動時自動開始</summary>
        public bool autoStart = false;

        /// <summary>設定ファイルパス</summary>
        public string path = string.Empty;

        internal Config()
        {

        }

        /// <summary>
        /// ポート番号の文字列から整数値への変換
        /// </summary>
        /// <param name="src">ポート番号文字列</param>
        /// <param name="dst">ポート番号整数値</param>
        /// <returns>正常／異常</returns>
        internal static bool TryToPort(string src, out ushort dst)
        {
            dst = 0;

            if (!int.TryParse(src, out int tmp) || tmp < MIN_PORT || tmp > MAX_PORT)
            {
                return false;
            }

            dst = (ushort)tmp;

            return true;
        }

        /// <summary>
        /// 設定ファイル読み込み
        /// </summary>
        /// <returns>正常／異常</returns>
        internal bool Load()
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return false;
            }

            var doc = XDocument.Load(path);
            var cfg = doc.Descendants("Config");

            var pno = cfg.Select(p => p.Element("Port")?.Value).Single();
            port = TryToPort(pno, out ushort pnum) ? pnum : DEF_PORT;

            var ast = cfg.Select(p => p.Element("AutoStart")?.Value).Single();
            autoStart = (!string.IsNullOrEmpty(ast) && string.Compare(ast, "true", true) == 0);

            return true;
        }

        /// <summary>
        /// 設定ファイル保存
        /// </summary>
        /// <returns>正常／異常</returns>
        internal bool Save()
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                                        new XElement("Config",
                                            new XElement("Port", port),
                                            new XElement("AutoStart", autoStart)
                                        )
                                    );

            doc.Save(path);

            return true;
        }

    }
}
