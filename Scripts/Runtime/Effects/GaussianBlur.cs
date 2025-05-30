using UnityEngine;
using UnityEngine.Rendering;

namespace Nexcide.PostProcessing {

    [VolumeComponentMenu("Nexcide/Gaussian Blur")]
    public class GaussianBlur : VolumeComponentBase {

        public FloatParameter Spread = new(0.0f);
        public NoInterpIntParameter GridSize = new(20);

        public override bool IsActive() => (Spread.value > 0.0f);
    }

    [PostProcessEffect(typeof(GaussianBlur))]
    public class GaussianBlurEffect : VolumeEffect {

        public override string ShaderName => "Nexcide/Gaussian Blur";

        public override int Passes => 2;

        private static readonly int _spread = Shader.PropertyToID("_Spread");
        private static readonly int _gridSize = Shader.PropertyToID("_GridSize");

        public override bool ConfigureMaterial(VolumeStack volumeStack, MaterialPropertyBlock material) {
            bool active = ComponentActive(volumeStack, out GaussianBlur component);

            if (active) {
                material.SetFloat(_spread, Mathf.Max(component.Spread.value, 0.01f));
                material.SetInteger(_gridSize, Mathf.Max(component.GridSize.value, 1));
            }

            return active;
        }
    }
}
