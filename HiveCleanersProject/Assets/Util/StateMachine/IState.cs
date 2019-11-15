
namespace Util.StateMachine {
    public interface IState<in TSource, in TInput> {
        void OnEnter(TSource source);
        void OnExit(TSource source);
        void Update(TSource source);
        void HandleInput(TSource source, TInput input);
    }
}