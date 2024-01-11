
using System;
using System.Collections;
using DG.Tweening;
using Game_DrawCar;
using Sirenix.OdinInspector;
using TigerForge;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game_Fly
{
    public partial class GameManager : SerializedMonoBehaviour
    {

        public static GameManager Instance;


        [FoldoutGroup("Persistant Component", false)]
        [SerializeField] private UiManager uiManager;
        [FoldoutGroup("Total Level")]
        public int totalLevel;

        public UiManager UiController => uiManager;
        public Profile Profile { get; private set; }

        public int levelPlaying;
        public GamePlayManager gamePlayManager;
        public LoadLevelManager loadLevelManager;
        public AddressablesUtils addressableGame;
        public bool isStartGame = false;

       
        private void Awake()
        {
            Instance = this;
            levelPlaying = DataLevel.GetLevel();
            Profile = new Profile();
            AudioController.Instance.PlayBackgroundMusic();
            DOTween.Init().SetCapacity(200, 125);
        }

        public void IncreaseLevel(int level)
        {
            if (DataLevel.GetLevel() == totalLevel)
            {
                return;
            }
            int currentLevel = DataLevel.GetLevel();
            level++;
            levelPlaying = level;
            if (level > currentLevel)
            {
                DataLevel.SetLevel(level);
            }
            
        }

        public void SkipLevel()
        {
            IncreaseLevel(levelPlaying);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            DOTween.KillAll();
        }

        
    }
}

