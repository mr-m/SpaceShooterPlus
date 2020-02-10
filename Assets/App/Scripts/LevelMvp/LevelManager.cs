using SpaceShooterPlus.Player;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SpaceShooterPlus.LevelMvp
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        public int InitialHealth = 3;

        [SerializeField]
        public int RequiredScore = 5;

        public System.IObservable<LevelModel> OnButtonClickAsObservable;

        private void Start()
        {
            var levelView = FindObjectOfType<LevelView>();
            var levelModel = new LevelModel(RequiredScore, InitialHealth);
            var levelPresenter = new LevelPresenter(levelView, levelModel);

            this.OnButtonClickAsObservable = levelPresenter.OnButtonClickAsObservable;
        }
    }
}
