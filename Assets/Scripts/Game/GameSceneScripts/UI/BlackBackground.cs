using UnityEngine;
using System.Collections;

namespace Game.GameSceneScripts.UI
{
    public class BlackBackground : MonoBehaviour
    {
        public void StartFadeOut(float fadeTime, float waitTime)
        {
            StartCoroutine(FadeOutCoroutine(fadeTime, waitTime));
        }

        private IEnumerator FadeOutCoroutine(float fadeTime, float waitTime)
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            float startAlpha = 0f;
            float targetAlpha = 0.8f;
            float elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeTime);
                var color = renderer.color;
                renderer.color = new Color(color.r, color.g, color.b, newAlpha);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, targetAlpha);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
