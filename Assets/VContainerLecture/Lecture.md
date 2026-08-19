# はじめに
この講習会資料は以下を前提とします。
- Unity 6000.3.21f1
- https://github.com/tuatmcc/VContainerLecture　のクローン

また前提知識としてUnity, C#の基礎的な理解を想定しています。(Class, Method, SerializeField, FindObject)
講習会当日であればサポートがある(見込み)ですし、リポジトリごとChatGPTに投げて逐次聞きながら進めるのも良いでしょう

また、この資料は一からDIを意識した設計が行えるようになるというよりかは、DIが用いられている既存プロジェクトで、DIを意識して機能の追加が行えることを目標にします。

# VContainerとは何か？
依存性注入(Dependency Injection, 以下DIと表記)のためのライブラリです。
詳しくは公式のページを参考にしてください
https://vcontainer.hadashikick.jp/ja/

Extenjectというライブラリもあります(去年はこれでした)がなぜ切り替えたかというと、半分くらいはノリです。
速かったり、シンプルだったりはします。

詳しい違いは
https://vcontainer.hadashikick.jp/ja/comparing/comparing-to-zenject
を見てください

# 依存性注入とはなにか？

ざっくり言えば、依存性注入とは
**(何らかの形で結びつけられた)外部のオブジェクトを受け取る**
テクニックです。

これを上手いことゴニョゴニョすると
- 疎結合なプログラムがつくれる
- モックと本番コードの切り替えが簡単にできる
- MonoBehaviourの継承を減らせる

ようになります。これだけ言っても何のことやらという話だと思うので実際にDIしてみましょう。

# (Pure C#に)DIしてみる
`Assets/VContainerLecture/Core/Scripts/GameFlowManager.cs`に`ISceneLoader`をDIしてみましょう！
`GameFlowManager`はシーンを跨いだ状態の管理を担い、`SceneLoader`はシーンのロードを担います(SceneLoaderのインターフェイスがISceneLoaderです)。`ISceneLoader`をDIすることで`GameFlowManager`へ`ISceneLoader`が注入される訳です。

一般的なDIの手順は次のとおりです。
1. 注入されるクラスを書く(実はインターフェイスは必須ではありません!!)
2. LifetimeScopeに登録する
3. 注入する

3分◯ッキング的手法として1.の注入するコードは用意してあります。

`GameFlowManager`, `IGameFlowManager`がそれです。

ただのピュア(MonoBehaviourを継承していない)なC#のコードです(ピュアなC#で書けるというのが偉大なことなんですよ！)

次に`RootLifetimeScope`に登録していきましょう。
(ZenjectでいうところのInstallerですね)

```csharp
using VContainer;
using VContainer.Unity;

namespace VContainerLecture.Core.Scripts
{
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneLoader>(Lifetime.Singleton).As<ISceneLoader>();
            builder.Register<GameFlowManager>(Lifetime.Singleton).As<IGameFlowManager>();
        }
    }
}
```
こんな感じです。依存性注入というのは注入する何かしらをどこかしらから持ってくるものですが、これは当たり前に指定してあげる必要があります。

素のC#であれば
```csharp
builder.Register<クラス名>(Lifetime.Singleton).as<インターフェイス1>.as<インターフェイス2>
```
このようにするということです。(インターフェイスは注入されるクラスが継承しているインターフェイスです)
Singletonはあまり気にしなくていいです。Extenjectでいうところの`FromNew().AsSingle()`みたいなやつですね。

3.インジェクトされてみましょう。MonoBehaviourを継承しないクラスは大体コンストラクタインジェクションを使えば良いと思います。つまりこういうことです。(コメントアウトされているだけなので解除しておいてください)
```csharp
public GameFlowManager(ISceneLoader sceneLoader)
{
    this.sceneLoader = sceneLoader;
    //以下、その他の初期化処理
}
```
この例だと`ISceneLoader`を継承したクラス`SceneLoader`のインスタンスが引数`sceneLoader`として注入されています。

# (MonoBehaviourに)DIしてみる
次はMonoBehaviourを継承したクラスにDIしてみましょう。

`Assets/VContainerLecture/Play/Scripts/PlayerController.cs`へ、プレイヤーの入力を扱う`IPlayerInput`を注入します。

先と同様にコード自体はすでに出来合いの物があるのでLifetimeScopeへ登録していきましょう。
いつも通りコメントアウトしてあると思うので解除しておいてください。
```csharp
namespace VContainerLecture.Play.Scripts
{
    public class PlayLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PlayerInput>(Lifetime.Singleton)
                .As<IPlayerInput>()
                .As<ITickable>()
                .As<IDisposable>();
            
            //シーン上にあるPlayerControllerがアタッチされたゲームオブジェクトから自動で登録
            builder.RegisterComponentInHierarchy<PlayerController>();
        }
    }
}

```
次に注入していきましょう。

先のPure C#のクラスではコンストラクタを用いましたが、MonoBehaviourを継承したクラスはコンストラクタを持てません。
そこでメソッドインジェクションを使います。

つまり次のような塩梅です。実際のコードは複数注入していますが、まぁ気にしないでください。
```csharp
public void Construct(IPlayerInput playerInput)
{
    _playerInput = playerInput;
}
```
また、MonoBehaviourを継承したクラス(RegisterComponentInHierarchy)で注入されているクラスについては、スクリプトがアタッチされたゲームオブジェクトがシーン上に1つ存在している必要があります。
というわけでHierarchyにいるunitychanにPlayerControllerをアタッチしてあげてください。

この時点で`PlayScene`を再生して無事に操作できれば成功です。

Pure C#, MonoBehaviour以外にもSciptableObject(SO)とかあったりしますが、ここでは割愛します。
実装例が見たい場合は `PlaySetting.cs`あたりを参考にしてみてください。

# テスト用コードの切り替え
先に`注入されるクラスを書く(実はインターフェイスは必須ではありません!!)`みたいなことを書きました。

そもそもなんでインターフェイスを切り替えたかといえば、振る舞いの切り替えが容易になるからです。
実際にテスト用コードに切り替えられるようにして、その嬉しさを実感してみましょう。

`PlayManager`をデバッグ用の`TestPlayManager`へ切り替えられるようにします。
この２つのクラスはどちらも共通のインターフェイス`IPlayManager`の実装です。

`PlayLifetimeScope`にテスト用実装をつかうか否かのフィールドを追加して切り替えられるようにします。

```csharp
[SerializeField]
private bool isTest;

//...

protected override void Configure(IContainerBuilder builder)
{
    if (isTest)
    {
        builder.Register<TestPlayManager>(Lifetime.Singleton)
            .As<IPlayManager>()
            .As<IStartable>();
    }
    else
    {
        builder.Register<PlayManager>(Lifetime.Singleton)
            .As<IPlayManager>()
            .As<IStartable>();
    }
}
```
こんな塩梅にします。

注入される側のコードを変更する必要はありません。

実際にチェックボックスをON/OFFして振る舞いが変わることを確認しましょう！

# APPENDIX

Q＆A
- SerializeFieldじゃ駄目なの？
- FindObjectは駄目なの？
- 依存性逆転ってなに？

# Reference
https://github.com/tuatmcc/ExtenjectLecture/tree/main

https://vcontainer.hadashikick.jp/


