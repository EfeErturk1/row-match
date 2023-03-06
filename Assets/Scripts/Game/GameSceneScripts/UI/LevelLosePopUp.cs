using System;
using DG.Tweening;
using UnityEngine;

namespace Game.GameSceneScripts.UI
{
    public class LevelLosePopUp: MonoBehaviour
    {
        public void StartAnimation()
        {
            transform.position = new Vector3(1.65f, 7.7f, 0);
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOLocalMoveY(2.6f,0.5f).SetEase(Ease.OutSine)); 
            sequence.Append(transform.DOLocalMoveY(2.7f,0.1f).SetEase(Ease.InSine));
            sequence.Append(transform.DOLocalMoveY(2.7f,1f).SetEase(Ease.InSine));
            sequence.Play().onComplete += () => GameManager.Instance.QuitLevel();

        }
    }
}