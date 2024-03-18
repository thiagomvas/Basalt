using Basalt.Core.Common.Abstractions;
using Basalt.Core.Common.Abstractions.Sound;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Sound
{
    public class RaylibSoundSystem : ISoundSystem
    {
        private readonly ILogger? logger;
        private Dictionary<string, Raylib_cs.Sound> loadedSounds;
        private Music backgroundMusic;

        private List<string> queuedSounds = new();



        public void Initialize()
        {
            InitAudioDevice();
            foreach (var sound in queuedSounds)
            {
                LoadAudio(sound, AudioType.SoundEffect);
            }
        }
        public void Shutdown()
        {
            logger?.LogWarning("Shutting down sound system.");
            CloseAudioDevice();
            foreach (var sound in loadedSounds.Values)
            {
                UnloadSound(sound);
            }
            UnloadMusicStream(backgroundMusic);
            logger?.LogInformation("Unloaded all sounds and music.");
        }


        public RaylibSoundSystem(ILogger? logger = null)
        {
            this.logger = logger;
            loadedSounds = new Dictionary<string, Raylib_cs.Sound>();
        }

        public void LoadAudio(string filename, AudioType type)
        {
            if (!Engine.Instance.HasStarted)
            {
                queuedSounds.Add(filename);
                return;
            }


            switch (type)
            {
                case AudioType.Music:
                    backgroundMusic = LoadMusicStream(filename);
                    break;
                case AudioType.SoundEffect:
                    loadedSounds.Add(filename, LoadSound(filename));
                    break;
                default:
                    throw new ArgumentException($"Unsupported audio type: {type}");
            }
        }

        public void PlayAudio(string filename, AudioType type)
        {
            switch (type)
            {
                case AudioType.Music:
                    PlayMusicStream(backgroundMusic);
                    break;
                case AudioType.SoundEffect:
                    PlaySound(loadedSounds[filename]);
                    break;
                default:
                    throw new ArgumentException($"Unsupported audio type: {type}");
            }
        }

        public void PauseAudio(AudioType type)
        {
            switch (type)
            {
                case AudioType.Music:
                    PauseMusicStream(backgroundMusic);
                    break;
                case AudioType.SoundEffect:
                    // Pause individual sound effects not supported in raylib
                    break;
                default:
                    throw new ArgumentException($"Unsupported audio type: {type}");
            }
        }

        public void ResumeAudio(AudioType type)
        {
            switch (type)
            {
                case AudioType.Music:
                    ResumeMusicStream(backgroundMusic);
                    break;
                case AudioType.SoundEffect:
                    // Resuming individual sound effects not supported in raylib
                    break;
                default:
                    throw new ArgumentException($"Unsupported audio type: {type}");
            }
        }

        public void StopAudio(AudioType type)
        {
            switch (type)
            {
                case AudioType.Music:
                    StopMusicStream(backgroundMusic);
                    break;
                case AudioType.SoundEffect:
                    break;
                default:
                    throw new ArgumentException($"Unsupported audio type: {type}");
            }
        }

        public void UnloadAudio(string filename, AudioType type)
        {
            switch (type)
            {
                case AudioType.Music:
                    UnloadMusicStream(backgroundMusic);
                    break;
                case AudioType.SoundEffect:
                    UnloadSound(loadedSounds[filename]);
                    loadedSounds.Remove(filename);
                    break;
                default:
                    throw new ArgumentException($"Unsupported audio type: {type}");
            }
        }

        public void SetVolume(float volume, AudioType type)
        {
            switch (type)
            {
                case AudioType.Music:
                    SetMusicVolume(backgroundMusic, volume);
                    break;
                case AudioType.SoundEffect:
                    foreach (var sound in loadedSounds.Values)
                    {
                        SetSoundVolume(sound, volume);
                    }
                    break;
                default:
                    throw new ArgumentException($"Unsupported audio type: {type}");
            }
        }

    }
}
