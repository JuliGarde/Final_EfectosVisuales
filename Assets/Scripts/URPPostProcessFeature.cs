using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class URPPostProcessFeature : ScriptableRendererFeature
{
  
   
        public static URPPostProcessFeature Instance;

        public Material material1;
        public Material material2;

        public enum EffectType
        {
            None,
            Effect1,
            Effect2
        }

        public EffectType activeEffect = EffectType.Effect1;

        class Pass : ScriptableRenderPass
        {
            public Material material;
            RenderTargetIdentifier source;
            RenderTargetHandle temp;

            public void Setup(RenderTargetIdentifier src)
            {
                source = src;
            }

            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor desc)
            {
                temp.Init("_TempPostFX");
                cmd.GetTemporaryRT(temp.id, desc);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData data)
            {
                if (material == null) return;

                CommandBuffer cmd = CommandBufferPool.Get("PostFX");

                Blit(cmd, source, temp.Identifier(), material);
                Blit(cmd, temp.Identifier(), source);

                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }

            public override void FrameCleanup(CommandBuffer cmd)
            {
                cmd.ReleaseTemporaryRT(temp.id);
            }
        }

        Pass pass;

        public override void Create()
        {
            Instance = this;

            pass = new Pass();
            pass.renderPassEvent = RenderPassEvent.AfterRendering;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            Material selected = null;

            switch (activeEffect)
            {
                case EffectType.Effect1:
                    selected = material1;
                    break;
                case EffectType.Effect2:
                    selected = material2;
                    break;
            }

            pass.material = selected;
            pass.Setup(renderer.cameraColorTarget);
            renderer.EnqueuePass(pass);
        }
    }
