using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooterPlus
{
    [ExecuteAlways]
    public class ChildList : MonoBehaviour
    {
        public IReadOnlyList<GameObject> Children { get => this.children; }

        [SerializeField]
        private readonly List<GameObject> children = new List<GameObject>();

        private void Awake()
        {
            this.UpdateChildrenList();
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
            this.children.Clear();

            foreach (Transform child in this.transform)
            {
                this.children.Add(child.gameObject);
            }
        }
    }
}
