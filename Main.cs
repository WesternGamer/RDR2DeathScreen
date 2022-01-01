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
using VRage.Game;
using VRage.Plugins;
using VRageMath;
using VRageRender;

namespace RDR2DeathScreen
{
    public class Main : IPlugin
    {
        ScreenFader ScreenFader;

        MySoundPair DeathSound;

        TextureFader TextureFader;

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

            //TextureFader = new TextureFader();
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
                //TextureFader.Update();
            }
        }

        private void ChangePostProcessSettings()    
        {
            MyPostprocessSettingsWrapper.AllEnabled = true;

            MyPostprocessSettingsWrapper.Settings.Data.Saturation = 0.0f;
            MyPostprocessSettingsWrapper.Settings.Data.BloomEmissiveness = 1f;
            MyPostprocessSettingsWrapper.Settings.Data.BloomExposure = 5.8f;
            MyPostprocessSettingsWrapper.Settings.Data.BloomLumaThreshold = 0.16f;
            MyPostprocessSettingsWrapper.Settings.Data.BloomMult = 0.5f;
            MyPostprocessSettingsWrapper.Settings.Data.BloomDepthStrength = 2f;
            MyPostprocessSettingsWrapper.Settings.Data.BloomDepthSlope = 0.3f;
            MyPostprocessSettingsWrapper.Settings.BloomSize = 1;
            MyPostprocessSettingsWrapper.Settings.BloomEnabled = true;
            MyPostprocessSettingsWrapper.Settings.DirtTexture = @"C:\Users\Kevng\Downloads\Overlays\photomode_filtervintage09.dds";
            MyPostprocessSettingsWrapper.Settings.Data.BloomDirtRatio = 0.6f;
            //MyPostprocessSettingsWrapper.Settings.Data.ChromaticFactor = 0.1f;
            MyPostprocessSettingsWrapper.Settings.Data.VignetteStart = 3f;
            MyPostprocessSettingsWrapper.Settings.Data.VignetteLength = 0.5f;

            MyAudio.Static.PauseGameSounds();
            MyAudio.Static.StopMusic();

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
                ShowDeadScreen();
            }


            IsPlayerAlreadyDead = true;
        }

        private void ChangePostProcessSettingsToDefault()
        {
            MyPostprocessSettingsWrapper.AllEnabled = true;

            MyPostprocessSettingsWrapper.Settings.Data.Saturation = 1f;
            MyPostprocessSettingsWrapper.Settings.Data.BloomEmissiveness = 1f;
            MyPostprocessSettingsWrapper.Settings.Data.BloomExposure = 5.8f;
            MyPostprocessSettingsWrapper.Settings.Data.BloomLumaThreshold = 0.16f;
            MyPostprocessSettingsWrapper.Settings.Data.BloomMult = 0.28f;
            MyPostprocessSettingsWrapper.Settings.Data.BloomDepthStrength = 2f;
            MyPostprocessSettingsWrapper.Settings.Data.BloomDepthSlope = 0.3f;
            MyPostprocessSettingsWrapper.Settings.BloomSize = 6;
            MyPostprocessSettingsWrapper.Settings.BloomEnabled = false;
            MyPostprocessSettingsWrapper.Settings.Data.BloomDirtRatio = 0.5f;
            //MyPostprocessSettingsWrapper.Settings.Data.ChromaticFactor = 0.1f;
            MyPostprocessSettingsWrapper.Settings.Data.VignetteStart = 2f;
            MyPostprocessSettingsWrapper.Settings.Data.VignetteLength = 2;

            MyAudio.Static.ResumeGameSounds();
            MyAudio.Static.PlayMusic();

            ScreenFader.FadeScreen(1f);
            //TextureFader.FadeScreen(1f);
            MyFakes.SIMULATION_SPEED = 1f;
            IsPlayerAlreadyDead = false;
        }

        private void StartFadeScreen()
        {
            RunTaskWithDelay(4000, delegate
            {
                ScreenFader.FadeScreen(0f, 0.65f);
                
            });
            //TextureFader.FadeScreen(0f, 1f);
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

        private void ShowDeadScreen()
        {
            MyGuiScreenIntroVideo video = new MyGuiScreenIntroVideo(new string[1] { @"C:\Users\Kevng\Downloads\RDR2DeathScreenVideo.wmv" }, false, false, true, 1f, false, 0);
            video.LoadContent();
        }
    }
}
