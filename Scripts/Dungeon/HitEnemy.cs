using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HitEnemy : MonoBehaviour
{
    static Texture2D screenShot;
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Detected collision");
        if (collision.gameObject.tag == "Enemy")
        {  
            Debug.Log("Hit enemy");
            StoreEnemy.instance.playerTurn = true;
            
            StartCoroutine(TakeScreenShot());
        }
    }
    IEnumerator TakeScreenShot()
    {
    yield return new WaitForEndOfFrame();
        screenShot = ScreenCapture.CaptureScreenshotAsTexture();
        // do something with texture
        Rect rec = new Rect(0,0,screenShot.width,screenShot.height);
        TransitionImage.instance.image.sprite = Sprite.Create(screenShot, rec, new Vector2(0,0));
        SceneManager.LoadScene(2);
    }
}
