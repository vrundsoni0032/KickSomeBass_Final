using UnityEngine;

// temp script
public class IM_AreaColliderHandler : MonoBehaviour
{
    public GameObject TutorialSignPost;
    public GameObject Socky;
    public GameObject Chocky;
    public GameObject initialTutorial;
    public GameObject TutorialLakeBlock;
    public GameObject Area2Block;
    public GameObject Area3Block;
    public GameObject AbandonedAreaBlockBlock;
    public GameObject FinalLighthouseBlock;

    void Update()
    {
       // switch (GameUtil.PlayerState.CurrentAreaAccess)
       // {
       //     case AreaSequence.StarterIsland:
       //         break;
       //
       //     case AreaSequence.TutorialLake:
       //         initialTutorial.SetActive(false);
       //         TutorialLakeBlock.SetActive(false);
       //         break;
       //
       //     case AreaSequence.Area2:
       //         TutorialSignPost.SetActive(true);
       //         Chocky.SetActive(false);
       //         Socky.SetActive(false);
       //         initialTutorial.SetActive(false);
       //         TutorialLakeBlock.SetActive(false);
       //         Area2Block.SetActive(false);
       //         break;
       //
       //     case AreaSequence.Area3:
       //         initialTutorial.SetActive(false);
       //         TutorialLakeBlock.SetActive(false);
       //         Area2Block.SetActive(false);
       //         Area3Block.SetActive(false);
       //         break;
       //
       //     case AreaSequence.FinalLightHouse:
       //         initialTutorial.SetActive(false);
       //         TutorialLakeBlock.SetActive(false);
       //         Area2Block.SetActive(false);
       //         Area3Block.SetActive(false);
       //         AbandonedAreaBlockBlock.SetActive(false);
       //         FinalLighthouseBlock.SetActive(false);
       //         break;
       // }
    }
}