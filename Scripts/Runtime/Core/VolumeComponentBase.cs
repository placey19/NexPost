using UnityEngine;
using UnityEngine.Rendering;

namespace Nexcide.PostProcessing {

    public abstract class VolumeComponentBase : VolumeComponent, IPostProcessComponent {

        public abstract bool IsActive();

        public bool IsTileCompatible() => false;
    }

    public abstract class VolumeEffect {

        public abstract string ShaderName { get; }

        public virtual int Passes { get => 1; }

        public abstract bool ConfigureMaterial(VolumeStack volumeStack, MaterialPropertyBlock material);

        protected bool ComponentActive<T>(VolumeStack volumeStack, out T component) where T : VolumeComponentBase {
            component = volumeStack.GetComponent<T>();
            return component.IsActive();
        }
    }
}
