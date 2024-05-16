using Basalt.Core.Common.Abstractions.Engine;
using Basalt.Core.Common.Abstractions.Sound;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Sound
{
	/// <summary>
	/// Represents a sound system implementation using Raylib.
	/// </summary>
	public class RaylibSoundSystem : ISoundSystem
	{
		private ILogger? logger;
		private Dictionary<string, Raylib_cs.Sound> loadedSounds;
		private Dictionary<string, Music> loadedMusic;
		private Music? MusicPlaying;
		private List<string> queuedSounds = new();
		private List<string> queuedMusic = new();

		/// <summary>
		/// Initializes the sound system.
		/// </summary>
		public void Initialize()
		{
			logger = Engine.Instance.Logger;
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

		/// <summary>
		/// Shuts down the sound system.
		/// </summary>
		public void Shutdown()
		{
			logger?.LogInformation("Shutting down sound system.");
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

		/// <summary>
		/// Initializes a new instance of the <see cref="RaylibSoundSystem"/> class.
		/// </summary>
		/// <param name="logger">The logger to use for logging.</param>
		public RaylibSoundSystem()
		{
			loadedSounds = new Dictionary<string, Raylib_cs.Sound>();
			loadedMusic = new Dictionary<string, Music>();
		}

		/// <summary>
		/// Loads an audio file.
		/// </summary>
		/// <param name="filename">The filename of the audio file.</param>
		/// <param name="type">The type of the audio file.</param>
		public void LoadAudio(string filename, AudioType type)
		{
			if (!Engine.Instance.Running)
			{
				if (type == AudioType.SoundEffect) queuedSounds.Add(filename);
				else queuedMusic.Add(filename);
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

		/// <summary>
		/// Plays an audio file.
		/// </summary>
		/// <param name="filename">The filename of the audio file.</param>
		/// <param name="type">The type of the audio file.</param>
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

		/// <summary>
		/// Pauses the audio playback.
		/// </summary>
		/// <param name="type">The type of the audio file.</param>
		public void PauseAudio(AudioType type)
		{
			switch (type)
			{
				case AudioType.Music:
					if (MusicPlaying is not null) PauseMusicStream(MusicPlaying.Value);
					break;
				case AudioType.SoundEffect:
					// Pause individual sound effects not supported in raylib
					break;
				default:
					throw new ArgumentException($"Unsupported audio type: {type}");
			}
		}

		/// <summary>
		/// Resumes the audio playback.
		/// </summary>
		/// <param name="type">The type of the audio file.</param>
		public void ResumeAudio(AudioType type)
		{
			switch (type)
			{
				case AudioType.Music:
					if (MusicPlaying is not null) ResumeMusicStream(MusicPlaying.Value);
					break;
				case AudioType.SoundEffect:
					// Resuming individual sound effects not supported in raylib
					break;
				default:
					throw new ArgumentException($"Unsupported audio type: {type}");
			}
		}

		/// <summary>
		/// Stops the audio playback.
		/// </summary>
		/// <param name="type">The type of the audio file.</param>
		public void StopAudio(AudioType type)
		{
			switch (type)
			{
				case AudioType.Music:
					if (MusicPlaying is not null) StopMusicStream(MusicPlaying.Value);
					break;
				case AudioType.SoundEffect:
					break;
				default:
					throw new ArgumentException($"Unsupported audio type: {type}");
			}
		}

		/// <summary>
		/// Unloads an audio file.
		/// </summary>
		/// <param name="filename">The filename of the audio file.</param>
		/// <param name="type">The type of the audio file.</param>
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

		/// <summary>
		/// Sets the volume of the audio.
		/// </summary>
		/// <param name="volume">The volume value.</param>
		/// <param name="type">The type of the audio file.</param>
		public void SetVolume(float volume, AudioType type)
		{
			switch (type)
			{
				case AudioType.Music:
					if (MusicPlaying is not null) SetMusicVolume(MusicPlaying.Value, volume);
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

		/// <summary>
		/// Checks if music is currently playing.
		/// </summary>
		/// <returns><c>true</c> if music is playing; otherwise, <c>false</c>.</returns>
		public bool IsMusicPlaying() => MusicPlaying is not null;

		/// <summary>
		/// Gets the currently playing music.
		/// </summary>
		/// <returns>The currently playing music.</returns>
		public object? GetMusicPlaying() => MusicPlaying;
	}
}
