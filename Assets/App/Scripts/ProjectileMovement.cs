using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;

    private void Start()
    {
        this.UpdateAsObservable()
            .Select(_ => speed * Time.deltaTime)
            .Subscribe(x =>
            {
                var position = this.transform.position;
                position.y += x;
                this.transform.position = position;
            });
    }
}
