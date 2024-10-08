﻿using HarmonyLib;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using VRage.FileSystem;
using VRage.Plugins;

namespace RDR2DeathScreen
{
    public class Main : IPlugin
    {
        private ScreenFader ScreenFader;

        private DeathScreen Video;

        public Audio Audio;

        private bool IsPlayerAlreadyDead;

        private bool IsPlayerAlreadyRespawned = true;

        public void Dispose()
        {
            if (Video != null)
            {
                Video.CloseScreenNow(true);
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public void Init(object gameInstance)
        {
            Harmony harmony = new Harmony("RDR2DeathScreen");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            if (!VerfiyFiles())
                //ExtractFiles();
                DownloadFiles();

            if (!VerifyXML())
            {
                WriteToXML();
                RestartGame();
            }
                

            ScreenFader = new ScreenFader();

            Video = new DeathScreen();

            Audio = new Audio();
        }

        public void Update()
        {
            if (MySession.Static != null)
            {
                ScreenFader.Update();
                Audio.Update();
                PostProcessing.Update();

                if (MySession.Static.LocalCharacter.IsDead)
                {
                    DuringDeath();
                }
                else
                {
                    DuringNormal();
                }
            }
        }

        private void DuringDeath()
        {
            IsPlayerAlreadyRespawned = false;
            MyGuiScreenGamePlay.DisableInput = true;

            if (!IsPlayerAlreadyDead)
            {
                if (MySession.Static.LocalCharacter.IsInFirstPersonView == true)
                {
                    MySession.Static.LocalCharacter.IsInFirstPersonView = false;
                }

                Audio.Muted = true;
                
                MyScreenManager.CloseAllScreensExcept(MyGuiScreenGamePlay.Static);
                PostProcessing.ChangeToDeathSettings();
                StartFadeScreen();

                if (!Sync.MultiplayerActive)
                {
                    MyFakes.SIMULATION_SPEED = 0.05f;
                    Audio.PlayDeathSoundSP();
                    ShowDeadScreen();
                }
                else
                {
                    Audio.PlayDeathSoundMP();
                }
                IsPlayerAlreadyDead = true;
            }
        }

        private void DuringNormal()
        {
            IsPlayerAlreadyDead = false;

            MyGuiScreenGamePlay.DisableInput = false;
            if (!IsPlayerAlreadyRespawned)
            {
                MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.HUDScreen));
                IsPlayerAlreadyRespawned = true;
                MyFakes.SIMULATION_SPEED = 1f;
                
                ScreenFader.FadeScreen(1f);
                Audio.Muted = false;
                PostProcessing.ChangeToDefaultSettings();
            }
        }

        public void StartFadeScreen()
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

        public void ShowDeadScreen()
        {
            RunTaskWithDelay(4500, delegate
            {
                MyGuiSandbox.AddScreen(Video);
            });
        }

        private void ExtractFiles()
        {
            try
            {
                Stream stream1 = Assembly.GetExecutingAssembly().GetManifestResourceStream("RDR2DeathScreen.EmbeddedFiles.HudDeathSoundSP.xwm");
                FileStream fileStream1 = new FileStream(Path.Combine(MyFileSystem.ContentPath, @"Audio\ARC\HUD\HudDeathSoundSP.xwm"), FileMode.CreateNew);
                for (int i = 0; i < stream1.Length; i++)
                    fileStream1.WriteByte((byte)stream1.ReadByte());
                fileStream1.Close();
            }
            catch 
            {

            }

            try
            {
                Stream stream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("RDR2DeathScreen.EmbeddedFiles.HudDeathSoundMP.xwm");
                FileStream fileStream2 = new FileStream(Path.Combine(MyFileSystem.ContentPath, @"Audio\ARC\HUD\HudDeathSoundMP.xwm"), FileMode.CreateNew);
                for (int i = 0; i < stream2.Length; i++)
                    fileStream2.WriteByte((byte)stream2.ReadByte());
                fileStream2.Close();
            }
            catch
            {

            }

            try
            {
                Stream stream3 = Assembly.GetExecutingAssembly().GetManifestResourceStream("RDR2DeathScreen.EmbeddedFiles.RDR2DeathScreenVideo.wmv");
                FileStream fileStream3 = new FileStream(Path.Combine(MyFileSystem.ContentPath, @"Videos\RDR2DeathScreenVideo.wmv"), FileMode.CreateNew);
                for (int i = 0; i < stream3.Length; i++)
                    fileStream3.WriteByte((byte)stream3.ReadByte());
                fileStream3.Close();
            }
            catch
            {

            }            
        }

