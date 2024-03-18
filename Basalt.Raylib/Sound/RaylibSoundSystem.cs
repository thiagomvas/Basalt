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
        private Dictionary<string, Music> loadedMusic;
        private Music? MusicPlaying;
        private List<string> queuedSounds = new();
        private List<string> queuedMusic = new();



        public void Initialize()
        {
            InitAudioDevice();
            foreach (var sound in queuedSounds)
            {
                LoadAudio(sound, AudioType.SoundEffect);
            }
            foreach (var music in queuedMusic)
            {
				LoadAudio(music, AudioType.Music);
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
            foreach (var sound in loadedMusic.Values)
            {
				UnloadMusicStream(sound);
			}
            logger?.LogInformation("Unloaded all sounds and music.");
        }


        public RaylibSoundSystem(ILogger? logger = null)
        {
            this.logger = logger;
            loadedSounds = new Dictionary<string, Raylib_cs.Sound>();
            loadedMusic = new Dictionary<string, Music>();
        }

        public void LoadAudio(string filename, AudioType type)
        {
            if (!Engine.Instance.HasStarted)
            {
                queuedSounds.Add(filename);
                queuedMusic.Add(filename);
                return;
            }


            switch (type)
            {
                case AudioType.Music:
                    loadedMusic.Add(filename, LoadMusicStream(filename));
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
                    MusicPlaying = loadedMusic[filename];
                    PlayMusicStream(MusicPlaying.Value);
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
                    if(MusicPlaying is not null) PauseMusicStream(MusicPlaying.Value);
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
                    if(MusicPlaying is not null) ResumeMusicStream(MusicPlaying.Value);
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
                    if(MusicPlaying is not null) StopMusicStream(MusicPlaying.Value);
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
                    UnloadMusicStream(loadedMusic[filename]);
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
                    if(MusicPlaying is not null) SetMusicVolume(MusicPlaying.Value, volume);
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

        public bool IsMusicPlaying() => MusicPlaying is not null;
        public object? GetMusicPlaying() => MusicPlaying;

    }
}
