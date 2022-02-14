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
            public float VignetteStart;
            public float VignetteLength;
        }

        private static PostProcessingSettings Default = new PostProcessingSettings
        {
            Enabled = settings.PostProcessingEnabled,
            Saturation = 1f,
            VignetteStart = 2f,
            VignetteLength = 2f
        };

        private static PostProcessingSettings Death = new PostProcessingSettings
        {
            Enabled = true,
            Saturation = 0f,
            VignetteStart = 3f,
            VignetteLength = 0.5f
        };

        public static void ChangeToDeathSettings()
        {
            MyPostprocessSettingsWrapper.AllEnabled = Death.Enabled;

            MyPostprocessSettingsWrapper.Settings.Data.Saturation = Death.Saturation;
            MyPostprocessSettingsWrapper.Settings.Data.VignetteStart = Death.VignetteStart;
            MyPostprocessSettingsWrapper.Settings.Data.VignetteLength = Death.VignetteLength;

            OnDeath = true;
        }

        public static void ChangeToDefaultSettings()
        {
            MyPostprocessSettingsWrapper.AllEnabled = Default.Enabled;

            MyPostprocessSettingsWrapper.Settings.Data.Saturation = Default.Saturation;
            MyPostprocessSettingsWrapper.Settings.Data.VignetteStart = Default.VignetteStart;
            MyPostprocessSettingsWrapper.Settings.Data.VignetteLength = Default.VignetteLength;

            OnDeath = false;
        }

        public static void Update()
        {
            if (OnDeath)
            {
                VerifyDeathSettings();
                return;
            }
            VerifyDefaultSettings();
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
