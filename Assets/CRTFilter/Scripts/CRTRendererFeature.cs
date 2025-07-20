using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

namespace CRTFilter
{
    public class CRTRendererFeature : ScriptableRendererFeature
    {
        public enum Presets
        {
            none,
            subtle,
            retro,
            strong,
            oldCrt,
            arcade,
            custom
        }

        public Shader shader;
        public Shader shaderLite;
        public bool useShaderLite = false;
        public bool useVolumeComponent = false;

        public RenderPassEvent injectionPoint = RenderPassEvent.BeforeRenderingPostProcessing;
        public Presets preset;

        [Range(0f, 640f)]
        public float pixelResolutionX = 320;
        [Range(0f, 640f)]
        public float pixelResolutionY = 200;

        [Header("Screen geometry")]
        [Range(0f, 11f)]
        public float screenBend = 4f;
        [Range(0f, 30f)]
        public float screenOverscan = 1f;
        [Range(0f, 20f)]
        public float vignetteSize = 5.7f;
        [Range(0f, 10f)]
        public float vignetteSmooth = 2f;
        [Range(0f, 100f)]
        public float vignetteRound = 25f;

        [Header("Blur effects")]
        [Range(0f, 10f)]
        public float blur = 0f;
        [Range(0f, 25f)]
        public float bleed = 1f;
        [Range(0f, 25f)]
        public float smidge = 0f;

        [Header("Scanlines and noise")]
        [Range(0f, 25f)]
        public float scanlinesStrength = 6f;
        [Range(0f, 10f)]
        public float apertureStrength = 0.5f;
        [Range(-50f, 50f)]
        public float shadowlines = 6f;
        [Range(-50f, 50f)]
        public float shadowlinesSpeed = 2f;
        [Range(0f, 1f)]
        public float shadowlinesAlpha = 0.1f;
        [Range(0f, 50f)]
        public float noiseSize = 50f;
        [Range(0f, 10f)]
        public float noiseSpeed = 2f;
        [Range(0f, 1f)]
        public float noiseAlpha = 0.1f;

        [Header("Image adjustments")]
        [Range(0f, 2f)]
        public float brightness = 1;
        [Range(-1f, 3f)]
        public float contrast = 1;
        [Range(0f, 2f)]
        public float gamma = 1;
        [Range(0f, 2f)]
        public float red = 1;
        [Range(0f, 2f)]
        public float green = 1;
        [Range(0f, 2f)]
        public float blue = 1;
        [Range(-10f, 10f)]
        public float chromaticAberration = 1;

        public Vector2 redOffset = new Vector2(0.1f, -0.1f);
        public Vector2 blueOffset = new Vector2(0, 0.1f);
        public Vector2 greenOffset = new Vector2(-0.1f, 0f);

        private CRTRenderPass crtRenderPass;
        private Material shaderMaterial;

        #region Settings presets

