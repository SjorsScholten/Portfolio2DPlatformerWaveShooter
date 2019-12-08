public interface IState<TSource> {
        void OnEnter(TSource source);
        void OnExit(TSource source);
        void Update(TSource source);
        void HandleInput(TSource source);
}