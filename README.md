# Hibernation #

PCの休止時間とスタンバイ時間の取得、設定を行う。

## バージョン ##

1.1.0

## ライセンス ##

本ソフトウェアはMITライセンスにより提供されています。  
詳細は「LICENSE.txt」をご覧ください。

## パッケージ ##

本ソフトウェアは以下のNuGetパッケージを使用しています。  

| パッケージ                   | ライセンス                                                                                     |
|:-----------------------------|:-----------------------------------------------------------------------------------------------|
| MahApps.Metro                | [MIT](https://github.com/MahApps/MahApps.Metro/blob/develop/LICENSE)                           |
| ControlzEx                   | [MIT](https://github.com/ControlzEx/ControlzEx/blob/develop/LICENSE)                           |
| Microsoft.Xaml.Behaviors.Wpf | [MIT](https://github.com/microsoft/XamlBehaviorsWpf/blob/master/LICENSE)                       |
| System.Text.Json             | [MIT](https://www.nuget.org/packages/System.Text.Json/4.7.2/license)                           |
| MaterialDesignThemes         | [MIT](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit/blob/master/LICENSE) |
| MaterialDesignColors         | [MIT](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit/blob/master/LICENSE) |
| MaterialDesignThemes.MahApps | [MIT](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit/blob/master/LICENSE) |


## 使用方法 ##

起動時には、現在、設定されているスタンバイ時間と休止時間が表示されます。

### Pause/Resumeボタン ###

スタンバイ時間や休止時間が設定されている場合に、Pauseボタンで一時的に設定を無効とします。

Resumeボタンで元のスタンバイ時間や休止時間に戻ります。

### スタンバイ時間 ###

スライダーとキー入力でスタンバイ時間を設定します。

単位は分です。  
0～480分の範囲で設定できます。

0はスタンバイ時間なしとなります。  
また、Clearボタンで0にリセットできます。

### 休止時間 ###

スライダーとキー入力で休止時間を設定します。

単位は分です。  
0～480分の範囲で設定できます。

0は休止時間なしとなります。  
また、Clearボタンで0にリセットできます。

### Saveボタン ###

現在、表示されているスタンバイ時間と休止時間を退避します。

### Restoreボタン ###

前回、Saveボタンで退避したスタンバイ時間と休止時間をセットします。

### Clear Allボタン ###

スタンバイ時間と休止時間を0にします。

### Setボタン ###

表示されているスタンバイ時間と休止時間を実際にセットします。

### Closeボタン ###

アプリケーションを終了してWindowを閉じます。

Pauseボタンで一時的にスタンバイ時間や休止時間の設定を無効化している場合には、ダイアログで無効化の中止を確認します。

キャンセルの場合には、アプリケーションを終了せずに戻ります。

### Aboutボタン ###

アプリケーションの情報を表示します。
## 変更履歴 ##

### 1.1.1 ###

スタンバイ時間や休止時間の一時停止ボタンの追加。

### 1.0.0 ###

GUIを整理し、AboutBoxを追加。

### 0.1.0 ###

基本機能を搭載。
