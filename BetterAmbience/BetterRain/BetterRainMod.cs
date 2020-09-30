using UnityEngine;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;
using DaggerfallWorkshop;
using System;
using UnityEngine.UIElements;

namespace SpellcastStudios.BetterRain
{
    public class BetterRainMod : MonoBehaviour
    {
        private static Mod mod;
        private ModSettings settings;

        private bool enableBetterRain;
        private bool enableBetterSnow;

        private ParticleSystem rainSystem;
        private bool wasStorming;

        private int rainAmount = 2000;
        private int stormAmount = 4000;

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            Debug.Log("Initializing Better Rain Mod");
            mod = initParams.Mod;

            var go = new GameObject("Better Rain Module");
            go.AddComponent<BetterRainMod>();
        }

        private void Start()
        {
            mod.IsReady = true;

            LoadSettings(mod.GetSettings());

            var systems = GameManager.Instance.PlayerObject.GetComponentsInChildren<ParticleSystem>(true);

            foreach(var system in systems)
            {
                //Rain!
                if (system.gameObject.name == "Rain_Particles" && enableBetterRain)
                {
                    rainSystem = system;
                    SetRainSettings(system);
                }

                //Snow!
                if (system.gameObject.name == "Snow_Particles" && enableBetterSnow)
                    SetSnowSettings(system);
            }
        }

        private void Update()
        {
            if (!enableBetterRain)
                return;

            if (GameManager.Instance.WeatherManager.IsStorming != wasStorming)
            {
                UpdateStormState();
                wasStorming = GameManager.Instance.WeatherManager.IsStorming;
            }
        }

        private void UpdateStormState()
        {
            var emission = rainSystem.emission;
            int count = GameManager.Instance.WeatherManager.IsStorming ? stormAmount : rainAmount;

            emission.rateOverTime = new ParticleSystem.MinMaxCurve(count);
        }

        private void LoadSettings(ModSettings settings)
        {
            enableBetterRain = settings.GetValue<bool>("Better Rain", "enableBetterRain");
            enableBetterSnow = settings.GetValue<bool>("Better Rain", "enableBetterSnow");
        }

        private void SetSnowSettings(ParticleSystem system)
        {
            var render = system.GetComponent<ParticleSystemRenderer>();
            render.material.SetFloat("_InvFade", 3);
        }

        private void SetRainSettings(ParticleSystem system)
        {
            system.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);

            var main = system.main;
            main.startSpeed = new ParticleSystem.MinMaxCurve(100);
            main.startSize = new ParticleSystem.MinMaxCurve(0.16f,2);
            main.startColor = new ParticleSystem.MinMaxGradient(new Color(0.575f, 0.575f, 0.575f, 0.8f), new Color(0.575f, 0.575f, 0.575f, 0.9f));

            var shape = system.shape;
            shape.scale = new Vector3(100, 100, 0);

            var forceOverLifetime = system.forceOverLifetime;
            forceOverLifetime.x = new ParticleSystem.MinMaxCurve(10, 20);
            forceOverLifetime.y = new ParticleSystem.MinMaxCurve(0);
            forceOverLifetime.z = new ParticleSystem.MinMaxCurve(0);

            var render = system.GetComponent<ParticleSystemRenderer>();
            render.material.SetFloat("_InvFade", 3);
            render.minParticleSize = 0.0015f;
            render.maxParticleSize = 0.0015f;
            render.lengthScale = 6f;

        }

    }
}