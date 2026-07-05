# はじめに

[English](../getting-started.md)

パッケージをインストールし、最初の宣言的 UI Toolkit ビューを構築します。

## インストール

コアパッケージと本パッケージの両方を `Packages/manifest.json` に追加します:

```json
{
  "dependencies": {
    "com.uzimaru.veauty": "https://github.com/uzimaru0000/Veauty.git",
    "com.uzimaru.veauty-uitoolkit": "https://github.com/uzimaru0000/Veauty-UIToolkit.git"
  }
}
```

シーンには Panel Settings アセットを割り当てた `UIDocument` (GameObject > UI Toolkit > UI Document) が必要です。

## 最小の例

```csharp
using UnityEngine;
using UnityEngine.UIElements;
using Veauty;
using Veauty.UIToolkit;
using Veauty.UIToolkit.Presets;

public class HelloVeauty : MonoBehaviour
{
    record struct AppState(int Count);

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var app = new VeautyElement<AppState>(
            root,
            (state, setState) => V.Column()[
                V.Label($"Count: {state.Count}", className: "counter-label"),
                V.Row()[
                    V.Button("-", () => setState(s => s with { Count = s.Count - 1 })),
                    V.Button("+", () => setState(s => s with { Count = s.Count + 1 }))
                ]
            ],
            new AppState(0)
        );
    }
}
```

## 1行ずつの解説

- `using Veauty.UIToolkit;` — 基本レイヤー: `VeautyElement<State>`、コントロールノード、属性。
- `using Veauty.UIToolkit.Presets;` — 便利レイヤー: `V` ファクトリと `Element` ビルダー。
- `record struct AppState(int Count);` — state の型。`VeautyElement<State>` は `State` を値型に制約している (`where State : struct`) ため、class ではなく `struct` または `record struct` を使います。
- `GetComponent<UIDocument>().rootVisualElement` — マウント先はどの `VisualElement` でも構いません。通常は `UIDocument` のルートを使います。
- `new VeautyElement<AppState>(root, renderFunc, initialState)` — 即座にマウントします: レンダー関数が同期的に実行され、生成されたツリーが `VisualElement` としてレンダリングされて `root` 配下に追加され、マウント領域いっぱいに広がるよう `flexGrow = 1` が設定されます。
- `(state, setState) => ...` — レンダー関数は現在の state とセッターを受け取ります。このオーバーロードの `setState` は更新関数 `State => State` を取ります。別のオーバーロードは新しい state 値を直接取り、3つ目はセッターなしです ([要素](elements.md#veautyelementstate) を参照)。
- `V.Column()[ ... ]` — `V` ファクトリメソッドは `Element` を返し、インデクサ `[...]` に子を渡すと実際のツリーノードが生成されます。`V.Column` は `flex-direction: column` のプレーンなコンテナです。
- `V.Label(...)`, `V.Button(...)` — 名前付きオプション引数を持つコントロール。渡した引数だけが属性になります。
- `setState(s => s with { Count = s.Count + 1 })` — 次の state を計算し再レンダリングをスケジュールします。Veauty は新しいツリーを前回のツリーと diff し、変更された部分だけをパッチします (この例ではラベルのテキストのみ)。

## 再レンダリング

`setState` を呼ぶたびに、レンダー関数 → diff → patch が実行されます。レンダリング中に呼ばれた場合は遅延され、1回の追加レンダリングにまとめられます — [アーキテクチャ](architecture.md#再レンダリングのスケジューリング) を参照。

## 次のステップ

- [要素](elements.md) — コントロールと `V` ファクトリの全カタログ。
- [スタイリング](styling.md) — インラインスタイル、USS クラス、スタイルシート。
- [イベント](events.md) — 変更イベントとクリックハンドラ。
