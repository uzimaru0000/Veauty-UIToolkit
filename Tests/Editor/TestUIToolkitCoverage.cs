using NUnit.Framework;
using UnityEngine.UIElements;
using Veauty.UIToolkit;
using Veauty.UIToolkit.Presets;

namespace Veauty.UIToolkit.Tests
{
    public class TestUIToolkitCoverage
    {
        [Test]
        public void Label_CreatesVTree()
        {
            var tree = V.Label("Hello");
            Assert.IsNotNull(tree);
            Assert.AreEqual(VTreeType.Node, tree.GetNodeType());
        }

        [Test]
        public void Button_CreatesElement()
        {
            var element = V.Button(text: "Click");
            Assert.IsNotNull(element);
            var tree = element.Unwrap();
            Assert.IsNotNull(tree);
        }

        [Test]
        public void TextField_CreatesElement()
        {
            var element = V.TextField(value: "text", label: "Name");
            Assert.IsNotNull(element);
        }

        [Test]
        public void Toggle_CreatesElement()
        {
            var element = V.Toggle(value: true, label: "Check");
            Assert.IsNotNull(element);
        }

        [Test]
        public void Slider_CreatesElement()
        {
            var element = V.Slider(value: 0.5f, lowValue: 0f, highValue: 1f);
            Assert.IsNotNull(element);
        }

        [Test]
        public void ScrollView_CreatesElement()
        {
            var element = V.ScrollView();
            Assert.IsNotNull(element);
        }

        [Test]
        public void Foldout_CreatesElement()
        {
            var element = V.Foldout(text: "Section");
            Assert.IsNotNull(element);
        }

        [Test]
        public void DropdownField_CreatesElement()
        {
            var element = V.DropdownField(
                choices: new System.Collections.Generic.List<string> { "A", "B", "C" },
                index: 0);
            Assert.IsNotNull(element);
        }

        [Test]
        public void ProgressBar_CreatesVTree()
        {
            var tree = V.ProgressBar(value: 50f, title: "Loading");
            Assert.IsNotNull(tree);
        }

        [Test]
        public void Row_CreatesElement()
        {
            var element = V.Row();
            Assert.IsNotNull(element);
        }

        [Test]
        public void Column_CreatesElement()
        {
            var element = V.Column();
            Assert.IsNotNull(element);
        }

        [Test]
        public void Element_Indexer_CreatesTree()
        {
            var tree = V.Button(text: "A")[V.Label("child")];
            Assert.IsNotNull(tree);
        }

        [Test]
        public void Element_Indexer_AcceptsEnumerableChildren()
        {
            var children = new[] { V.Label("A"), V.Label("B") };
            var tree = V.Column()[children];
            var ve = Renderer.Render(tree);
            Assert.AreEqual(2, ve.childCount);
        }

        [Test]
        public void Children_FiltersNullEntries()
        {
            var children = V.Children(V.Label("A"), null, V.Label("B"));
            Assert.AreEqual(2, children.Length);
        }

        [Test]
        public void Classes_FiltersEmptyClassNames()
        {
            var tree = V.Label("A", extras: new IAttribute<VisualElement>[]
            {
                V.Classes("alpha", "", null, "beta")
            });
            var ve = Renderer.Render(tree);
            Assert.IsTrue(ve.ClassListContains("alpha"));
            Assert.IsTrue(ve.ClassListContains("beta"));
            Assert.IsFalse(ve.ClassListContains(""));
        }

        [Test]
        public void Render_Label()
        {
            var tree = V.Label("Hello");
            var ve = Renderer.Render(tree);
            Assert.IsNotNull(ve);
            Assert.IsInstanceOf<UnityEngine.UIElements.Label>(ve);
            Assert.AreEqual("Hello", ((UnityEngine.UIElements.Label)ve).text);
        }

        [Test]
        public void Render_Button()
        {
            var tree = V.Button(text: "Click").Unwrap();
            var ve = Renderer.Render(tree);
            Assert.IsNotNull(ve);
            Assert.IsInstanceOf<UnityEngine.UIElements.Button>(ve);
        }

        [Test]
        public void Render_Nested()
        {
            var tree = V.Column()[
                V.Label("A"),
                V.Label("B")
            ];
            var ve = Renderer.Render(tree);
            Assert.IsNotNull(ve);
            Assert.AreEqual(2, ve.childCount);
        }

        [Test]
        public void Diff_NoChange_NoPatch()
        {
            var tree1 = Resolve(V.Label("Hello"));
            var tree2 = Resolve(V.Label("Hello"));
            var patches = Diff<VisualElement>.Calc(tree1, tree2);
            Assert.AreEqual(0, patches.Length);
        }

        [Test]
        public void Diff_TextChange_ProducesPatch()
        {
            var tree1 = Resolve(V.Label("Hello"));
            var tree2 = Resolve(V.Label("World"));
            var patches = Diff<VisualElement>.Calc(tree1, tree2);
            Assert.Greater(patches.Length, 0);
        }

        [Test]
        public void Patch_AppliesTextChange()
        {
            var tree1 = Resolve(V.Label("Hello"));
            var tree2 = Resolve(V.Label("World"));
            var ve = Renderer.Render(tree1);
            var patches = Diff<VisualElement>.Calc(tree1, tree2);
            ve = Patch.Apply(ve, tree1, patches);
            Assert.IsInstanceOf<UnityEngine.UIElements.Label>(ve);
            Assert.AreEqual("World", ((UnityEngine.UIElements.Label)ve).text);
        }

        [Test]
        public void StyleAttribute_Applied()
        {
            var tree = new UIToolkit.Label(new IAttribute<VisualElement>[]
            {
                new UIToolkit.Label.Text("Styled"),
                new Veauty.UIToolkit.BackgroundColor(UnityEngine.Color.red)
            });
            var ve = Renderer.Render(tree);
            Assert.AreEqual(UnityEngine.Color.red, ve.resolvedStyle.backgroundColor);
        }

        private static IVTree Resolve(IVTree tree)
        {
            return new HookRuntime().Resolve<VisualElement>(tree);
        }
    }
}
