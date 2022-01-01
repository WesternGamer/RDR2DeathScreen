using HarmonyLib;
using RDR2DeathScreen.Patches;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Reflection;
using System.Threading.Tasks;
using VRage.Plugins;
using VRageRender;

namespace RDR2DeathScreen
{
    public class Main : IPlugin
    {
        private ScreenFader ScreenFader;

        private DeathVideo Video;

        public Audio Audio;

        public bool IsPlayerDead;

        public bool IsPlayerInFirstPerson;

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
                
            ScreenFader = new ScreenFader();

            Video = new DeathVideo();

            Audio = new Audio();
        }

        public void Update()
        {
            ScreenFader.Update();
            Audio.Update();

            if (MySession.Static != null)
            {
                IsPlayerDead = GetCharacterFromPlayerId(MySession.Static.LocalPlayerId).IsDead; //Even though it is the local player Id, it still works on servers, clientside.
                IsPlayerInFirstPerson = GetCharacterFromPlayerId(MySession.Static.LocalPlayerId).IsInFirstPersonView;

                if (GetCharacterFromPlayerId(MySession.Static.LocalPlayerId).IsDead)
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
            PostProcessing.ChangeToDeathSettings();

            Audio.Muted = true;
            StartFadeScreen();
            CloseAllScreens();
            if (!Sync.MultiplayerActive)
            {
                MyFakes.SIMULATION_SPEED = 0.1f;
                Audio.PlayDeathSoundSP();
                ShowDeadScreen();
            }
            else
            {
                Audio.PlayDeathSoundMP();
            }

            if (GetCharacterFromPlayerId(MySession.Static.LocalPlayerId).IsInFirstPersonView == true)
            {
                GetCharacterFromPlayerId(MySession.Static.LocalPlayerId).IsInFirstPersonView = false;
            }

            MyGuiScreenGamePlay.DisableInput = true;
        }

        private void DuringNormal()
        {
            PostProcessing.ChangeToDefaultSettings();

            Audio.Muted = false;

            ScreenFader.FadeScreen(1f);
            MyFakes.SIMULATION_SPEED = 1f;
            MyGuiScreenGamePlay.DisableInput = false;
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

        public static void CloseAllScreens()
        {
            foreach (MyGuiScreenBase screen in MyScreenManager.Screens)
            {
                if (screen != MyGuiScreenGamePlay.Static && screen != MyGuiScreenGamePlay.ActiveGameplayScreen)
                {
                    screen.CloseScreen();
                }
            }

            MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.HUDScreen));
        }

        private MyCharacter GetCharacterFromPlayerId(long playerId = 0L)
        {
            if (playerId != 0L)
            {
                return MySession.Static.Players.TryGetIdentity(playerId).Character;
            }
            return MySession.Static.LocalCharacter;
        }
    }
}
