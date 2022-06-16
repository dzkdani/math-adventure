using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class IntroManager : MonoBehaviour
{
    public Button btnStart;
    public CanvasGroup Landing;
    public GameObject Intro;
    public List<Pics> scenes;

    public Image blackfilm;
    public GameObject UI;
    public Sprite lagiPlay;
    public Sprite lagiPause;
    // Start is called before the first frame update

    [System.Serializable]
    public class Pics
    {
        public GameObject textBox;
        public TextMeshProUGUI text;
        public List<Image> take;
    }

    Sequence sequence;

    void LandingAppear(bool isFadeOut = false)
    {
        sequence.Kill();
        sequence = DOTween.Sequence();

        int alpha = 1;
        if (isFadeOut) alpha = 0;

        sequence.Append(Landing.DOFade(alpha, 1f).OnComplete(()=> btnStart.interactable = !isFadeOut));
        sequence.PrependInterval(1);
    }

    void Start()
    {
       // scenes[0].sequence = PicOne(scenes[0]);
        LandingAppear();
        sequence.Play();

        btnStart.onClick.AddListener(() => {
            if (PlayerDataManager.Instance.IsDoneIntro)
            {
                LandingAppear(true);
                sequence.OnComplete(() => { SceneManager.LoadScene("MainMenu"); });
                sequence.Play();
            }
            else
            {
                LandingAppear(true);
                Intro.SetActive(true);
                sequence.Append(PicOne(scenes[0]));
                sequence.Append(PicTwo(scenes[1]));
                sequence.Append(PicThree(scenes[2]));
                sequence.Append(PicThree(scenes[3]));
                sequence.Append(PicTwo(scenes[4]));
                sequence.Append(PicSix(scenes[5]));
                sequence.Append(PicSeven(scenes[6]));

                sequence.Play();

                PlayerDataManager.Instance.IsDoneIntro = true;
                PlayerDataManager.Instance.SaveStatusIntro();
                AudioManager.Instance.PlaySFX(7);
            }
        });
    }

    public void Skip()
    {
        blackfilm.gameObject.SetActive(true);
        sequence.Kill();
        blackfilm.DOFade(1, 1).OnComplete(()=> { SceneManager.LoadScene("MainMenu"); });
    }

    public void Pause(Image e)
    {
        if (e.sprite.Equals(lagiPlay)) { e.sprite = lagiPause; }
        else { e.sprite = lagiPlay; }
        sequence.TogglePause();
    }

    Sequence PicOne(Pics e)
    {
        Sequence temp = DOTween.Sequence();

        temp.Append(e.take[0].DOFade(1, 0.5f).OnStart(()=> {
            UI.SetActive(true);
            e.take[0].transform.DOLocalMoveY(299, 2f);
        }).SetDelay(1));

        temp.Append(e.take[3].DOFade(1, 0.5f).OnStart(() => e.take[3].transform.DOLocalMoveX(43, 2f)).SetDelay(1));
        temp.Append(e.take[1].DOFade(1, 0.5f).OnStart(() => e.take[1].transform.DOLocalMoveX(100, 2f)).SetDelay(1));
        temp.Append(e.take[4].DOFade(1, 0.5f).OnStart(() => e.take[4].transform.DOLocalMoveY(42, 2f)).SetDelay(1));
        temp.Append(e.take[2].DOFade(1, 0.5f).OnStart(() => e.take[2].transform.DOLocalMoveX(243, 3f)).SetDelay(1));

        temp.Append(e.take[5].DOFade(1, 1f).OnStart(() => {
            for (int i = 0; i < 5; i++) { e.take[i].gameObject.SetActive(false); }
            e.textBox.SetActive(true);

        }).SetDelay(1.5f));

        temp.Append(e.take[5].DOFade(0, 1f).OnStart(() => {
            e.textBox.SetActive(false);
        }).SetDelay(3.5f));
        return temp;
    }

    Sequence PicTwo(Pics e)
    {
        Sequence temp = DOTween.Sequence();
        temp.Append(e.take[0].DOFade(1, 1f).OnComplete(() => e.textBox.SetActive(true)));
        temp.Append(e.take[0].DOFade(0, 1f).OnComplete(() => e.textBox.SetActive(false)).SetDelay(3.5f));
        return temp;
    }

    Sequence PicThree(Pics e)
    {
        Sequence temp = DOTween.Sequence();
        temp.Append(e.take[0].DOFade(1, 1f).OnComplete(() => e.textBox.SetActive(true)));
        temp.Append(e.take[1].DOFade(1, 1f).OnComplete(() => e.take[0].gameObject.SetActive(false)).SetDelay(1));
        temp.Append(e.take[1].DOFade(0, 1f).OnComplete(() => e.textBox.SetActive(false)).SetDelay(2.5f));
        return temp;
    }

    Sequence PicSix(Pics e)
    {
        Sequence temp = DOTween.Sequence();
        temp.Append(e.take[0].transform.DOScale(1, 0.7f).OnComplete(() => e.textBox.SetActive(true)));
        temp.Append(e.take[0].DOFade(0, 1f).OnComplete(() => e.textBox.SetActive(false)).SetDelay(4.5f));
        return temp;
    }

    Sequence PicSeven(Pics e)
    {
        Sequence temp = DOTween.Sequence();
        temp.Append(e.take[0].DOFade(1, 1f).OnPlay(() => { e.textBox.SetActive(true); })
            .OnComplete(() => {
            e.take[0].transform.DOScale(1, 2f);
            e.take[0].transform.DOMove(Vector3.zero, 2f);
            e.take[0].transform.DORotate(Vector3.zero, 2f).OnComplete(()=> {
                e.take[1].gameObject.SetActive(true);
                e.text.SetText("Apakah mereka akan berhasil?");
            });
        }));

        temp.Append(e.take[0].DOFade(0, 1f).OnComplete(() => {
            e.textBox.SetActive(false);
            e.take[1].gameObject.SetActive(false);
            SceneManager.LoadScene("MainMenu");
        }).SetDelay(4.5f)); 
        return temp;
    }
}
