using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basalt.Core.Common.Abstractions.Engine;

namespace Basalt.Core.Common.Abstractions.Sound
{
    public interface ISoundSystem : IEngineComponent
	{
		void LoadAudio(string filename, AudioType type);

		// Play a loaded audio resource
		void PlayAudio(string filename, AudioType type);

		// Pause currently playing audio
		void PauseAudio(AudioType type);

		// Resume paused audio
		void ResumeAudio(AudioType type);

		// Stop playing audio
		void StopAudio(AudioType type);

		// Unload audio resource
		void UnloadAudio(string filename, AudioType type);

		// Set volume for audio type
		void SetVolume(float volume, AudioType type);

		bool IsMusicPlaying();
		object? GetMusicPlaying();
	}
}
