using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System.IO;
using System.Threading;
using VRage.FileSystem;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace RDR2DeathScreen
{
    // From Sandbox.Game.Gui.MyGuiScreenIntroVideo, modified to work for this plugin.
    internal class DeathVideo : MyGuiScreenBase
    {
        private uint m_videoID = uint.MaxValue;

        private bool m_playbackStarted;

        private readonly string m_currentVideo = Path.Combine(MyFileSystem.ContentPath, @"Videos\RDR2DeathScreenVideo.wmv");

        public DeathVideo()
            : base(Vector2.Zero)
        {
            MyRenderProxy.Settings.RenderThreadHighPriority = true;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            SkipTransition = true;
            CanBeHidden = false;
            CanHideOthers = true;
            DrawMouseCursor = false;
            CanHaveFocus = true;
            m_isTopMostScreen = true;
            m_closeOnEsc = false;
            m_drawEvenWithoutFocus = true;
            m_canCloseInCloseAllScreenCalls = false;
        }

        public override string GetFriendlyName()
        {
            return "DeathVideo";
        }

        public override void LoadContent()
        {
            m_playbackStarted = false;
            base.LoadContent();
        }

        public override void CloseScreenNow(bool isUnloading = false)
        {
            if (base.State != MyGuiScreenState.CLOSED)
            {
                UnloadContent();
            }
            MyRenderProxy.Settings.RenderThreadHighPriority = false;
            Thread.CurrentThread.Priority = ThreadPriority.Normal;
            base.CloseScreenNow(isUnloading);
        }

        private void CloseVideo()
        {
            if (m_videoID != uint.MaxValue)
            {
                MyRenderProxy.CloseVideo(m_videoID);
                m_videoID = uint.MaxValue;
            }
        }

        public override void UnloadContent()
        {
            CloseVideo();
            base.UnloadContent();
        }

        public override bool Update(bool hasFocus)
        {
            if (!base.Update(hasFocus))
            {
                return false;
            }
            if (!m_playbackStarted)
            {
                TryPlayVideo();
                m_playbackStarted = true;
            }
            else
            {
                if (MyRenderProxy.IsVideoValid(m_videoID) && MyRenderProxy.GetVideoState(m_videoID) != 0)
                {
                    CloseScreen();
                }
                if (base.State == MyGuiScreenState.CLOSING && MyRenderProxy.IsVideoValid(m_videoID))
                {
                    MyRenderProxy.SetVideoVolume(m_videoID, m_transitionAlpha);
                }
            }
            return true;
        }

        private void TryPlayVideo()
        {
            CloseVideo();
            if (File.Exists(m_currentVideo))
            {
                m_videoID = MyRenderProxy.PlayVideo(m_currentVideo, 1f);
            }
            else
            {
                throw new FileNotFoundException("[RDR2DeathScreen]: [ERROR]: Death screen video file not found!");
            }
        }

        public override bool CloseScreen(bool isUnloading = false)
        {
            bool num = base.CloseScreen(isUnloading);
            MyRenderProxy.Settings.RenderThreadHighPriority = false;
            Thread.CurrentThread.Priority = ThreadPriority.Normal;
            if (num)
            {
                CloseVideo();
            }
            return num;
        }

        public override bool Draw()
        {
            if (!base.Draw())
            {
                return false;
            }
            if (MyRenderProxy.IsVideoValid(m_videoID))
            {
                MyRenderProxy.UpdateVideo(m_videoID);
                Vector4 vector = Vector4.One * m_transitionAlpha;
                MyRenderProxy.DrawVideo(m_videoID, MyGuiManager.GetSafeFullscreenRectangle(), new Color(vector), MyVideoRectangleFitMode.AutoFit, ignoreBounds: true);
            }
            return true;
        }
    }
}
