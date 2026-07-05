# Veauty-UIToolkit

[English](../com.uzimaru.veauty-uitoolkit.md)

Veauty の仮想ツリーコアの上に構築された、Unity UI Toolkit (UIElements) 向けの宣言的・React 風 UI 構築パッケージ。

`com.uzimaru.veauty-uitoolkit` は Veauty の仮想ツリー (`IVTree`) を `VisualElement` 階層としてレンダリングし、差分検出とパッチ適用によって state と同期し続けます。要素を手動で操作する必要も、UXML も不要です。

## 要件

- Unity 6000.5 以上 (Unity 6)
- `com.uzimaru.veauty` (コアパッケージ。依存として自動的に取得されます)

## インストール

`Packages/manifest.json` に追加します:

```json
{
  "dependencies": {
    "com.uzimaru.veauty": "https://github.com/uzimaru0000/Veauty.git",
    "com.uzimaru.veauty-uitoolkit": "https://github.com/uzimaru0000/Veauty-UIToolkit.git"
  }
}
```

## クイックスタート

```csharp
using UnityEngine;
using UnityEngine.UIElements;
using Veauty;
using Veauty.UIToolkit;
using Veauty.UIToolkit.Presets;

public class CounterPanel : MonoBehaviour
{
    record struct CounterState(int Count);

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var app = new VeautyElement<CounterState>(
            root,
            (state, setState) => V.Column()[
                V.Label($"Count: {state.Count}"),
                V.Button("Increment", () => setState(s => s with { Count = s.Count + 1 }))
            ],
            new CounterState(0)
        );
    }
}
```

`VeautyElement<State>` がツリーを `root` 配下にマウントし、`setState` が呼ばれるたびに再レンダリングします。

## 名前空間

| 名前空間 | 役割 | 主な型 |
|-----------|------|-----------|
| `Veauty.UIToolkit` | 基本レイヤー: エントリーポイント、レンダラー、パッチ適用、コントロールノード、属性 | `VeautyElement<State>`, `Renderer`, `Patch`, `UIBase<T>`, `StyleAttribute<T>`, `StyleBuilder` |
| `Veauty.UIToolkit.Presets` | 便利レイヤー: 名前付きオプション引数を持つファクトリ | `V`, `Element` |

## ドキュメント

- [はじめに](getting-started.md) — インストールと最小の動作例を1行ずつ解説。
- [アーキテクチャ](architecture.md) — レンダリングサイクル、diff/patch、ホストライフサイクルの順序、uGUI パッケージとの違い。
- [要素](elements.md) — `VeautyElement<State>`、`UIBase<T>` コントロール一覧、`V` ファクトリ、`Element` ビルダー。
- [スタイリング](styling.md) — インラインスタイル属性、`StyleBuilder` fluent API、USS クラス、スタイルシート。
- [イベント](events.md) — イベント処理、コールバックの重複排除、フィードバックループ防止。
- [API リファレンス](api-reference.md) — 全公開型のシグネチャとメンバー表。
