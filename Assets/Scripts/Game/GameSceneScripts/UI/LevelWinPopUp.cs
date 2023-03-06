using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.GameSceneScripts.UI
{
    public class LevelWinPopUp: MonoBehaviour
    {
        public GameObject star;
        public GameObject highScoreText;
        
        public void UpdateHighScoreText(int highScore)
        {
            highScoreText.GetComponent<TextMeshPro>().text = "Highest Score\n" + highScore;
        }
        public void StartAnimation()
        {
            transform.position = new Vector3(1.65f, 2.7f, 0);
            transform.localScale = new Vector3(0.01f,0.01f,0.01f);
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(0.5f,1.2f).SetEase(Ease.OutBack));
            sequence.Join(star.transform.DORotate(new Vector3(0,0,360),1.2f,RotateMode.FastBeyond360).SetEase(Ease.OutSine));
            sequence.Append(transform.DOScale(0.4f,0.4f).SetEase(Ease.OutBack)); 
            sequence.Append(transform.DOScale(0.4f,2f).SetEase(Ease.InSine)); 
            sequence.Play().onComplete += () => GameManager.Instance.QuitLevel();
        }
    }
}