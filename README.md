# Hibernation #

PCの休止時間とスタンバイ時間の取得、設定を行う。

## バージョン ##

1.0.0

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

Windowを閉じます。

### Aboutボタン ###

アプリケーションの情報を表示します。
## 変更履歴 ##

### 1.0.0 ###

GUIを整理し、AboutBoxを追加。

### 0.1.0 ###

基本機能を搭載。
