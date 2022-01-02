using VRageMath;
using VRageRender;

namespace RDR2DeathScreen
{
    //From Sandbox.Game.Gui.MyHudScreenEffects, modified to work with this plugin.
    internal class ScreenFader
    {
        private float m_blackScreenCurrent = 1f;

        private float m_blackScreenStart;

        private float m_blackScreenTimeIncrement;

        private float m_blackScreenTimeTimer;

        private float m_blackScreenTarget = 1f;

        private bool m_blackScreenDataSaved;

        private Color m_blackScreenDataSavedLightColor = Color.White;

        private Color m_blackScreenDataSavedDarkColor = Color.Black;

        private float m_blackScreenDataSavedStrength;

        public void FadeScreen(float targetAlpha, float time = 0f)
        {
            targetAlpha = MathHelper.Clamp(targetAlpha, 0f, 1f);
            if (time <= 0f)
            {
                m_blackScreenTarget = targetAlpha;
                m_blackScreenCurrent = targetAlpha;
            }
            else
            {
                m_blackScreenTarget = targetAlpha;
                m_blackScreenStart = m_blackScreenCurrent;
                m_blackScreenTimeTimer = 0f;
                m_blackScreenTimeIncrement = 0.0166666675f / time;
            }
            if (targetAlpha < 1f && !m_blackScreenDataSaved)
            {
                m_blackScreenDataSaved = true;
                m_blackScreenDataSavedLightColor = MyPostprocessSettingsWrapper.Settings.Data.LightColor;
                m_blackScreenDataSavedDarkColor = MyPostprocessSettingsWrapper.Settings.Data.DarkColor;
                m_blackScreenDataSavedStrength = MyPostprocessSettingsWrapper.Settings.Data.SepiaStrength;
            }
        }

        public void Update()
        {
            MyPostprocessSettingsWrapper.Settings.Data.SepiaStrength = 0f;
            if (m_blackScreenTimeTimer < 1f && m_blackScreenCurrent != m_blackScreenTarget)
            {
                m_blackScreenTimeTimer += m_blackScreenTimeIncrement;
                if (m_blackScreenTimeTimer > 1f)
                {
                    m_blackScreenTimeTimer = 1f;
                }
                m_blackScreenCurrent = MathHelper.Lerp(m_blackScreenStart, m_blackScreenTarget, m_blackScreenTimeTimer);
            }
            if (m_blackScreenCurrent < 1f)
            {
                MyPostprocessSettingsWrapper.Settings.Data.LightColor = Color.Black;
                MyPostprocessSettingsWrapper.Settings.Data.DarkColor = Color.Black;
                MyPostprocessSettingsWrapper.Settings.Data.SepiaStrength = 1f - m_blackScreenCurrent;
            }
            else
            {
                if (m_blackScreenDataSaved)
                {
                    m_blackScreenDataSaved = false;
                    MyPostprocessSettingsWrapper.Settings.Data.LightColor = m_blackScreenDataSavedLightColor;
                    MyPostprocessSettingsWrapper.Settings.Data.DarkColor = m_blackScreenDataSavedDarkColor;
                    MyPostprocessSettingsWrapper.Settings.Data.SepiaStrength = m_blackScreenDataSavedStrength;
                }
            }
        }
    }
}
