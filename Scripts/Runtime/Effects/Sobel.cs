﻿using UnityEngine;
using UnityEngine.Rendering;

namespace Nexcide.PostProcessing {

    [VolumeComponentMenu("Nexcide/Sobel")]
    public class Sobel : VolumeComponentBase {

        public ClampedFloatParameter Blend = new(0.0f, 0.0f, 1.0f);
        public BoolParameter Clamp01 = new(false);

        public override bool IsActive() => (Blend.value > 0.0f);
    }

    [PostProcessEffect(typeof(Sobel))]
    public class SobelEffect : VolumeEffect {

        public override string ShaderName => "Nexcide/Sobel";

        private static readonly int _blend = Shader.PropertyToID("_Blend");
        private static readonly int _clamp01 = Shader.PropertyToID("_Clamp01");

        public override bool ConfigureMaterial(VolumeStack volumeStack, MaterialPropertyBlock material) {
            bool active = ComponentActive(volumeStack, out Sobel component);

            if (active) {
                material.SetFloat(_blend, component.Blend.value);
                material.SetFloat(_clamp01, component.Clamp01.value ? 1.0f : 0.0f);
            }

            return active;
        }
    }
}
