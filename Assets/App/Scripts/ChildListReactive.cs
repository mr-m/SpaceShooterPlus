using System.Linq;
using UniRx;
using UnityEngine;

namespace SpaceShooterPlus
{
    [ExecuteAlways]
    public class ChildListReactive : MonoBehaviour
    {
        public IReadOnlyReactiveCollection<GameObject> Children { get => this.children; }

        [SerializeField]
        private readonly ReactiveCollection<GameObject> children = new ReactiveCollection<GameObject>();

        private void Awake()
        {
            foreach (Transform child in this.transform)
            {
                this.children.Add(child.gameObject);
            }
        }

        private void Start()
        {
            this.UpdateChildrenList();
        }

        private void OnTransformChildrenChanged()
        {
            this.UpdateChildrenList();
        }

        private void UpdateChildrenList()
        {
            for (int i = this.children.Count - 1; i >= 0; i--)
            {
                GameObject childOld = this.children[i];

                if (!childOld.transform.IsChildOf(this.transform))
                {
                    this.children.Remove(childOld.gameObject);
                }
            }

            foreach (Transform childNew in this.transform)
            {
                if (!this.children.Contains<GameObject>(childNew.gameObject))
                {
                    this.children.Add(childNew.gameObject);
                }
            }
        }
    }
}
