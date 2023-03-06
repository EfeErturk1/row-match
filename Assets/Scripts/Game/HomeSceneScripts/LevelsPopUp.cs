using System;
using DG.Tweening;
using UnityEngine;

namespace Game.HomeSceneScripts
{
    public class LevelsPopUp:MonoBehaviour
    {
        public void StartAnimation()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(1.04f,0.06f).SetEase(Ease.OutSine)); 
            sequence.Append(transform.DOScale(1f,0.03f).SetEase(Ease.InSine));
            sequence.Play();
        }
    }
}