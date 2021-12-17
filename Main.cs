using HarmonyLib;
using RDR2DeathScreen.Patches;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using VRage.Audio;
using VRage.Plugins;
using VRageMath;
using VRageRender;

namespace RDR2DeathScreen
{
    public class Main : IPlugin
    {
        ScreenFader ScreenFader;

        MySoundPair DeathSound;

        //IMyAudio myAudio1 = new ();

        public bool IsPlayerAlreadyDead { get; private set; }

        public Main()
        {

        }

        public void Dispose()
        {

        }

        public void Init(object gameInstance)
        {
            Harmony harmony = new Harmony("RDR2DeathScreen");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            ScreenFader = new ScreenFader();

            DeathSound = new MySoundPair("HudDeathAudioSound", true);
        }

        public void Update()
        {
            if (MySession.Static != null)
            {             
                if (MyVisualScriptLogicProvider.IsPlayerDead(MySession.Static.LocalPlayerId)) //Even though it is the local player Id, it still works on servers, clientside.
                {
                    ChangePostProcessSettings();
                }
                else
                {
                    ChangePostProcessSettingsToDefault();
                }

                ScreenFader.Update();
            }
        }

        private void ChangePostProcessSettings()
        {
            MyPostprocessSettingsWrapper.AllEnabled = true;
            MyPostprocessSettingsWrapper.Settings.Data.Saturation = 0f;
            MyPostprocessSettingsWrapper.Settings.Data.VignetteLength = 0.5f;
            MyPostprocessSettingsWrapper.Settings.Data.VignetteStart = 2.5f;
            //MyPostprocessSettingsWrapper.Settings.Data.LuminanceExposure = 25f;
            MyPostprocessSettingsWrapper.Settings.Data.BrightnessFactorR = 3.5f;
            MyPostprocessSettingsWrapper.Settings.Data.BrightnessFactorG = 3.5f;
            MyPostprocessSettingsWrapper.Settings.Data.BrightnessFactorB = 3.5f;
            MyPostprocessSettingsWrapper.Settings.Data.GrainStrength = 0.75f;
            MyPostprocessSettingsWrapper.Settings.Data.GrainAmount = 0.1f;

            MyAudio.Static.Mute = true;
            MyAudio.Static.StopMusic();

            Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
            RectangleF rectangleF = new RectangleF(0f, 0f, fullscreenRectangle.Width, fullscreenRectangle.Height);
            MyRenderProxy.DrawSprite(@"C:\Users\Kevng\Downloads\texturefabrik_dust-03.png", ref rectangleF, null, new VRageMath.Color(255, 255, 255, 10), 0f, true, true, null, null, 0f);

            
            if (ExamplePatch.Instance != null)
            {
                if (ExamplePatch.Instance.IsInFirstPersonView == true)
                    ExamplePatch.Instance.IsInFirstPersonView = false;
            }
            

            if (!IsPlayerAlreadyDead)
            {
                StartFadeScreen();
                PlayDeathSound();
            }

            if (!Sync.MultiplayerActive)
            {
                MyFakes.SIMULATION_SPEED = 0.1f;
            }


            IsPlayerAlreadyDead = true;
        }

        private void ChangePostProcessSettingsToDefault()
        {
            MyPostprocessSettingsWrapper.AllEnabled = true;
            MyPostprocessSettingsWrapper.Settings.Data.Saturation = 1f;
            MyPostprocessSettingsWrapper.Settings.Data.VignetteLength = 2f;
            MyPostprocessSettingsWrapper.Settings.Data.VignetteStart = 2f;
            //MyPostprocessSettingsWrapper.Settings.Data.LuminanceExposure = 1f;
            MyPostprocessSettingsWrapper.Settings.Data.BrightnessFactorR = 1f;
            MyPostprocessSettingsWrapper.Settings.Data.BrightnessFactorG = 1f;
            MyPostprocessSettingsWrapper.Settings.Data.BrightnessFactorB = 1f;
            MyPostprocessSettingsWrapper.Settings.Data.GrainStrength = 0f;
            MyPostprocessSettingsWrapper.Settings.Data.GrainAmount = 0.1f;

            MyAudio.Static.Mute = false;
            MyAudio.Static.PlayMusic();

            ScreenFader.FadeScreen(1f);
            MyFakes.SIMULATION_SPEED = 1f;
            IsPlayerAlreadyDead = false;
        }

        private void StartFadeScreen()
        {
            RunTaskWithDelay(4000, delegate
            {
                ScreenFader.FadeScreen(0f, 0.65f);
            });            
        }

        private async void RunTaskWithDelay(int secondsMs, Action afterDelay)
        {
            await Task.Run(async delegate
            {
                await Task.Delay(secondsMs);
                afterDelay();
            });
        }

        private IMySourceVoice PlayDeathSound()
        {
            return MyAudio.Static.PlaySound(DeathSound.SoundId);
        }
    }
}
