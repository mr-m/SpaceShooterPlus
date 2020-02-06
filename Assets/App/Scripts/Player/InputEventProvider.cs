using UnityEngine;
using UniRx;

namespace SpaceShooterPlus.Player
{
    public class InputEventProvider : MonoBehaviour
    {
        public IReadOnlyReactiveProperty<Vector2> Move => this.move;
        public IReadOnlyReactiveProperty<bool> Fire => this.fire;

        private readonly ReactiveProperty<Vector2> move = new ReactiveProperty<Vector2>();
        private readonly ReactiveProperty<bool> fire = new ReactiveProperty<bool>();

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

            this.ObserveEveryValueChanged(_ =>
            {
                bool v = Input.GetButton("Fire1");
                //Debug.Log($"Fire: {v}");
                return v;
            })
            .Subscribe(x => this.fire.Value = x);
        }
    }
}
