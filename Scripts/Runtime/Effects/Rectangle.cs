﻿using UnityEngine;
using UnityEngine.Rendering;

namespace Nexcide.PostProcessing {

    [VolumeComponentMenu("Nexcide/Rectangle")]
    public class Rectangle : VolumeComponentBase {

        public ClampedFloatParameter Opacity = new(0.0f, 0.0f, 1.0f);
        public NoInterpFloatParameter AspectRatio = new(1.77777f);
        public NoInterpClampedFloatParameter Width = new(0.5f, 0.0f, 1.0f);
        public NoInterpClampedFloatParameter Height = new(0.5f, 0.0f, 1.0f);
        public NoInterpClampedFloatParameter EdgeRadius = new(0.1f, 0.0f, 1.0f);

        public override bool IsActive() => (Opacity.value > 0.0f);
    }

    [PostProcessEffect(typeof(Rectangle))]
    public class RectangleEffect : VolumeEffect {

        public override string ShaderName => "Nexcide/Rectangle";

        private static readonly int _opacity = Shader.PropertyToID("_Opacity");
        private static readonly int _aspectRatio = Shader.PropertyToID("_AspectRatio");
        private static readonly int _width = Shader.PropertyToID("_Width");
        private static readonly int _height = Shader.PropertyToID("_Height");
        private static readonly int _edgeRadius = Shader.PropertyToID("_EdgeRadius");

        public override bool ConfigureMaterial(VolumeStack volumeStack, MaterialPropertyBlock material) {
            bool active = ComponentActive(volumeStack, out Rectangle component);

            if (active) {
                material.SetFloat(_opacity, component.Opacity.value);
                material.SetFloat(_aspectRatio, component.AspectRatio.value);
                material.SetFloat(_width, component.Width.value);
                material.SetFloat(_height, component.Height.value);
                material.SetFloat(_edgeRadius, component.EdgeRadius.value);
            }

            return active;
        }
    }
}
