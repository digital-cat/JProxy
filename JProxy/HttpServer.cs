using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;

namespace JProxy
{
    /// <summary>
    /// HTTPサーバー
    /// </summary>
    internal class HttpServer
    {
        /// <summary>HTTPサーバーリスニングスレッド</summary>
        private Task task = null;

        /// <summary>停止要求トークン</summary>
        private CancellationTokenSource tokensrc = null;

        /// <summary>HTTPサーバー稼働状況</summary>
        internal bool working { get { return (task != null && task.Status == TaskStatus.Running); } }

        /// <summary>HTTPクライアント（全スレッド間で共有）</summary>
        private static readonly HttpClient client = new HttpClient();

        /// <summary>ログ出力管理</summary>
        private LogManager log = null;

        internal HttpServer()
        {
        }

        /// <summary>
        /// ログ出力管理設定
        /// </summary>
        /// <param name="logManager">ログ出力管理</param>
        internal void SetLog(ref LogManager logManager)
        {
            log = logManager;
        }

        /// <summary>
        /// 停止要求トークン解放
        /// </summary>
        private void ClearToken()
        {
            if (tokensrc != null)
            {
                try
                {
                    tokensrc.Dispose();
                    tokensrc = null;
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// HTTPサーバー開始
        /// </summary>
        /// <param name="port">ポート番号</param>
        /// <returns>正常／異常</returns>
        internal bool Start(ushort port)
        {
            log.Debug($"HttpServer.Start({port})");

            if (!Stop())
            {
                return false;
            }

            tokensrc = new CancellationTokenSource();

            try
            {
                task = Task.Run(() => { ExecListener(port); });

                log.Debug("HttpServer.Start OK");

                return true;
            }
            catch (Exception e)
            {
                log.Error($"HttpServer.Start Exception:{e.Message}");
                return false;
            }
        }

        /// <summary>
        /// HTTPサーバー停止
        /// </summary>
        /// <returns>正常／異常</returns>
        internal bool Stop()
        {
            log.Debug("HttpServer.Stop()");

            if (task == null || tokensrc == null)
            {
                log.Debug("HttpServer.Stop Non-working");
                return true;
            }

            try
            {
                tokensrc.Cancel();

                task.Wait();
                task = null;

                ClearToken();

                log.Debug("HttpServer.Stop OK");

                return true;
            }
            catch (Exception e)
            {
                log.Error($"HttpServer.Stop Exception:{e.Message}");
                return false;
            }
        }

        /// <summary>
        /// HTTPサーバー実行
        /// </summary>
        /// <param name="port">ポート番号</param>
        private void ExecListener(ushort port)
        {
            log.Debug($"HttpServer.ExecListener({port})");

            using (var listener = new HttpListener())
            {
                try
                {
                    //listener.Prefixes.Add($"http://*:{port}/");   「*」でポートを開くのは管理者権限が必要
                    listener.Prefixes.Add($"http://localhost:{port}/");
                    listener.Prefixes.Add($"http://127.0.0.1:{port}/");
                }
                catch (Exception e)
                {
                    log.Error($"HttpServer.ExecListener [listener.Prefixes.Add] Exception:{e.Message}");
                    MessageBox.Show(e.Message, "JProxyサーバー開始", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    listener.Start();
                }
                catch (Exception e)
                {
                    log.Error($"HttpServer.ExecListener [listener.Start] Exception:{e.Message}");
                    MessageBox.Show(e.Message, "JProxyサーバー開始", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    var token = tokensrc.Token;

                    using (var r = token.Register(() => listener.Close()))
                    {
                        while (!token.IsCancellationRequested && listener.IsListening)
                        {
                            try
                            {
                                var context = listener.GetContext();    // クライアントからの要求待機

                                _ = Task.Run(() => GetAsync(context.Request, context.Response) );

                            }
                            catch (Exception)
                            {
                                if (!token.IsCancellationRequested)
                                    throw;  // 停止要求ではない
                                log.Debug("HttpServer.ExecListener CancellationRequested");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    log.Error($"HttpServer.ExecListener [listening loop] Exception:{e.Message}");
                    MessageBox.Show(e.Message, "JProxyサーバー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            log.Debug("HttpServer.ExecListener Exit");
        }

        /// <summary>
        /// クライアントからの要求処理
        /// （HTTPクライアントによるGET実行）
        /// </summary>
        /// <param name="request">クライアントからのリクエスト情報</param>
        /// <param name="response">クライアントへのレスポンス情報</param>
        /// <returns>タスク</returns>
        private async Task GetAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            var id = Thread.CurrentThread.ManagedThreadId;

            log.Debug($"HttpServer.GetAsync Thread[{id}]");

            try
            {
                // http://localhost:port/?url=取得要求のURL
                var url = request.QueryString["url"];

                log.Debug($"HttpServer.GetAsync Thread[{id}] URL[{url}]");

                if (string.IsNullOrEmpty(url))
                {
                    log.Error($"HttpServer.GetAsync Thread[{id}] BadRequest URL[{url}]");
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.StatusDescription = $"{HttpStatusCode.BadRequest} JProxy - The \"url\" parameter is required.";
                }
                else
                {
                    if (string.Compare(url, 0, "http://", 0, 7) == 0)
                    {
                        url = url.Insert(4, "s");     // http:// -> https://
                        log.Debug($"HttpServer.GetAsync Thread[{id}] changed http into https. URL[{url}]");
                    }

                    try
                    {
                        // User-Agent引き継ぎ
                        //if (first)
                        //{
                        //    var ua = request.UserAgent;
                        //    if (!string.IsNullOrEmpty(ua))
                        //    {
                        //        client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", ua);
                        //        first = false;
                        //    }
                        //}

                        var res = await client.GetAsync(url, tokensrc.Token);

                        log.Debug($"HttpServer.GetAsync Thread[{id}] HTTPClient Status[{(int)res.StatusCode}][{res.ReasonPhrase}]");

                        response.StatusCode = (int)res.StatusCode;
                        response.StatusDescription = res.ReasonPhrase;
                        
                        if (res.StatusCode == HttpStatusCode.OK)
                        {
                            try
                            {
                                using (var stream = await res.Content.ReadAsStreamAsync())
                                {
                                    stream.CopyTo(response.OutputStream);
                                }
                            }
                            catch (Exception e)
                            {
                                log.Error($"HttpServer.GetAsync Thread[{id}] [stream.CopyTo] Exception:{e.Message}");
                                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                response.StatusDescription = $"{HttpStatusCode.InternalServerError} JProxy - HttpClient Exception: {e.Message}";
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (!tokensrc.IsCancellationRequested)
                        {
                            log.Error($"HttpServer.GetAsync Thread[{id}] [client.GetAsync] Exception:{e.Message}");
                            response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            response.StatusDescription = $"{HttpStatusCode.InternalServerError} JProxy - HttpClient Exception: {e.Message}";
                        }
                        else
                        {
                            log.Debug($"HttpServer.GetAsync Thread[{id}] CancellationRequested");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log.Error($"HttpServer.GetAsync Thread[{id}] Exception:{e.Message}");
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.StatusDescription = $"{HttpStatusCode.InternalServerError} JProxy - Request URL processing Exception: {e.Message}";
            }

            try
            {
                response.Close();   // クライアントへの応答送信
            }
            catch (Exception e)
            {
                log.Error($"HttpServer.GetAsync Thread[{id}] [response.Close] Exception:{e.Message}");
            }
        }

    }
}
