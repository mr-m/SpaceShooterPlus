using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using SpaceShooterPlus.MarkerStates;

namespace SpaceShooterPlus.MarkerMvp
{
    public class MarkerPresenter
    {
        private MarkerView View;
        private MarkerModel Model;

        public IObservable<MarkerModel> OnButtonClickAsObservable;

        public MarkerPresenter(MarkerView view, MarkerModel model)
        {
            this.View = view;
            this.Model = model;

            Debug.Log($"{nameof(MarkerPresenter)}.{nameof(MarkerPresenter)}()");

            var dict = new Dictionary<Type, Action>
            {
                [typeof(MarkerStateLocked)] = () =>
                {
                    Debug.Log($"case {nameof(MarkerStateLocked)}");
                    this.View.Button.interactable = false;
                },
                [typeof(MarkerStateUnlocked)] = () =>
                {
                    Debug.Log($"case {nameof(MarkerStateUnlocked)}");
                    this.View.Button.interactable = true;
                },
                [typeof(MarkerStateCompleted)] = () =>
                {
                    Debug.Log($"case {nameof(MarkerStateCompleted)}");
                    ColorBlock colors = this.View.Button.colors;
                    colors.normalColor = Color.red;
                    this.View.Button.colors = colors;
                }
            };

            this.Model.State.Subscribe(x =>
            {
                //Debug.Log(x.GetType());
                //Debug.Log(typeof(MarkerStateUnlocked));

                dict[x.GetType()]();
            });

            this.OnButtonClickAsObservable = this.View.Button
                .OnClickAsObservable()
                .Select(_ => this.Model);
        }
    }
}
