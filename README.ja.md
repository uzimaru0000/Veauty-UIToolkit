# Veauty-UIToolkit

Veauty で Unity の UI Toolkit (UIElements) を宣言的に構築するためのパッケージ。

## 要件

- Unity 6000.5 以上 (Unity 6)
- `com.uzimaru.veauty`

## インストール

`Packages/manifest.json` に追加:

```json
{
  "dependencies": {
    "com.uzimaru.veauty": "https://github.com/uzimaru0000/Veauty.git",
    "com.uzimaru.veauty-uitoolkit": "https://github.com/uzimaru0000/Veauty-UIToolkit.git"
  }
}
```

## 基本的な使い方

`VeautyElement<State>` で UI ツリーを `VisualElement` にマウントします。

```csharp
using UnityEngine;
using UnityEngine.UIElements;
using Veauty;
using Veauty.UIToolkit;

public class CounterPanel : MonoBehaviour
{
    record CounterState(int Count);

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var app = new VeautyElement<CounterState>(
            root,
            (state, setState) => V.Column()[
                CountDisplay(state.Count),
                V.Button("Increment", () => setState(s => s with { Count = s.Count + 1 }))
            ],
            new CounterState(0)
        );
    }

    [Component]
    static IVTree CountDisplay(int value)
    {
        Hooks.UseEffect(() =>
        {
            Debug.Log($"Count: {value}");
            return null;
        }, new object[] { value });

        return V.Label($"Count: {value}");
    }
}
```

## V ファクトリ API

`V` クラスのメソッドでコントロールを生成します。`[]` インデクサで子要素を指定できます。

### コントロール

```csharp
V.Label("Hello", className: "title")

V.Button("Click", onClick: () => { /* ... */ })

V.TextField(value: "input", placeholder: "Type here",
    onValueChanged: v => { /* ... */ })

V.Toggle(value: true, label: "Enable",
    onValueChanged: v => { /* ... */ })

V.Slider(value: 0.5f, lowValue: 0f, highValue: 1f,
    onValueChanged: v => { /* ... */ })

V.ProgressBar(value: 0.7f, title: "Loading...")

V.DropdownField(choices: new List<string> { "A", "B", "C" },
    index: 0, onValueChanged: v => { /* ... */ })

V.Foldout(text: "Details", value: true)[
    V.Label("Content")
]

V.ScrollView()[
    // スクロール可能なコンテンツ
]
```

### レイアウト

```csharp
// Flexbox Row
V.Row()[
    V.Label("Left"),
    V.Label("Right")
]

// Flexbox Column
V.Column()[
    V.Label("Top"),
    V.Label("Bottom")
]

// 汎用コンテナ
V.Box(className: "card")[
    V.Label("Content")
]
```

## スタイル

### 属性で指定

```csharp
V.Box(extras: new IAttribute<VisualElement>[] {
    new FlexDirection(UnityEngine.UIElements.FlexDirection.Row),
    new BackgroundColor(Color.gray),
    new PaddingTop(new StyleLength(16)),
    new PaddingBottom(new StyleLength(16)),
    new BorderRadius(new StyleLength(8)),
    new FontSize(new StyleLength(18))
})
```

### Style Builder (fluent API)

```csharp
V.Box(extras: new IAttribute<VisualElement>[] {
    new Style.Set
        .FlexDirection(FlexDirection.Row)
        .Padding(new StyleLength(16))
        .BackgroundColor(Color.gray)
        .BorderRadius(new StyleLength(8))
        .Build()
})
```

### 主なスタイル属性

| カテゴリ | 属性 |
|---------|------|
| レイアウト | `FlexDirection`, `FlexGrow`, `FlexShrink`, `FlexWrap`, `JustifyContent`, `AlignItems`, `AlignSelf` |
| サイズ | `Width`, `Height`, `MinWidth`, `MinHeight`, `MaxWidth`, `MaxHeight` |
| 余白 | `MarginTop/Bottom/Left/Right`, `PaddingTop/Bottom/Left/Right` |
| 外観 | `BackgroundColor`, `TextColor`, `BorderColor`, `BorderWidth`, `BorderRadius`, `Opacity` |
| 配置 | `Display`, `Overflow`, `Position`, `Top`, `Bottom`, `Left`, `Right` |
| テキスト | `FontSize` |

## 共通属性

すべてのコントロールで使える属性:

| 属性 | 効果 |
|------|------|
| `ClassName(string)` | USS クラスを設定 |
| `ClassList(string[])` | 複数の USS クラスを設定 |
| `StyleSheetAttr(StyleSheet)` | スタイルシートを適用 |
| `Visible(bool)` | 表示/非表示 |
| `Enabled(bool)` | 有効/無効 |
| `PickingMode(PickingMode)` | ピッキングモード |
| `Tooltip(string)` | ツールチップ |

## uGUI パッケージとの違い

| | Veauty-uGUI | Veauty-UIToolkit |
|---|---|---|
| 対象 | Canvas + uGUI コンポーネント | VisualElement ツリー |
| マウント先 | `GameObject` | `VisualElement` |
| エントリーポイント | `VeautyObject` | `VeautyElement<State>` |
| スタイリング | 属性ベース | USS ライクな属性 + Style Builder |
| レイアウト | Layout Group コンポーネント | Flexbox |
