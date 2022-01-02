using Sandbox.Engine.Platform.VideoMode;
using VRageRender;

namespace RDR2DeathScreen
{
    internal static class PostProcessing
    {
        private static bool OnDeath;

        private static MyGraphicsSettings settings = default;
        
        private struct PostProcessingSettings
        {
            public bool Enabled;
            public float Saturation;
            public float BloomMult;
            public int BloomSize;
            public bool BloomEnabled;
            public string DirtTexture;
            public float BloomDirtRatio;
            public float VignetteStart;
            public float VignetteLength;
        }

        private static PostProcessingSettings Default = new PostProcessingSettings
        {
            Enabled = settings.PostProcessingEnabled,
            Saturation = 1f,
            BloomMult = 0.28f,
            BloomSize = 6,
            BloomEnabled = false,
            DirtTexture = "",
            BloomDirtRatio = 0.5f,
            VignetteStart = 2f,
            VignetteLength = 2f
        };

        private static PostProcessingSettings Death = new PostProcessingSettings
        {
            Enabled = true,
            Saturation = 0f,
            BloomMult = 0.5f,
            BloomSize = 1,
            BloomEnabled = true,
            DirtTexture = "",
            BloomDirtRatio = 0.6f,
            VignetteStart = 3f,
            VignetteLength = 0.5f
        };

        public static void ChangeToDeathSettings()
        {
            MyPostprocessSettingsWrapper.AllEnabled = Death.Enabled;

            MyPostprocessSettingsWrapper.Settings.Data.Saturation = Death.Saturation;
            MyPostprocessSettingsWrapper.Settings.Data.BloomMult = Death.BloomMult;
            MyPostprocessSettingsWrapper.Settings.BloomSize = Death.BloomSize;
            MyPostprocessSettingsWrapper.Settings.BloomEnabled = Death.BloomEnabled;
            MyPostprocessSettingsWrapper.Settings.DirtTexture = Death.DirtTexture;
            MyPostprocessSettingsWrapper.Settings.Data.BloomDirtRatio = Death.BloomDirtRatio;
            MyPostprocessSettingsWrapper.Settings.Data.VignetteStart = Death.VignetteStart;
            MyPostprocessSettingsWrapper.Settings.Data.VignetteLength = Death.VignetteLength;

            OnDeath = true;
        }

        public static void ChangeToDefaultSettings()
        {
            MyPostprocessSettingsWrapper.AllEnabled = Default.Enabled;

            MyPostprocessSettingsWrapper.Settings.Data.Saturation = Default.Saturation;
            MyPostprocessSettingsWrapper.Settings.Data.BloomMult = Default.BloomMult;
            MyPostprocessSettingsWrapper.Settings.BloomSize = Default.BloomSize;
            MyPostprocessSettingsWrapper.Settings.BloomEnabled = Default.BloomEnabled;
            MyPostprocessSettingsWrapper.Settings.DirtTexture = Default.DirtTexture;
            MyPostprocessSettingsWrapper.Settings.Data.BloomDirtRatio = Default.BloomDirtRatio;
            MyPostprocessSettingsWrapper.Settings.Data.VignetteStart = Default.VignetteStart;
            MyPostprocessSettingsWrapper.Settings.Data.VignetteLength = Default.VignetteLength;

            OnDeath = false;
        }

        public static void Update()
        {
            if (OnDeath)
            {
                VerifyDeathSettings();
            }
        }

        private static void VerifyDeathSettings()
        {
            if (MyPostprocessSettingsWrapper.AllEnabled.Equals(Death.Enabled))
            {
                MyPostprocessSettingsWrapper.AllEnabled = Death.Enabled;
            }

            if (MyPostprocessSettingsWrapper.Settings.Data.Saturation.Equals(Death.Saturation))
            {
                MyPostprocessSettingsWrapper.Settings.Data.Saturation = Death.Saturation;
            }

            if (MyPostprocessSettingsWrapper.Settings.Data.BloomMult.Equals(Death.BloomMult))
            {
                MyPostprocessSettingsWrapper.Settings.Data.BloomMult = Death.BloomMult;
            }

            if (MyPostprocessSettingsWrapper.Settings.BloomSize.Equals(Death.BloomSize))
            {
                MyPostprocessSettingsWrapper.Settings.BloomSize = Death.BloomSize;
            }

            if (MyPostprocessSettingsWrapper.Settings.BloomEnabled.Equals(Death.BloomEnabled))
            {
                MyPostprocessSettingsWrapper.Settings.BloomEnabled = Death.BloomEnabled;
            }

            if (MyPostprocessSettingsWrapper.Settings.DirtTexture.Equals(Death.DirtTexture))
            {
                MyPostprocessSettingsWrapper.Settings.DirtTexture = Death.DirtTexture;
            }

            if (MyPostprocessSettingsWrapper.Settings.Data.BloomDirtRatio.Equals(Death.BloomDirtRatio))
            {
                MyPostprocessSettingsWrapper.Settings.Data.BloomDirtRatio = Death.BloomDirtRatio;
            }

            if (MyPostprocessSettingsWrapper.Settings.Data.VignetteStart.Equals(Death.VignetteStart))
            {
                MyPostprocessSettingsWrapper.Settings.Data.VignetteStart = Death.VignetteStart;
            }

            if (MyPostprocessSettingsWrapper.Settings.Data.VignetteLength.Equals(Death.VignetteLength))
            {
                MyPostprocessSettingsWrapper.Settings.Data.VignetteLength = Death.VignetteLength;
            }
        }

        private static void VerifyDefaultSettings()
        {
            if (MyPostprocessSettingsWrapper.AllEnabled.Equals(Default.Enabled))
            {
                MyPostprocessSettingsWrapper.AllEnabled = Default.Enabled;
            }

            if (MyPostprocessSettingsWrapper.Settings.Data.Saturation.Equals(Default.Saturation))
            {
                MyPostprocessSettingsWrapper.Settings.Data.Saturation = Default.Saturation;
            }

            if (MyPostprocessSettingsWrapper.Settings.Data.BloomMult.Equals(Default.BloomMult))
            {
                MyPostprocessSettingsWrapper.Settings.Data.BloomMult = Default.BloomMult;
            }

            if (MyPostprocessSettingsWrapper.Settings.BloomSize.Equals(Default.BloomSize))
            {
                MyPostprocessSettingsWrapper.Settings.BloomSize = Default.BloomSize;
            }

            if (MyPostprocessSettingsWrapper.Settings.BloomEnabled.Equals(Default.BloomEnabled))
            {
                MyPostprocessSettingsWrapper.Settings.BloomEnabled = Default.BloomEnabled;
            }

            if (MyPostprocessSettingsWrapper.Settings.DirtTexture.Equals(Default.DirtTexture))
            {
                MyPostprocessSettingsWrapper.Settings.DirtTexture = Default.DirtTexture;
            }

            if (MyPostprocessSettingsWrapper.Settings.Data.BloomDirtRatio.Equals(Default.BloomDirtRatio))
            {
                MyPostprocessSettingsWrapper.Settings.Data.BloomDirtRatio = Default.BloomDirtRatio;
            }

            if (MyPostprocessSettingsWrapper.Settings.Data.VignetteStart.Equals(Default.VignetteStart))
            {
                MyPostprocessSettingsWrapper.Settings.Data.VignetteStart = Default.VignetteStart;
            }

            if (MyPostprocessSettingsWrapper.Settings.Data.VignetteLength.Equals(Default.VignetteLength))
            {
                MyPostprocessSettingsWrapper.Settings.Data.VignetteLength = Default.VignetteLength;
            }
        }
    }
}
