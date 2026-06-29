using UnityEngine.UIElements;

namespace Veauty.UIToolkit
{
    public class VeautyElement<State> where State : struct
    {
        private IVTree oldTree;
        private readonly VisualElement mounter;
        private readonly System.Func<State, System.Action<State>, IVTree> renderFunc;
        private VisualElement rootElement;

        private State _state;
        private State state
        {
            get => this._state;
            set
            {
                this._state = value;
                ForceUpdate();
            }
        }

        public VeautyElement(VisualElement mounter, System.Func<State, System.Action<State>, IVTree> renderFunc, State state = default)
        {
            this.mounter = mounter;
            this.renderFunc = renderFunc;
            this._state = state;
            this.oldTree = renderFunc(this.state, s => this.state = s);

            Render();
        }

        public VeautyElement(
            VisualElement mounter,
            System.Func<State, IVTree> renderFunc,
            State state = default
        ) : this(mounter, (s, _) => renderFunc(s), state) { }

        public void ForceUpdate()
        {
            var newTree = this.renderFunc(this.state, s => this.state = s);
            var patches = Diff<VisualElement>.Calc(this.oldTree, newTree);

            this.rootElement = Patch.Apply(this.rootElement, this.oldTree, patches);
            this.oldTree = newTree;
        }

        private void Render()
        {
            this.rootElement = Renderer.Render(this.oldTree);
            this.mounter.Add(this.rootElement);
            this.rootElement.style.flexGrow = 1;
        }
    }
}
