using UnityEngine;
 
public class StageBackgroundController : MonoBehaviour
{
    public SpriteRenderer[] StageBackgroundRenderers; 

    private StageData stageData;

    void Start()
    {
        stageData = StageManager.Instance.GetCurStageData();

        DisplayBackground();
    }
    private void DisplayBackground()
    {
        for(int i = 0; i < StageBackgroundRenderers.Length; i++)
        {
            StageBackgroundRenderers[i].sprite = stageData.StageBackgroundList[i];
        }
    }
}
