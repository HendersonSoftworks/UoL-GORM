using UnityEngine;
using UnityEngine.Rendering;

namespace CRTFilter
{
    [VolumeComponentMenu("CRTFilter")]
    public class CRTVolumeComponent : VolumeComponent
    {
        //Screen geometry
        public ClampedFloatParameter screenBend = new(value: 0f, min: 0f, max: 11f);
        public ClampedFloatParameter screenOverscan = new(value: 0f, min: 0f, max: 30f);
        public ClampedFloatParameter vignetteSize = new(value: 0f, min: 0f, max: 20f);
        public ClampedFloatParameter vignetteSmooth = new(value: 2f, min: 0f, max: 10f);
        public ClampedFloatParameter vignetteRound = new(value: 25f, min: 0f, max: 100f);

        //Blur effects
        public ClampedFloatParameter blur = new(value: 0f, min: 0f, max: 10f);
        public ClampedFloatParameter bleed = new(value: 0f, min: 0f, max: 25f);
        public ClampedFloatParameter smidge = new(value: 0f, min: 0f, max: 25f);

        //Scanlines and noise
        public ClampedFloatParameter scanlinesStrength = new(value: 0f, min: 0f, max: 25f);
        public ClampedFloatParameter apertureStrength = new(value: 0f, min: 0f, max: 10f);
        public ClampedFloatParameter shadowlines = new(value: 6f, min: -50f, max: 50f);
        public ClampedFloatParameter shadowlinesSpeed = new(value: 2f, min: -50f, max: 50f);
        public ClampedFloatParameter shadowlinesAlpha = new(value: 0f, min: 0f, max: 1f);
        public ClampedFloatParameter noiseSize = new(value: 50f, min: 0f, max: 50f);
        public ClampedFloatParameter noiseSpeed = new(value: 2f, min: 0f, max: 10f);
        public ClampedFloatParameter noiseAlpha = new(value: 0f, min: 0f, max: 1f);

        //Image adjustments
        public ClampedFloatParameter brightness = new(value: 1, min: 0f, max: 2f);
        public ClampedFloatParameter contrast = new(value: 1, min: -1f, max: 3f);
        public ClampedFloatParameter gamma = new(value: 1, min: 0f, max: 2f);
        public ClampedFloatParameter red = new(value: 1, min: 0f, max: 2f);
        public ClampedFloatParameter green = new(value: 1, min: 0f, max: 2f);
        public ClampedFloatParameter blue = new(value: 1, min: 0f, max: 2f);
        public ClampedFloatParameter chromaticAberration = new(value: 1, min: -10f, max: 10f);

        public Vector2Parameter redOffset = new(value: Vector2.zero);
        public Vector2Parameter blueOffset = new(value: Vector2.zero);
        public Vector2Parameter greenOffset = new(value: Vector2.zero);
    }
}