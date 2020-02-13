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
        private readonly MarkerView view;
        private readonly MarkerModel model;

        public IObservable<MarkerModel> OnButtonClickAsObservable;

        public MarkerPresenter(MarkerView markerView, MarkerModel markerModel)
        {
            this.view = markerView;
            this.model = markerModel;

            Debug.Log($"{nameof(MarkerPresenter)}.{nameof(MarkerPresenter)}()");

            var dict = new Dictionary<Type, Action>
            {
                [typeof(MarkerStateLocked)] = () =>
                {
                    Debug.Log($"case {nameof(MarkerStateLocked)}");
                    this.view.Button.interactable = false;
                },
                [typeof(MarkerStateUnlocked)] = () =>
                {
                    Debug.Log($"case {nameof(MarkerStateUnlocked)}");
                    this.view.Button.interactable = true;
                },
                [typeof(MarkerStateCompleted)] = () =>
                {
                    Debug.Log($"case {nameof(MarkerStateCompleted)}");
                    ColorBlock colors = this.view.Button.colors;
                    colors.normalColor = Color.red;
                    this.view.Button.colors = colors;
                }
            };

            this.model.State.Subscribe(x =>
            {
                //Debug.Log(x.GetType());
                //Debug.Log(typeof(MarkerStateUnlocked));

                dict[x.GetType()]();
            });

            this.OnButtonClickAsObservable = this.view.Button
                .OnClickAsObservable()
                .Select(_ => this.model);
        }
    }
}
