# イベント

[English](../events.md)

再レンダリングでコールバックが重複したり、state が自分自身に跳ね返ったりしないように、Veauty-UIToolkit がイベントハンドラをどう配線しているか。

## 問題

仮想 DOM システムでは、属性は変更のあったレンダリングごとに再適用されます。素朴に毎回 `RegisterCallback` を呼ぶと、レンダリングごとにハンドラが積み重なります。素朴に `value` を書き戻すと変更イベントが発火し、state を変更し、再レンダリング — つまり無限のフィードバックループです。本パッケージは両方に対処しています。

## 変更イベント: コールバックの重複排除

`OnValueChanged` 属性 (`TextField`, `Toggle`, `Slider`, `DropdownField`) は、**(要素, キー)** で管理される内部のコールバックストアを通して登録されます — キーは属性の diff キー、例えば `"onValueChanged"` です:

1. その要素/キーのペアに既にコールバックが保存されていれば、まず**登録解除**されます。
2. 新しいコールバックが登録され、そのペアで保存されます。

したがって何回再レンダリングしても、要素はキーごとに Veauty が登録したハンドラを最大1つしか持ちません。ストア外で自分が要素に登録したハンドラ (別のデリゲート) には影響しません。

```csharp
V.TextField(
    value: state.Name,
    onValueChanged: evt => setState(s => s with { Name = evt.newValue })
)
// 新しいラムダで再レンダリングすると古いハンドラは置き換えられる — 積み重ならない
```

補足: 新しいラムダは別のデリゲートでハッシュも異なるため、`OnValueChanged` 属性は通常レンダリングのたびに「変更あり」として diff されます。ストアの置き換えセマンティクスがそれを安全かつ低コストにしています。

## Button.OnClick: clickable の置き換え

`Button.OnClick` はプレーンな `System.Action` を受け取り、ボタンの `clickable` を**置き換える**ことで適用します:

```csharp
button.clickable = new Clickable(action);
```

コールバックストアと同じ効果 — 以前のハンドラは破棄され、積み重なりません — ですが、より強い注意点があります: 古い `Clickable` に紐付いていた他のもの (追加の activator や manipulator の状態) も一緒に破棄されます。カスタムの `Clickable` の挙動が必要な場合は Veauty の外でセットアップし、`OnClick` は使わないでください。

## 値の書き込み: SetValueWithoutNotify

インタラクティブなコントロールの値属性は `SetValueWithoutNotify` で適用されます:

| 属性 | 効果 |
|---|---|
| `TextField.Value` | `ChangeEvent<string>` を発火せずにテキストを設定 |
| `Toggle.Value` | `ChangeEvent<bool>` を発火せずにチェック状態を設定 |
| `Slider.Value` | `ChangeEvent<float>` を発火せずに値を設定 |
| `DropdownField.Value` | `ChangeEvent<string>` を発火せずに選択を設定 |
| `Foldout.Value` | `ChangeEvent<bool>` を発火せずに開閉状態を設定 |

これにより、制御されたインプットのフィードバックループが断ち切られます: ユーザーが入力 → `ChangeEvent` → `setState` → 再レンダリングが `state.Name` をフィールドに書き戻す → 書き込みはサイレントなので新しい `ChangeEvent` は**発生しない**。

```
ユーザー入力 ──ChangeEvent──▶ onValueChanged ──setState──▶ 再レンダリング
     ▲                                                          │
     └────────── SetValueWithoutNotify (イベントなし) ◀─────────┘
```

例外: `DropdownField.Index` は `element.index` を直接代入するため、通知**されます**。state 駆動の選択には `DropdownField.Value` を、1回きりの初期化には `Index` を使ってください。

## ライフタイム

コールバックストアのエントリは要素の参照でキー付けされます。要素が削除されて再生成された場合 (例: `Redraw` パッチ)、新しい要素はストアに何も持たない状態から始まり、属性の適用でハンドラが新たに登録されます。

## 関連ページ

- [要素](elements.md) — どのファクトリ引数がこれらの属性に対応するか。
- [アーキテクチャ](architecture.md) — 属性がいつ (再) 適用されるか。
