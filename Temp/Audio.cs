using Sandbox.Game.Entities;
using Sandbox.Game.World;
using VRage.Audio;

namespace RDR2DeathScreen
{
    public class Audio
    {
        private readonly MySoundPair DeathSoundSP;

        private readonly MySoundPair DeathSoundMP;

        public bool Muted = false;

        public Audio()
        {
            DeathSoundSP = new MySoundPair("HudDeathAudioSound", true);

            DeathSoundMP = new MySoundPair("HudMpDeathFinal", true);
        }

        public void Update()
        {
            if (MySession.Static != null) //Removing this check would cause Space Engineers to not load at all.
            {
                if (Muted)
                {
                    MyAudio.Static.PauseGameSounds();
                    MyAudio.Static.StopMusic();
                }
                else
                {
                    MyAudio.Static.ResumeGameSounds();
                    MyAudio.Static.PlayMusic();
                }
            }            
        }

        public IMySourceVoice PlayDeathSoundSP()
        {
            return MyAudio.Static.PlaySound(DeathSoundSP.SoundId);
        }

        public IMySourceVoice PlayDeathSoundMP()
        {
            return MyAudio.Static.PlaySound(DeathSoundMP.SoundId);
        }
    }
}
