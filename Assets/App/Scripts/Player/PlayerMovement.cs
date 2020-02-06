using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SpaceShooterPlus.Player
{
    [RequireComponent(typeof(Transform))]
    [RequireComponent(typeof(InputEventProvider))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private float movementSpeed = 10;

        [SerializeField]
        private Collider boundary;

        private Transform player;
        private InputEventProvider inputEventProvider;

        private void Start()
        {
            this.player = this.GetComponent<Transform>();
            this.inputEventProvider = GetComponent<InputEventProvider>();

            this.OnMoveAsObservable()
                .Select(x =>
                {
                    //Debug.Log($"{nameof(OnMoveAsObservable)}.Select()");
                    return x;
                })
                .ObserveOn(Scheduler.MainThreadFixedUpdate)
                .Select(v => v.normalized * movementSpeed * Time.deltaTime)
                .Subscribe(v =>
                {
                    var position = player.transform.position;
                    position.x = Mathf.Clamp(position.x + v.x, boundary.bounds.min.x, boundary.bounds.max.x);
                    position.y = Mathf.Clamp(position.y + v.y, boundary.bounds.min.y, boundary.bounds.max.y);
                    player.transform.position = position;
                }).AddTo(this);
        }

        private IObservable<Vector2> OnMoveAsObservable()
        {
            //Debug.Log($"{nameof(OnMoveAsObservable)}()");

            return this.UpdateAsObservable()
                .WithLatestFrom(this.inputEventProvider.Move, (_, v) => v);
        }
    }
}