        private void DownloadFiles()
        {
            WebClient webClient = new WebClient();

            try
            {
                webClient.DownloadFile(new Uri("https://raw.githubusercontent.com/WesternGamer/RDR2DeathScreen/master/RDR2DeathScreen/EmbeddedFiles/HudDeathSoundSP.xwm"),
                Path.Combine(MyFileSystem.ContentPath, @"Audio\ARC\HUD\HudDeathSoundSP.xwm"));
            }
            catch
            {

            }

            try
            {
                webClient.DownloadFile(new Uri("https://raw.githubusercontent.com/WesternGamer/RDR2DeathScreen/master/RDR2DeathScreen/EmbeddedFiles/HudDeathSoundMP.xwm"),
                Path.Combine(MyFileSystem.ContentPath, @"Audio\ARC\HUD\HudDeathSoundMP.xwm"));
            }
            catch
            {

            }

            try
            {
                webClient.DownloadFile(new Uri("https://raw.githubusercontent.com/WesternGamer/RDR2DeathScreen/master/RDR2DeathScreen/EmbeddedFiles/RDR2DeathScreenVideo.wmv"),
                Path.Combine(MyFileSystem.ContentPath, @"Videos\RDR2DeathScreenVideo.wmv"));
            }
            catch
            {

            }

            webClient.Dispose();
        }

        private bool VerfiyFiles()
        {
            if (!File.Exists(Path.Combine(MyFileSystem.ContentPath, @"Audio\ARC\HUD\HudDeathSoundSP.xwm")))
            {
                return false;
            }
            if (!File.Exists(Path.Combine(MyFileSystem.ContentPath, @"Audio\ARC\HUD\HudDeathSoundMP.xwm")))
            {
                return false;
            }
            if (!File.Exists(Path.Combine(MyFileSystem.ContentPath, @"Videos\RDR2DeathScreenVideo.wmv")))
            {
                return false;
            }
            return true;
        }

        private void WriteToXML()
        {
            var fileName = Path.Combine(MyFileSystem.ContentPath, @"Data\Audio_gui.sbc");
            var lineToAdd = @"  <Sound>
      <Id>
        <TypeId>AudioDefinition</TypeId>
        <SubtypeId>DeathSoundMP</SubtypeId>
      </Id>
      <Category>HUD</Category>
      <Volume>1.00</Volume>
      <Waves>
        <Wave Type='D2'>
          <Start>ARC\HUD\HudDeathSoundMP.xwm</Start>
        </Wave>
      </Waves>
    </Sound>
    <Sound>
      <Id>
        <TypeId>AudioDefinition</TypeId>
        <SubtypeId>DeathSoundSP</SubtypeId>
      </Id>
      <Category>HUD</Category>
      <Volume>1.00</Volume>
      <Waves>
        <Wave Type='D2'>
          <Start>ARC\HUD\HudDeathSoundSP.xwm</Start>
        </Wave>
      </Waves>
    </Sound>";

            var txtLines = File.ReadAllLines(fileName).ToList();
            txtLines.Insert(3, lineToAdd);
            File.WriteAllLines(fileName, txtLines);
        }

        private bool VerifyXML()
        {
            foreach (string line in File.ReadLines(Path.Combine(MyFileSystem.ContentPath, @"Data\Audio_gui.sbc")))
            {
                if (line.Contains(@"          <Start>ARC\HUD\HudDeathSoundMP.xwm</Start>"))
                {
                    return true;
                }
            }
            return false;
        }

        private void RestartGame()
        {
            DialogResult result = MessageBox.Show("Game needs to be restarted to properly load audio definitions. Click Yes to restart. Click No to load game. Game will still run without the death sound if you click No.",
               "RDR2 Death Screen: Restart Required.", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);

            if (result == DialogResult.Yes)
            {
                Application.Restart();
                Process.GetCurrentProcess().Kill();
            }
        }
    }
}
