using UnityEngine;
using UniRx;

namespace SpaceShooterPlus.Player
{
    public class InputEventProvider : MonoBehaviour
    {
        public IReadOnlyReactiveProperty<Vector2> Move => this.move;

        private readonly ReactiveProperty<Vector2> move = new ReactiveProperty<Vector2>();

        private void Start()
        {
            this.ObserveEveryValueChanged(_ =>
            {
                var v = new Vector2(
                    Input.GetAxis("Horizontal"),
                    Input.GetAxis("Vertical"));
                //Debug.Log($"Move: {v}");
                return v;
            })
            .Subscribe(x => this.move.Value = x);
        }
    }
}
