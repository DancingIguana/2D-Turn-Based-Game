using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlashDetection : MonoBehaviour
{
    
    static Texture2D screenShot;

    void OnCollisionEnter2D(Collision2D collision)
    {    
        if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(TakeScreenShot());
            //If the GameObject has the same tag as specified, output this message in the console
            //SceneManager.LoadScene(0);
        }
    }

    IEnumerator TakeScreenShot()
    {
        yield return new WaitForEndOfFrame();
        screenShot = ScreenCapture.CaptureScreenshotAsTexture();
        // do something with texture
        Rect rec = new Rect(0,0,screenShot.width,screenShot.height);
        TransitionImage.instance.image.sprite = Sprite.Create(screenShot, rec, new Vector2(0,0));
        SceneManager.LoadScene(0);
    }
}

