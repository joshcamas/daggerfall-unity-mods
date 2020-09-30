using UnityEngine;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Utility;
using System;
using DaggerfallWorkshop.Game.Serialization;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;

namespace SpellcastStudios.CameraShake
{
    public class CameraShakeMod : MonoBehaviour
    {
        private static Mod mod;
        private CameraShaker cameraShaker;
        private DamageShaker damageShaker;


        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            Debug.Log("Initializing Camera Shake Mod");

            mod = initParams.Mod;

            var go = new GameObject("Camera Shake Module");
            go.AddComponent<CameraShakeMod>();

        }

        private void Start()
        {
            StartGameBehaviour.OnStartGame += OnStartGame;
            SaveLoadManager.OnLoad += OnLoad;

            mod.IsReady = true;
        }

        private void LoadSettings(ModSettings settings)
        {
            if(damageShaker != null)
            {
                damageShaker.fadeInTime = settings.GetValue<float>("Camera Shake", "fadeInTime");
                damageShaker.fadeOutTime = settings.GetValue<float>("Camera Shake", "fadeOutTime");
                damageShaker.maxShake = settings.GetValue<float>("Camera Shake", "maxShake");
                damageShaker.roughness = settings.GetValue<float>("Camera Shake", "roughness");
                damageShaker.shakeAmountAdd = settings.GetValue<float>("Camera Shake", "shakeAmountAdd");
                damageShaker.shakeAmountMultiplier = settings.GetValue<float>("Camera Shake", "shakeAmountMultiplier");
            }
        }

        private void OnLoad(SaveData_v1 saveData)
        {
            SetUpPlayer();
        }

        private void OnStartGame(object sender, EventArgs e)
        {
            SetUpPlayer();
        }

        private void SetUpPlayer()
        {
            if (!GameManager.Instance.MainCamera.transform.parent.GetComponent<CameraShaker>())
            {
                //Inject a new object between the "smooth follow" object and the camera.
                GameObject shakeObject = new GameObject();
                shakeObject.name = "Shaker";
                shakeObject.transform.parent = GameManager.Instance.MainCamera.transform.parent;
                shakeObject.transform.localPosition = Vector3.zero;
                shakeObject.transform.localRotation = Quaternion.identity;

                cameraShaker = shakeObject.AddComponent<CameraShaker>();

                GameManager.Instance.MainCamera.transform.parent = shakeObject.transform;
            }

            if (!GameManager.Instance.PlayerObject.GetComponent<DamageShaker>())
                damageShaker = GameManager.Instance.PlayerObject.gameObject.AddComponent<DamageShaker>();

            damageShaker.SetPlayer(GameManager.Instance.PlayerEntity, cameraShaker);

            LoadSettings(mod.GetSettings());
        }
    }
}