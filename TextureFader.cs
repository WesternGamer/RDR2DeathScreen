using Sandbox.Game.Gui;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRageMath;
using VRageRender;

namespace RDR2DeathScreen
{
    internal class TextureFader
    {
		private float m_blackScreenCurrent = 1f;

		private float m_blackScreenStart;

		private float m_blackScreenTimeIncrement;

		private float m_blackScreenTimeTimer;

		private float m_blackScreenTarget = 1f;

		public bool BlackScreenMinimalizeHUD = false;

		public Color BlackScreenColor = Color.Black;

		public float BlackScreenCurrent => m_blackScreenCurrent;

		public void Update()
		{
			UpdateBlackScreen();
		}

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
				m_blackScreenStart = targetAlpha;
				m_blackScreenTarget = m_blackScreenCurrent;
				m_blackScreenTimeTimer = 0f;
				m_blackScreenTimeIncrement = -0.0166666675f / time;
			}
		}

		private void UpdateBlackScreen()
		{
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
				Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
				RectangleF rectangleF = new RectangleF(0f, 0f, fullscreenRectangle.Width, fullscreenRectangle.Height);
				MyRenderProxy.DrawSprite(@"C:\Users\Kevng\Downloads\Overlay.dds", ref rectangleF, null, new Vector4(1, 1, 1, m_blackScreenCurrent), 0f, true, false, null, null, 0f);
			}
		}
	}
}
