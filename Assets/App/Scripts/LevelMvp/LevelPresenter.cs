using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace SpaceShooterPlus.LevelMvp
{
    public class LevelPresenter : MonoBehaviour
    {
        [SerializeField]
        private Text healthText;

        [SerializeField]
        private Text deathText;

        [SerializeField]
        private int initialHealth = 3;

        private LevelModel model;

        private void Start()
        {
            Debug.Log("Start");

            this.model = new LevelModel(initialHealth);

            this.model.Health
                .SubscribeToText(healthText, x => $"{x}\u2764");

            this.model.IsDead
                .Select(x => x)
                .Subscribe(x =>
                {
                    Debug.Log($"IsDead: {x}");
                    deathText.enabled = x;
                });
        }
    }
}
