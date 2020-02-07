using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SpaceShooterPlus.MarkerStates;

namespace SpaceShooterPlus.MarkerMvp
{
    public class MarkerPresenter
    {
        private Button View;
        private MarkerModel Model;

        public MarkerPresenter(Button view, MarkerModel model)
        {
            this.View = view;
            this.Model = model;

            Debug.Log("LevelPresenter");

            var str = this.Model.GetType().ToString();
            Debug.Log(str);

            var dict = new Dictionary<Type, Action> {
                { typeof(MarkerStateLocked), () => {
                    Debug.Log("case MarkerStateLocked:");
                    this.View.interactable = false;
                } },
                { typeof(MarkerStateUnlocked), () => {
                    Debug.Log("case MarkerStateUnlocked:");
                    this.View.interactable = true;
                } },
                { typeof(MarkerStateCompleted), () => {
                    Debug.Log("case MarkerStateCompleted:");
                    var colors = this.View.colors;
                    colors.normalColor = Color.red;
                } },
            };

            dict[this.Model.State.GetType()]();
        }
    }
}
