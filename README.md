# JProxy について

JProxyは、株式会社ジェーン(http://janesoft.net/)のWindows用Twitterクライアントソフトウェア「Janetter for Windows」(http://janetter.net/jp/desktop.html)でユーザーのプロフィール画像を表示できない問題を解決するために作成したソフトウェアです。

---
## インストール

- **全て自己責任でお願いします。**

- JProxyは、.NET Framework 4.8用のソフトウェアです。必要に応じて実行環境を設定してください。

- 以下の作業には、UTF-8に対応したテキストエディターが必要です。

- Janetter for Windowsを終了させてから作業を行ってください。


1. ダウンロードしたzipファイルを任意のフォルダーに解凍してください。
    1. JProxy.exe と同じフォルダー内に設定ファイルやエラーログファイルが作成されるので、書き込みに制限のあるフォルダー「C:&yen;Program Files」、「C:&yen;Program Files (x86)」、「C:&yen;Windows」などの中は避けてください。
    2. JProxyは、レジストリの読み書きを行いません。
    3. JProxyは、配置されたフォルダー内にのみフォルダーやファイルを作成します。

2. JProxy.exe を起動し、タスクトレイにある![icon image](JProxy/res/j16.png)アイコンを右クリックして表示されるメニューの「開始」「停止」「設定」項目により、動作可能なポート番号を決定してください。

3. Janetter for Windowsのインストール先にある「Common」フォルダーをバックアップしてください。
    <br>**※インストール時の状態に戻すために必ず必要です。**
    1. Windowsが64bit版の場合　C:&yen;Program Files (x86)&yen;Janetter2&yen;Theme&yen;Common
    2. Windowsが32bit版の場合　C:&yen;Program Files&yen;Janetter2&yen;Theme&yen;Common

4. Janetter for Windowsのインストール先にある「Common」フォルダーを編集のために作業用のフォルダーへコピーし、imgタグのsrcに ***profile_image_url*** が設定されている箇所を書き換えてください。
    <br>**※インストールフォルダーの中で直接編集しないでください。**
    1. Common&yen;template の中の *.tplを検索し、以下のように書き換えてください。

    `<img src="{$〇〇.profile_image_url}">`<br>
    　　↓<br>
    `<img src="http://localhost[:ポート番号]/?url={$〇〇.profile_image_url}">`<br>

    〇〇の部分は、複数のパターンがあります。
    ポート番号は、上記 **2** で決定した番号です。80の場合だけ省略可能ですが、1023以下の番号はお勧めしません。<br>
    例）`<img src="http://localhost:60080/?url={$user.profile_image_url}">`<br>


    2. Common のサブフォルダー内の *.js を検索し、同様に書き換えてください。

    `img.src = '' + 〇〇.profile_image_url + '';`<br>
    　　↓<br>
    `img.src = 'http://localhost[:ポート番号]/?url=' + 〇〇.profile_image_url + '';`<br>

    `$('＊＊＊＊').attr('src', '' + 〇〇.user_profile_image_url);`<br>
    　　↓<br>
    `$('＊＊＊＊').attr('src', 'http://localhost[:ポート番号]/?url=' + 〇〇.user_profile_image_url);`<br>

    `.append( $('<img>').attr('src', 〇〇.profile_image_url)`<br>
    　　↓<br>
    `.append( $('<img>').attr('src', 'http://localhost[:ポート番号]/?url=' + 〇〇.profile_image_url)`<br>

    いろいろなパターンがありますが、いずれもHTMLのimgタグのsrc属性に画像のURLを設定しています。
    「profile_image_url」で検索すれば「user_profile_image_url」なども対象になるので探しやすいと思います。


5. 上記 **4** で編集したファイルをJanetter for Windowsのインストール先フォルダーに上書きコピーしてください。

6. JProxyでサーバーを開始し、Janetter for Windowsを起動してください。

