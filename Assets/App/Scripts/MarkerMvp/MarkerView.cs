using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooterPlus.MarkerMvp
{
    [ExecuteAlways]
    [RequireComponent(typeof(Button))]
    public class MarkerView : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        public Button Button
        {
            get => this.button;
            private set => this.button = value;
        }

        private void Awake()
        {
            Debug.Log($"{nameof(MarkerView)}.{nameof(Awake)}()");

            this.Button = GetComponent<Button>();
        }
    }
}