        public void OnValidate()
        {
            if (useVolumeComponent)
                preset = Presets.none;

            switch (preset)
            {
                case Presets.none:
                    screenBend = 0;
                    screenOverscan = 0;
                    blur = 0;
                    bleed = 0;
                    smidge = 0;
                    scanlinesStrength = 0;
                    apertureStrength = 0;
                    shadowlines = 0;
                    shadowlinesSpeed = 0;
                    shadowlinesAlpha = 0;
                    vignetteSize = 0;
                    vignetteSmooth = 0;
                    vignetteRound = 0;
                    noiseSize = 0;
                    noiseAlpha = 0;
                    noiseSpeed = 0;
                    brightness = 1;
                    contrast = 1;
                    gamma = 1;
                    red = 1;
                    green = 1;
                    blue = 1;
                    chromaticAberration = 0;
                    redOffset = Vector2.zero;
                    blueOffset = Vector2.zero;
                    greenOffset = Vector2.zero;
                    break;
                case Presets.subtle:
                    screenBend = 0.51f;
                    screenOverscan = 0;
                    blur = 0.5f;
                    bleed = 0;
                    smidge = 0;
                    scanlinesStrength = 1;
                    apertureStrength = 0.1f;
                    shadowlines = 0;
                    shadowlinesSpeed = 0;
                    shadowlinesAlpha = 0;
                    vignetteSize = 5.7f;
                    vignetteSmooth = 2;
                    vignetteRound = 63;
                    noiseSize = 0;
                    noiseAlpha = 0;
                    noiseSpeed = 0;
                    chromaticAberration = 0;
                    break;
                case Presets.retro:
                    screenBend = 0.05f;
                    screenOverscan = 0f;
                    blur = 0.5f;
                    bleed = 1.1f;
                    smidge = 14;
                    scanlinesStrength = 6.6f;
                    apertureStrength = 0.7f;
                    shadowlines = 0;
                    shadowlinesSpeed = 0;
                    shadowlinesAlpha = 0;
                    vignetteSize = 5.7f;
                    vignetteSmooth = 3.6f;
                    vignetteRound = 33.3f;
                    noiseSize = 0;
                    noiseAlpha = 0;
                    noiseSpeed = 0;
                    chromaticAberration = 0;
                    break;
                case Presets.strong:
                    screenBend = 6.5f;
                    screenOverscan = 0.5f;
                    blur = 0.8f;
                    bleed = 0;
                    smidge = 0;
                    scanlinesStrength = 2.8f;
                    apertureStrength = 1;
                    shadowlines = 3.5f;
                    shadowlinesSpeed = 0.5f;
                    shadowlinesAlpha = 0.1f;
                    vignetteSize = 5.7f;
                    vignetteSmooth = 2.8f;
                    vignetteRound = 70;
                    noiseSize = 0;
                    noiseAlpha = 0;
                    noiseSpeed = 0;
                    chromaticAberration = 0.5f;
                    break;
                case Presets.oldCrt:
                    screenBend = 8.3f;
                    screenOverscan = 1.5f;
                    blur = 1;
                    bleed = 0.1f;
                    smidge = 0;
                    scanlinesStrength = 9;
                    apertureStrength = 4;
                    shadowlines = 3.5f;
                    shadowlinesSpeed = 1.5f;
                    shadowlinesAlpha = 0.2f;
                    vignetteSize = 5.7f;
                    vignetteSmooth = 2;
                    vignetteRound = 87;
                    noiseSize = 26;
                    noiseAlpha = 0.25f;
                    noiseSpeed = 7.2f;
                    chromaticAberration = 1.5f;
                    break;
                case Presets.arcade:
                    screenBend = 7.2f;
                    screenOverscan = 0.5f;
                    blur = 0;
                    bleed = 3;
                    smidge = 15;
                    scanlinesStrength = 9;
                    apertureStrength = 4;
                    shadowlines = 0;
                    shadowlinesSpeed = 0;
                    shadowlinesAlpha = 0;
                    vignetteSize = 5.7f;
                    vignetteSmooth = 1;
                    vignetteRound = 85;
                    noiseSize = 0;
                    noiseAlpha = 0;
                    noiseSpeed = 0;
                    chromaticAberration = 1;
                    break;
                case Presets.custom:
                default:
                    break;
            }

            if (useShaderLite)
            {
                blur = 0;
                smidge = 0;
                apertureStrength = 0;
                brightness = 1;
                contrast = 1;
                gamma = 1;
                red = 1;
                green = 1;
                blue = 1;
                chromaticAberration = 0;
                redOffset = Vector2.zero;
                blueOffset = Vector2.zero;
                greenOffset = Vector2.zero;
            }

            if (chromaticAberration != 0)
            {
                redOffset = new Vector2(chromaticAberration / 10, chromaticAberration / 10);
                blueOffset = new Vector2(0, -(chromaticAberration / 10) * 1.4f);
                greenOffset = new Vector2(-chromaticAberration / 10, chromaticAberration / 10);
            }
        }

        #endregion

