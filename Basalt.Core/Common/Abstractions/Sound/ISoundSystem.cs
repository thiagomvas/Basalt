using Basalt.Core.Common.Abstractions.Engine;

namespace Basalt.Core.Common.Abstractions.Sound
{
	/// <summary>
	/// Represents a sound system component that handles audio playback.
	/// </summary>
	public interface ISoundSystem : IEngineComponent
	{

		/// <summary>
		/// Plays a loaded audio resource.
		/// </summary>
		/// <param name="audioCacheKey">The filename of the audio resource to play.</param>
		/// <param name="type">The type of audio.</param>
		void PlayAudio(string audioCacheKey, AudioType type);

		/// <summary>
		/// Pauses the currently playing audio.
		/// </summary>
		/// <param name="type">The type of audio.</param>
		void PauseAudio(AudioType type);

		/// <summary>
		/// Resumes paused audio.
		/// </summary>
		/// <param name="type">The type of audio.</param>
		void ResumeAudio(AudioType type);

		/// <summary>
		/// Stops playing audio.
		/// </summary>
		/// <param name="type">The type of audio.</param>
		void StopAudio(AudioType type);

		/// <summary>
		/// Sets the volume for the specified audio type.
		/// </summary>
		/// <param name="volume">The volume level.</param>
		void SetVolume(float volume);

		/// <summary>
		/// Checks if music is currently playing.
		/// </summary>
		/// <returns><c>true</c> if music is playing; otherwise, <c>false</c>.</returns>
		bool IsMusicPlaying();

		/// <summary>
		/// Gets the currently playing music.
		/// </summary>
		/// <returns>The currently playing music.</returns>
		object? GetMusicPlaying();
	}
}
