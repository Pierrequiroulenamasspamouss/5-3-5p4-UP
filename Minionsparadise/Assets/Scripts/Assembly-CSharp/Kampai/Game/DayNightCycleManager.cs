using System;
using UnityEngine;
using strange.extensions.injector.api;

namespace Kampai.Game {
    public class DayNightCycleManager : MonoBehaviour {
        public enum NightCycleMode {
            AUTO,
            DAY,
            NIGHT
        }

        [Inject]
        public global::Kampai.Game.IConfigurationsService configurationsService { get; set; }

        private Light _directionalLight;
        private Color _dayAmbient = new Color(0.5f, 0.5f, 0.5f);
        private Color _nightAmbient = new Color(0.15f, 0.15f, 0.35f);
        private Color _nightTint = new Color(0.3f, 0.35f, 0.7f);
        
        private NightCycleMode _currentMode = NightCycleMode.AUTO;

        void Awake() {
            global::UnityEngine.GameObject lightGo = global::UnityEngine.GameObject.Find("Directional Light");
            if (lightGo != null) {
                _directionalLight = lightGo.GetComponent<global::UnityEngine.Light>();
            }
        }

        void Start() {
            // Initial update
            ApplyNightEffect(CalculateNightFactor());
        }

        void Update() {
            float factor = CalculateNightFactor();
            ApplyNightEffect(factor);
        }

        public float CalculateNightFactor() {
            // Priority 1: Manual Mode (UI Toggle)
            if (_currentMode == NightCycleMode.DAY) return 0.0f;
            if (_currentMode == NightCycleMode.NIGHT) return 1.0f;

            // Priority 2: Config Override (Only if AUTO)
            if (configurationsService != null) {
                global::Kampai.Game.ConfigurationDefinition config = configurationsService.GetConfigurations();
                if (config != null && config.night.HasValue) {
                    return config.night.Value ? 1.0f : 0.0f;
                }
            }

            // Priority 3: Real Time (AUTO)
            int hour = DateTime.Now.Hour;
            
            // Night: 21:00 to 05:00
            if (hour >= 21 || hour < 5) return 1.0f;
            // Day: 07:00 to 19:00
            if (hour >= 7 && hour < 19) return 0.0f;

            float minuteFactor = DateTime.Now.Minute / 60.0f;
            float timeAsFloat = hour + minuteFactor;

            // Transitions: 05:00-07:00 (Sunrise)
            if (timeAsFloat >= 5.0f && timeAsFloat < 7.0f) {
                return 1.0f - (timeAsFloat - 5.0f) / 2.0f;
            }
            // Transitions: 19:00-21:00 (Sunset)
            if (timeAsFloat >= 19.0f && timeAsFloat < 21.0f) {
                return (timeAsFloat - 19.0f) / 2.0f;
            }

            return 0.0f;
        }

        private void ApplyNightEffect(float factor) {
            Shader.SetGlobalColor("_GlobalNightTint", _nightTint);
            Shader.SetGlobalFloat("_GlobalNightFactor", factor);
            
            RenderSettings.ambientLight = Color.Lerp(_dayAmbient, _nightAmbient, factor);
            
            if (_directionalLight != null) {
                _directionalLight.intensity = Mathf.Lerp(1.2f, 0.15f, factor);
                _directionalLight.color = Color.Lerp(Color.white, new Color(0.2f, 0.2f, 0.5f), factor);
            }
        }

        public void CycleNightMode() {
            if (_currentMode == NightCycleMode.AUTO) _currentMode = NightCycleMode.DAY;
            else if (_currentMode == NightCycleMode.DAY) _currentMode = NightCycleMode.NIGHT;
            else _currentMode = NightCycleMode.AUTO;
        }

        public NightCycleMode GetCurrentMode() {
            return _currentMode;
        }

        public bool IsNight() {
            return CalculateNightFactor() > 0.5f;
        }
    }
}