        public override void Create()
        {
            if (shaderMaterial == null)
                shaderMaterial = CoreUtils.CreateEngineMaterial(useShaderLite ? shaderLite : shader);

            if (crtRenderPass == null)
            {
                crtRenderPass = new CRTRenderPass(shaderMaterial);
                crtRenderPass.renderPassEvent = (RenderPassEvent)injectionPoint;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (shaderMaterial != null)
            {
                CoreUtils.Destroy(shaderMaterial);
                shaderMaterial = null;
            }

            if (crtRenderPass != null)
            {
                crtRenderPass.Dispose();
                crtRenderPass = null;
            }
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (shaderMaterial == null || crtRenderPass == null)
                return;

            CRTVolumeComponent volume = null;
            if (useVolumeComponent)
            {
                try
                {
                    volume = VolumeManager.instance.stack.GetComponent<CRTVolumeComponent>();
                }
                catch (Exception ex)
                {
                    Debug.LogError("CRT filter wasn't able to retrieve CRTVolumeComponent from VolumeManager. Please make sure that CRTVolumeComponent is defined or disable use of VolumeComponent on CRTFilter settings. Refer to following error for more details.");
                    Debug.LogException(ex);
                }
            }

            shaderMaterial.SetFloat(Shader.PropertyToID("p_resX"), pixelResolutionX);
            shaderMaterial.SetFloat(Shader.PropertyToID("p_resY"), pixelResolutionY);
            if (volume != null)
            {
                shaderMaterial.SetFloat(Shader.PropertyToID("p_screenBend"), volume.screenBend.value == 0 ? 1000 : 13 - volume.screenBend.value);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_screenOverscan"), volume.screenOverscan.value * 0.025f);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_blur"), volume.blur.value / 1000);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_smidge"), volume.smidge.value / 50);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_bleedr"), volume.bleed.value);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_bleedg"), volume.bleed.value > 0 ? 1 : 0);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_bleedb"), volume.bleed.value > 0 ? 1 : 0);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_scanlinesStrength"), volume.scanlinesStrength.value / 10);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_apertureStrength"), volume.apertureStrength.value / 10);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_shadowlines"), volume.shadowlines.value);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_shadowlinesSpeed"), volume.shadowlinesSpeed.value);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_shadowlinesAlpha"), volume.shadowlinesAlpha.value * 0.2f);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_vignetteSize"), volume.vignetteSize.value * 0.35f);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_vignetteSmooth"), volume.vignetteSmooth.value * 0.1f);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_vignetteRound"), 102f - volume.vignetteRound.value);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_noiseSize"), volume.noiseSize.value * 20);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_noiseAlpha"), volume.noiseAlpha.value * 0.2f);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_noiseSpeed"), volume.noiseSpeed.value * 0.0001f);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_brightness"), volume.brightness.value - 1);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_contrast"), volume.contrast.value);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_gamma"), volume.gamma.value);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_red"), volume.red.value);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_green"), volume.green.value);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_blue"), volume.blue.value);
                shaderMaterial.SetVector(Shader.PropertyToID("p_redOffset"), volume.redOffset.value / 100);
                shaderMaterial.SetVector(Shader.PropertyToID("p_greenOffset"), volume.greenOffset.value / 100);
                shaderMaterial.SetVector(Shader.PropertyToID("p_blueOffset"), volume.blueOffset.value / 100);
            }
            else
            {
                shaderMaterial.SetFloat(Shader.PropertyToID("p_screenBend"), screenBend == 0 ? 1000 : 13 - screenBend);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_screenOverscan"), screenOverscan * 0.025f);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_blur"), blur / 1000);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_smidge"), smidge / 50);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_bleedr"), bleed);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_bleedg"), bleed > 0 ? 1 : 0);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_bleedb"), bleed > 0 ? 1 : 0);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_scanlinesStrength"), scanlinesStrength / 10);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_apertureStrength"), apertureStrength / 10);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_shadowlines"), shadowlines);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_shadowlinesSpeed"), shadowlinesSpeed);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_shadowlinesAlpha"), shadowlinesAlpha * 0.2f);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_vignetteSize"), vignetteSize * 0.35f);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_vignetteSmooth"), vignetteSmooth * 0.1f);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_vignetteRound"), 102f - vignetteRound);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_noiseSize"), noiseSize * 20);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_noiseAlpha"), noiseAlpha * 0.2f);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_noiseSpeed"), noiseSpeed * 0.0001f);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_brightness"), brightness - 1);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_contrast"), contrast);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_gamma"), gamma);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_red"), red);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_green"), green);
                shaderMaterial.SetFloat(Shader.PropertyToID("p_blue"), blue);
                shaderMaterial.SetVector(Shader.PropertyToID("p_redOffset"), redOffset / 100);
                shaderMaterial.SetVector(Shader.PropertyToID("p_greenOffset"), greenOffset / 100);
                shaderMaterial.SetVector(Shader.PropertyToID("p_blueOffset"), blueOffset / 100);
            }

            crtRenderPass.ConfigureInput(ScriptableRenderPassInput.Color);
            renderer.EnqueuePass(crtRenderPass);
        }

        class CRTRenderPass : ScriptableRenderPass
        {
            private const string TEXNAME = "CRTFilterTexture";
            private const string PASSNAME = "CRTFilterPass";

            private Material shaderMaterial;

            public CRTRenderPass(Material material)
            {
                this.shaderMaterial = material;
            }

            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
            {
                if (shaderMaterial == null)
                    return;

                shaderMaterial.SetFloat(Shader.PropertyToID("p_time"), Time.time);

                UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
                UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();

                // don't blit from the back buffer.
                if (resourceData.isActiveTargetBackBuffer)
                    return;

                // Set the CRT texture size to be the same as the camera target size.
                RenderTextureDescriptor crtTextureDescriptor = cameraData.cameraTargetDescriptor;
                crtTextureDescriptor.msaaSamples = 1;
                crtTextureDescriptor.depthBufferBits = 0;

                TextureHandle src = resourceData.activeColorTexture;
                TextureHandle dst = UniversalRenderer.CreateRenderGraphTexture(renderGraph, crtTextureDescriptor, TEXNAME, false);


                // This check is to avoid an error from the material preview in the scene
                if (!src.IsValid() || !dst.IsValid())
                    return;

                RenderGraphUtils.BlitMaterialParameters blitParams = new(src, dst, shaderMaterial, 0);
                renderGraph.AddBlitPass(blitParams, PASSNAME);
                renderGraph.AddBlitPass(dst, src, Vector2.one, Vector2.zero);
            }

            public void Dispose()
            {
            }

            //If used in Unity 2022.x.x or if Compatibility Mode in Unity 6 is disabled (no Render Graph), comment out existing
            //methods above - "RecordRenderGraph" and "Dispose" and uncomment following section. Also replace shaders in the
            //renderer configuration with UnityCG variants.

            /*private RTHandle crtTexture;

            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
            {
                var crtTextureDescriptor = cameraTextureDescriptor;
                crtTextureDescriptor.msaaSamples = 1;
                crtTextureDescriptor.depthBufferBits = 0;

                RenderingUtils.ReAllocateHandleIfNeeded(ref crtTexture, crtTextureDescriptor, name: TEXNAME, wrapMode: TextureWrapMode.Clamp);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (shaderMaterial == null || crtTexture == null)
                    return;

                var cmd = CommandBufferPool.Get(PASSNAME);

                shaderMaterial.SetFloat("p_time", Time.time);
                cmd.Blit(renderingData.cameraData.renderer.cameraColorTargetHandle, crtTexture, shaderMaterial, 0);
                cmd.Blit(crtTexture, renderingData.cameraData.renderer.cameraColorTargetHandle);
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                CommandBufferPool.Release(cmd);
            }

            public void Dispose()
            {
                RTHandles.Release(crtTexture);
                crtTexture = null;
            }*/
        }
    }
}