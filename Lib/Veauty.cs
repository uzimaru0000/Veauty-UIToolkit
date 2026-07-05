using UnityEngine.UIElements;

namespace Veauty.UIToolkit
{
    /// <summary>
    /// Entry point that mounts a Veauty virtual tree onto a UI Toolkit <see cref="VisualElement"/>
    /// and keeps it in sync with a state value via diff/patch re-rendering.
    /// </summary>
    /// <typeparam name="State">The state type driving the UI. Must be a value type (struct).</typeparam>
    /// <remarks>
    /// Construction renders synchronously: the render function is invoked, the resulting tree is
    /// resolved through a <see cref="HookRuntime"/> (function components and hooks), rendered to a
    /// <see cref="VisualElement"/>, added as a child of the mount element, and given
    /// <c>style.flexGrow = 1</c> so it fills the mount area. Subsequent state changes trigger
    /// Resolve → <see cref="Diff{T}.Calc"/> → <see cref="Patch.Apply"/> → effect commit.
    /// State updates issued while a render is in progress are not applied recursively; they are
    /// deferred and coalesced into a single follow-up render that runs after the current one finishes.
    /// </remarks>
    /// <example>
    /// <code>
    /// var root = GetComponent&lt;UIDocument&gt;().rootVisualElement;
    /// var app = new VeautyElement&lt;int&gt;(
    ///     root,
    ///     (count, setState) =&gt; V.Column()[
    ///         V.Label($"Count: {count}"),
    ///         V.Button("Increment", () =&gt; setState(count + 1))
    ///     ],
    ///     0
    /// );
    /// </code>
    /// </example>
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

        /// <summary>
        /// Creates and immediately mounts a Veauty tree whose render function receives the current
        /// state and a setter that replaces the state with a new value.
        /// </summary>
        /// <param name="mounter">The element the rendered tree is added to as a child.</param>
        /// <param name="renderFunc">Builds the virtual tree from the current state; the second argument replaces the state and schedules a re-render.</param>
        /// <param name="state">The initial state value.</param>
        public VeautyElement(VisualElement mounter, System.Func<State, System.Action<State>, IVTree> renderFunc, State state = default)
        {
            this.mounter = mounter;
            this.renderFunc = renderFunc;
            this._state = state;
            this.hookRuntime = new HookRuntime(RequestRender);

            Render();
        }

        /// <summary>
        /// Creates and immediately mounts a Veauty tree whose render function receives the current
        /// state and an updater that computes the next state from the current one.
        /// </summary>
        /// <param name="mounter">The element the rendered tree is added to as a child.</param>
        /// <param name="renderFunc">Builds the virtual tree from the current state; the second argument accepts a <c>State → State</c> function applied to the state captured at render time.</param>
        /// <param name="state">The initial state value.</param>
        /// <remarks>
        /// The update function is applied to the state value that was current when the render
        /// function ran, not the state at invocation time.
        /// </remarks>
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

        /// <summary>
        /// Creates and immediately mounts a Veauty tree from a state-only render function.
        /// Re-renders are driven by hooks (e.g. <c>Hooks.UseState</c>) or <see cref="ForceUpdate"/>.
        /// </summary>
        /// <param name="mounter">The element the rendered tree is added to as a child.</param>
        /// <param name="renderFunc">Builds the virtual tree from the current state.</param>
        /// <param name="state">The initial state value.</param>
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

        /// <summary>
        /// Re-runs the render function and applies the resulting diff to the mounted tree,
        /// even if the state has not changed.
        /// </summary>
        /// <remarks>
        /// If called before the initial mount completed or while another render is in progress,
        /// the update is deferred and coalesced into a single render that runs when the current
        /// one finishes. Effects registered through hooks are committed after patching.
        /// </remarks>
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
