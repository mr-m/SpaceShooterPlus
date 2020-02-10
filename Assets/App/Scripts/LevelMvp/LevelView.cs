using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooterPlus.LevelMvp
{
    [ExecuteAlways]
    public class LevelView : MonoBehaviour
    {
        [SerializeField]
        public Image Background;

        [SerializeField]
        public Text HealthText;

        [SerializeField]
        public Text ScoreText;

        [SerializeField]
        public Text DefeatText;

        [SerializeField]
        public Text VictoryText;

        [SerializeField]
        public Button MenuButton;

        [SerializeField]
        public Button RestartButton;

        [SerializeField]
        public GameObject Player;

        [SerializeField]
        public Spawner HazardSpawner;

        private void Awake()
        {
        }
    }
}
