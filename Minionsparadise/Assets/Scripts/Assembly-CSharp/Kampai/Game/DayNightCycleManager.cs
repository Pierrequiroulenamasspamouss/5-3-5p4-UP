using UnityEngine;
using System;
using System.IO;
using Kampai.Game;
using strange.extensions.injector.api;

namespace Kampai.Game {
    public class DayNightCycleManager : MonoBehaviour {
        public static DayNightCycleManager Instance { get; private set; }

        private bool _forceNight = false;
        private bool _hasManualToggle = false;

        private Light _directionalLight;
        private Color _dayAmbient = new Color(0.5f, 0.5f, 0.5f);
        private Color _nightAmbient = new Color(0.15f, 0.15f, 0.35f);
        private Color _nightTint = new Color(0.3f, 0.35f, 0.7f);

        [Inject]
        public global::Kampai.Game.IConfigurationsService configurationsService { get; set; }

        void Awake() {
            Instance = this;
            global::UnityEngine.GameObject lightGo = global::UnityEngine.GameObject.Find("Directional Light");
            if (lightGo != null) {
                _directionalLight = lightGo.GetComponent<global::UnityEngine.Light>();
            }
        }

        void Start() {
            // Apply initial state
            Update();
        }

        void Update() {
            float factor = CalculateNightFactor();
            ApplyNightEffect(factor);
        }

        private float CalculateNightFactor() {
            // Priority 1: Config Override
            if (configurationsService != null) {
                global::Kampai.Game.ConfigurationDefinition config = configurationsService.GetConfigurations();
                if (config != null && config.night.HasValue) {
                    return config.night.Value ? 1.0f : 0.0f;
                }
            }

            // Priority 2: Manual Toggle
            if (_hasManualToggle) {
                return _forceNight ? 1.0f : 0.0f;
            }

            // Priority 3: Real Time
            int hour = DateTime.Now.Hour;
            
            // Night: 21:00 to 05:00
            // Day: 07:00 to 19:00
            // Transitions: 05:00-07:00 (Sunrise), 19:00-21:00 (Sunset)

            if (hour >= 21 || hour < 5) return 1.0f;
            if (hour >= 7 && hour < 19) return 0.0f;

            float minuteFactor = DateTime.Now.Minute / 60.0f;
            float timeAsFloat = hour + minuteFactor;

            if (timeAsFloat >= 19.0f && timeAsFloat < 21.0f) {
                return (timeAsFloat - 19.0f) / 2.0f; // Sunset
            }
            if (timeAsFloat >= 5.0f && timeAsFloat < 7.0f) {
                return 1.0f - (timeAsFloat - 5.0f) / 2.0f; // Sunrise
            }

            return 0.0f;
        }

        private void ApplyNightEffect(float factor) {
            Shader.SetGlobalColor("_GlobalNightTint", _nightTint);
            Shader.SetGlobalFloat("_GlobalNightFactor", factor);
            
            RenderSettings.ambientLight = Color.Lerp(_dayAmbient, _nightAmbient, factor);
            
            if (_directionalLight != null) {
                // Dim the sun and make it cooler/bluer
                _directionalLight.intensity = Mathf.Lerp(1.2f, 0.15f, factor);
                _directionalLight.color = Color.Lerp(Color.white, _nightTint, factor * 0.4f);
            }
        }

        public void ToggleNightMode() {
            _hasManualToggle = true;
            _forceNight = !_forceNight;
        }

        public void SetNightMode(bool night) {
            _hasManualToggle = true;
            _forceNight = night;
        }

        public void ResetToAuto() {
            _hasManualToggle = false;
        }
    }
}
