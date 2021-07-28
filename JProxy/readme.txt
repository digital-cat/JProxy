================================================================================
JProxy について

JProxyは、株式会社ジェーン(http://janesoft.net/)のWindows用Twitterクライアント
ソフトウェア「Janetter for Windows」(http://janetter.net/jp/desktop.html)で
ユーザーのプロフィール画像を表示できない問題を解決するためのソフトウェアです。

================================================================================
インストール

全て自己責任でお願いします。

JProxyは、.NET Framework 4.8用のソフトウェアです。
必要に応じて実行環境を設定してください。

以下の作業には、UTF-8に対応したテキストエディターが必要です。

Janetter for Windowsを終了させてから作業を行ってください。

1). JProxy.zip を任意のフォルダーに解凍してください。
　　※JProxy.exe と同じフォルダー内に設定ファイルやエラーログファイルが作成される
　　　ので、書き込みに制限のあるフォルダー「C:\Program Files」、
　　　「C:\Program Files (x86)」、「C:\Windows」などの中は避けてください。
　　※JProxyは、レジストリの読み書きを行いません。
　　※JProxyは、配置されたフォルダー内にのみフォルダーやファイルを作成します。

2).JProxy.exe を起動し、タスクトレイにある(J)アイコンを右クリックして表示される
　　メニューの「開始」「停止」「設定」項目により、動作可能なポート番号を決定して
　　ください。

3).Janetter for Windowsのインストール先にある「Common」フォルダーをバックアップ
　　してください。
　　※インストール時の状態に戻すために必ず必要です。
　　Windowsが64bit版の場合　C:\Program Files (x86)\Janetter2\Theme\Common
　　Windowsが32bit版の場合　C:\Program Files\Janetter2\Theme\Common

4).Janetter for Windowsのインストール先にある「Common」フォルダーを編集のために
　　作業用のフォルダーへコピーし、imgタグのsrcに profile_image_url が設定されて
    いる箇所を書き換えてください。
    ※インストールフォルダーの中で直接編集しないでください。
　　・Common\template の中の *.tplを検索し、以下のように書き換えてください。

　　　　<img src="{$○○.profile_image_url}">
　　　　　　↓
　　　　<img src="http://localhost[:ポート番号]/?url={$○○.profile_image_url}">

　　　　○○の部分は、複数のパターンがあります。
　　　　ポート番号は、上記 2) で決定した番号です。80の場合だけ省略可能ですが、
　　　　1023以下の番号はお勧めしません。

　　　　例）
　　　　<img src="http://localhost:60080/?url={$user.profile_image_url}">

　　・Common のサブフォルダー内の *.js を検索し、同様に書き換えてください。

　　　　img.src = '' + ○○.profile_image_url + '';
　　　　　　↓
　　　　img.src = 'http://localhost[:ポート番号]/?url=' + ○○.profile_image_url + '';

　　　　$('＊＊＊＊').attr('src', '' + ○○.user_profile_image_url);
　　　　　　↓
　　　　$('＊＊＊＊').attr('src', 'http://localhost[:ポート番号]/?url=' + ○○.user_profile_image_url);

　　　　.append( $('<img>').attr('src', account.user.profile_image_url)
　　　　　　↓
　　　　.append( $('<img>').attr('src', 'http://localhost[:ポート番号]/?url=' + ○○.profile_image_url)

　　いろいろなパターンがありますが、いずれもHTMLのimgタグのsrc属性に画像のURLを
　　設定しています。
　　「profile_image_url」で検索すれば「user_profile_image_url」なども対象になる
　　ので探しやすいと思います。

5).上記 4) で編集したファイルをJanetter for Windowsのインストール先フォルダーに
　　上書きコピーしてください。

6).JProxyでサーバーを開始し、Janetter for Windowsを起動してください。

