using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

namespace DigitalMaru.MiniGame.MusicMovement
{
    public class ResultManager : MonoBehaviour
    {
        [Header("종료창에 표시할 것들")]
        public List<Sprite> titleimage;
        public Sprite star;
        public EndView endView;
        public GameObject endview_Auto;
        public GameObject endview_Free;
        [SerializeField] GameObject rankTransform;

        // 게임 속성
        int playMode;
        int musicNum;
        string musicName;
        string artistName;

        Player[] players;

        [Header("Rank")]
        public Rank _rank_prefebs;
        private List<int> rankList = new();

        // 게임매니저 Init에서 호출
        public void Init(RhythmGameSettings settings, Player[] _players)
        {
            // 속성 설정
            musicNum = settings.MusicNum;
            musicName = settings.MusicName;
            artistName = settings.ArtistName;
            playMode = settings.PlayMode;

            players = _players;

            endView.gameObject.SetActive(false);
            LoadRankings();
        }

        public void ShowResult()
        {
            switch(playMode)
            {
                case 0: endview_Auto.SetActive(true); break;    // 자동 연주
                case 1: Endview(); break;           // 일반 연주
                case 2: default: break;             // 자유 연주
            }
        }

        // 종료창 설정
        private void Endview()
        {
            endView.gameObject.SetActive(true);
            endView.SetEndView(titleimage[musicNum], musicName, artistName, star);

            StartCoroutine(nameof(SetResultScore));
        }

        // 점수 계산
        public IEnumerator SetResultScore()
        {
            for (int i = 0; i < players.Length; i++)
            {
                // Player
                PlayerScore player = players[i].GetPlayerScore();
                player.SetMaxCombo();
                List<TMP_Text> result_list = endView.GetEndViewList(i);

                List<int> result_int = new List<int> { player.Score, player.maxCombo, player.perfectCount, player.goodCount, player.missCount };
                rankList.Add(player.Score);

                for (int list = 0; list < 5; list++)
                {
                    // 결과창 연출 (랜덤한 숫자 반복)
                    for (int five = 0; five < 5; five++)
                    {
                        yield return YieldInstructionCache.WaitForSeconds(0.1f);
                        for (int num = list; num < 5; num++)
                        {
                            result_list[num].text = Random.Range(0, result_int[num]).ToString();
                        }
                    }

                    result_list[list].text = result_int[list].ToString();
                }
            }

            // 랭킹의 개수가 5개가 안되면, 0을 삽입
            if (rankList.Count < 5)
            {
                int rankcount = 5 - rankList.Count;
                for (int i = 0; i < rankcount; i++)
                {
                    rankList.Add(0);
                }
            }

            // 점수를 큰순서로 정렬
            rankList = rankList.OrderByDescending(num => num).ToList(); 

            // 랭크 생성
            for (int i = 0; i < 5; i++)
            {
                Rank targetRank = Instantiate(_rank_prefebs, rankTransform.transform);

                // 4등 이상
                if (i > 2) targetRank.SetRank(rankList[i].ToString() + " Score", (i + 1).ToString(), i);
                // 1, 2, 3 등
                else targetRank.SetRank(rankList[i].ToString() + " Score", "", i);

            }

            // 랭킹 순차적으로 Up DoAnim
            int ypotion = 670;
            for (int i = 0; i < 5; i++)
            {
                rankTransform.transform.GetChild(i).DOLocalMoveY(ypotion, 0.5f);
                yield return YieldInstructionCache.WaitForSeconds(0.5f);
                if (i == 0)
                    endView.GetParticle().Play();
                ypotion -= 70;
            }

            SaveRankings();
        }
        
        // PlayerPrefs에 랭크 저장
        public void SaveRankings()
        {
            PlayerPrefs.DeleteKey("Ranking" + musicNum); // 기존 랭킹 정보를 모두 삭제합니다.

            string ranklist = "";

            for (int i = 0; i < 5; i++)
            {
                ranklist += rankList[i].ToString();
                ranklist += ",";
            }
            PlayerPrefs.SetString("Ranking" + musicNum, ranklist);

            PlayerPrefs.Save(); // 변경 사항을 저장합니다.
        }

        // PlayerPrefs로 부터 랭크 불러오기
        public void LoadRankings()
        {
            if (PlayerPrefs.HasKey("Ranking" + musicNum))
            {
                string[] ranklist = PlayerPrefs.GetString("Ranking" + musicNum).Split(",");
                for (int i = 0; i < ranklist.Length - 1; i++)
                {
                    int score = int.Parse(ranklist[i]);
                    rankList.Add(score);
                }
                for (int i = ranklist.Length - 1; i < 5; i++) // 없는 부분은 0으로 채우기
                {
                    rankList.Add(0);
                }
            }
        }
        
    }
}