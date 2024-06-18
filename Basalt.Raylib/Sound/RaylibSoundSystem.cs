using Basalt.Common.Utils;
using Basalt.Core.Common.Abstractions.Engine;
using Basalt.Core.Common.Abstractions.Sound;
using Basalt.Raylib.Graphics;
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
		private Music? MusicPlaying;
		private float volume = 1;

		/// <summary>
		/// Initializes the sound system.
		/// </summary>
		public void Initialize()
		{
			logger = Engine.Instance.Logger;
			InitAudioDevice();
			Engine.Instance.GetEngineComponent<IEventBus>()!.Subscribe(BasaltConstants.UpdateEventKey, UpdateStream);
		}

		private void UpdateStream(object? sender, EventArgs args)
		{
			if (MusicPlaying is not null)
			{
				UpdateMusicStream(MusicPlaying.Value);
			}
		}

		/// <summary>
		/// Shuts down the sound system.
		/// </summary>
		public void Shutdown()
		{
			logger?.LogInformation("Shutting down sound system.");
			CloseAudioDevice();
			logger?.LogInformation("Unloaded all sounds and music.");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RaylibSoundSystem"/> class.
		/// </summary>
		/// <param name="logger">The logger to use for logging.</param>
		public RaylibSoundSystem()
		{
		}

		/// <summary>
		/// Plays an audio file.
		/// </summary>
		/// <param name="filename">The filename of the audio file.</param>
		/// <param name="type">The type of the audio file.</param>
		public void PlayAudio(string audioCacheKey, AudioType type)
		{
			switch (type)
			{
				case AudioType.Music:
					MusicPlaying = ResourceCache.Instance.GetMusic(audioCacheKey);
					PlayMusicStream(MusicPlaying.Value);
					break;
				case AudioType.SoundEffect:
					PlaySound(ResourceCache.Instance.GetSound(audioCacheKey)!.Value);
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
		/// Sets the volume of the audio.
		/// </summary>
		/// <param name="volume">The volume value.</param>
		/// <param name="type">The type of the audio file.</param>
		public void SetVolume(float volume)
		{
			SetMasterVolume(volume);
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
