using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RhythmSelected : MonoBehaviour
{
    [SerializeField] private RhythmMusicSelectCanvas musicSelect;
    [SerializeField] private GameObject musicPrefab;
    [SerializeField] private MusicElement musicElements;
    [SerializeField] Sprite[] selectBackImg;
    [SerializeField] Transform[] parentTr;

    private GameObject[] musicButtons;
    private int selectedButtonIndex = -1;

    public int trCount;

    private void Start()
    {
        musicButtons = new GameObject[musicElements.musicList.Length];

        for (int i = 0; i < musicElements.musicList.Length; i++)
        {
            trCount = i;
            trCount = (trCount <= 5) ? 0 : (trCount >= 6 && trCount <= 11) ? 1 : 2;

            var parentObj = parentTr[trCount];
            var musicElement = musicElements.musicList[i];            
            var musicPrefabInstance = Instantiate(musicPrefab, parentObj);
            musicButtons[i] = musicPrefabInstance;

            Transform buttonTransform = musicPrefabInstance.transform;
            buttonTransform.Find("icon").GetComponent<Image>().sprite = musicElement._albumImg;
            buttonTransform.Find("name").GetComponent<TMP_Text>().text = musicElement._musicName;

            Transform levelIcons = buttonTransform.Find("levelIcon");
            for (int j = 0; j < levelIcons.childCount; j++)
            {
                Image levelIcon = levelIcons.GetChild(j).GetComponent<Image>();
                levelIcon.sprite = (j < musicElement._level) ? musicElement._fillLevelIcon : musicElement._voidLevelIcon;
            }

            int index = i;
            musicButtons[i].GetComponent<TouchEventHandler>().m_PressedEvent.AddListener(val =>
            {
                SelectMusicBtn(index);
            });
        }
    }

    public void SelectMusicBtn(int index)
    {        
        if (selectedButtonIndex == index)
        {
            Managers.Sound.MusicSoundMute();
            SetButtonColor(index, selectBackImg[0], false);
            selectedButtonIndex = -1;
            musicSelect.MusicDisable();
        }
        else
        {
            if (selectedButtonIndex != -1)
            {
                Managers.Sound.MusicSoundMute();
                musicSelect.MusicDisable();
                SetButtonColor(selectedButtonIndex, selectBackImg[0], false);
            }

            SetButtonColor(index, selectBackImg[1],true);
            selectedButtonIndex = index;
            musicSelect.MusicSelect();
            Managers.SelGameSet.selectMusic = musicElements.musicList[selectedButtonIndex];
            Managers.Sound.playMusicSound(musicElements.musicList[selectedButtonIndex]._clip,false);
        }
    }

    private void SetButtonColor(int index, Sprite img , bool aniPlay)
    {
        Image buttonImage = musicButtons[index].transform.Find("back").GetComponent<Image>();
        Animator animator = musicButtons[index].transform.Find("levelIcon").GetComponent<Animator>();
        animator.SetBool("Select", aniPlay);
        buttonImage.sprite = img;
    }

    public void NextPage()
    {
        if (selectedButtonIndex == -1) return;
        SetButtonColor(selectedButtonIndex, selectBackImg[0], false);
        Managers.Sound.MusicSoundMute();
        musicSelect.MusicDisable();
    }    
}