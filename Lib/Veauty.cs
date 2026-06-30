using UnityEngine.UIElements;

namespace Veauty.UIToolkit
{
    public class VeautyElement<State> where State : struct
    {
        private IVTree oldResolvedTree;
        private readonly VisualElement mounter;
        private readonly System.Func<State, System.Action<State>, IVTree> renderFunc;
        private readonly HookRuntime hookRuntime;
        private VisualElement rootElement;
        private bool isRendering;
        private bool renderRequested;

        private State _state;
        private State state
        {
            get => this._state;
            set
            {
                this._state = value;
                RequestRender();
            }
        }

        public VeautyElement(VisualElement mounter, System.Func<State, System.Action<State>, IVTree> renderFunc, State state = default)
        {
            this.mounter = mounter;
            this.renderFunc = renderFunc;
            this._state = state;
            this.hookRuntime = new HookRuntime(RequestRender);

            Render();
        }

        public VeautyElement(
            VisualElement mounter,
            System.Func<State, System.Action<System.Func<State, State>>, IVTree> renderFunc,
            State state = default
        )
        {
            this.mounter = mounter;
            this.renderFunc = (s, setState) => renderFunc(s, update => setState(update(s)));
            this._state = state;
            this.hookRuntime = new HookRuntime(RequestRender);

            Render();
        }

        public VeautyElement(
            VisualElement mounter,
            System.Func<State, IVTree> renderFunc,
            State state = default
        )
        {
            this.mounter = mounter;
            this.renderFunc = (s, _) => renderFunc(s);
            this._state = state;
            this.hookRuntime = new HookRuntime(RequestRender);

            Render();
        }

        public void ForceUpdate()
        {
            if (this.rootElement == null || this.isRendering)
            {
                this.renderRequested = true;
                return;
            }

            this.isRendering = true;
            try
            {
            var newTree = this.renderFunc(this.state, s => this.state = s);
                var newResolvedTree = this.hookRuntime.Resolve<VisualElement>(newTree);
                var patches = Diff<VisualElement>.Calc(this.oldResolvedTree, newResolvedTree);

                this.rootElement = Patch.Apply(this.rootElement, this.oldResolvedTree, patches);
                this.oldResolvedTree = newResolvedTree;
                this.hookRuntime.CommitEffects();
            }
            finally
            {
                this.isRendering = false;
            }

            FlushPendingRender();
        }

        private void Render()
        {
            this.isRendering = true;
            try
            {
                var tree = this.renderFunc(this.state, s => this.state = s);
                this.oldResolvedTree = this.hookRuntime.Resolve<VisualElement>(tree);
                this.rootElement = Renderer.Render(this.oldResolvedTree);
                this.mounter.Add(this.rootElement);
                this.rootElement.style.flexGrow = 1;
                this.hookRuntime.CommitEffects();
            }
            finally
            {
                this.isRendering = false;
            }

            FlushPendingRender();
        }

        private void RequestRender()
        {
            if (this.rootElement == null || this.isRendering)
            {
                this.renderRequested = true;
                return;
            }

            ForceUpdate();
        }

        private void FlushPendingRender()
        {
            if (!this.renderRequested)
            {
                return;
            }

            this.renderRequested = false;
            ForceUpdate();
        }
    }
}
