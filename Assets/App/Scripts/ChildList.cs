using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooterPlus
{
    [ExecuteAlways]
    class ChildList : MonoBehaviour
    {
        public List<GameObject> Children = new List<GameObject>();

        private void Start()
        {
            this.UpdateChildrenList();
        }

        private void Awake()
        {
            this.UpdateChildrenList();
        }

        private void OnTransformChildrenChanged()
        {
            this.UpdateChildrenList();
        }

        private void UpdateChildrenList()
        {
            this.Children.Clear();

            foreach (Transform child in this.transform)
            {
                this.Children.Add(child.gameObject);
            }
        }
    }
}
