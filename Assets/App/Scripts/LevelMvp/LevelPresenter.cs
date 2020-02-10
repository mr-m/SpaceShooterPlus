using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SpaceShooterPlus.LevelMvp
{
    public class LevelPresenter
    {
        private readonly LevelView view;
        private readonly LevelModel model;

        public System.IObservable<LevelModel> OnButtonClickAsObservable;

        public LevelPresenter(LevelView view, LevelModel model)
        {
            this.view = view;
            this.model = model;

            Debug.Log($"{nameof(LevelPresenter)}()");

            this.model.Health
                .SubscribeToText(this.view.HealthText, x => $"\u2764 {x}");

            this.model.Score
                .SubscribeToText(this.view.ScoreText, x => $"{x} \u2605");

            this.model.IsPlaying
                .Select(x => x)
                .Subscribe(x =>
                {
                    Debug.Log($"{nameof(this.model.IsPlaying)}: {x}");
                    this.view.Player.SetActive(x);
                    this.view.HazardSpawner.gameObject.SetActive(x);

                    this.view.Background.gameObject.SetActive(!x);
                    this.view.MenuButton.gameObject.SetActive(!x);
                    this.view.RestartButton.gameObject.SetActive(!x);
                });

            this.model.IsDefeat
                .Select(x => x)
                .Subscribe(x =>
                {
                    Debug.Log($"{nameof(this.model.IsDefeat)}: {x}");
                    this.view.DefeatText.gameObject.SetActive(x);
                });

            this.model.IsVictory
                .Select(x => x)
                .Subscribe(x =>
                {
                    Debug.Log($"{nameof(this.model.IsVictory)}: {x}");
                    this.view.VictoryText.gameObject.SetActive(x);
                });

            this.view.Player.gameObject.OnTriggerEnterAsObservable()
                .Where(other => other.gameObject.CompareTag("Enemy"))
                .Subscribe(other =>
                {
                    this.model.Health.Value--;
                    Object.Destroy(other.gameObject);
                });

            this.view.HazardSpawner.Spawned
                .Subscribe(hazard => hazard
                    .OnTriggerEnterAsObservable()
                    .Where(other => other.gameObject.CompareTag("Bullet"))
                    .Select(other => other.gameObject)
                    .Subscribe(bullet =>
                    {
                        Debug.Log($"Destroyed {hazard} {bullet}");
                        Object.Destroy(hazard);
                        Object.Destroy(bullet);
                        this.model.Score.Value++;
                    }));

            this.view.RestartButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    this.view.Player.gameObject.transform.position = Vector3.zero;
                    this.model.Health.Value = this.model.InitialHealth.Value;
                    this.model.Score.Value = 0;
                });

            this.OnButtonClickAsObservable = this.view.MenuButton
                .OnClickAsObservable()
                .Select(_ => this.model);
        }
    }
}
