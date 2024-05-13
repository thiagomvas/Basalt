using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Utils;

/// <summary>
/// Represents a light in the scene.
/// </summary>
public struct Light
{
	/// <summary>
	/// Gets or sets a value indicating whether the light is enabled.
	/// </summary>
	public bool Enabled { get; set; }

	/// <summary>
	/// Gets or sets the type of the light.
	/// </summary>
	public LightType Type { get; set; }

	/// <summary>
	/// Gets or sets the position of the light.
	/// </summary>
	public Vector3 Position { get; set; }

	/// <summary>
	/// Gets or sets the target of the light.
	/// </summary>
	public Vector3 Target { get; set; }

	/// <summary>
	/// Gets or sets the color of the light.
	/// </summary>
	public Color Color { get; set; }

	/// <summary>
	/// Gets or sets the location of the "enabled" uniform in the shader.
	/// </summary>
	public int EnabledLoc { get; set; }

	/// <summary>
	/// Gets or sets the location of the "type" uniform in the shader.
	/// </summary>
	public int TypeLoc { get; set; }

	/// <summary>
	/// Gets or sets the location of the "position" uniform in the shader.
	/// </summary>
	public int PosLoc { get; set; }

	/// <summary>
	/// Gets or sets the location of the "target" uniform in the shader.
	/// </summary>
	public int TargetLoc { get; set; }

	/// <summary>
	/// Gets or sets the location of the "color" uniform in the shader.
	/// </summary>
	public int ColorLoc { get; set; }
}

/// <summary>
/// Represents the type of a light.
/// </summary>
public enum LightType
{
	/// <summary>
	/// A directional light.
	/// </summary>
	Directional,

	/// <summary>
	/// A point light.
	/// </summary>
	Point
}

/// <summary>
/// Provides utility methods for working with lights.
/// </summary>
public static class Rlights
{
	/// <summary>
	/// Creates a new light and sets its initial values.
	/// </summary>
	/// <param name="lightsCount">The number of lights.</param>
	/// <param name="type">The type of the light.</param>
	/// <param name="pos">The position of the light.</param>
	/// <param name="target">The target of the light.</param>
	/// <param name="color">The color of the light.</param>
	/// <param name="shader">The shader to apply the light to.</param>
	/// <returns>The created light.</returns>
	public static Light CreateLight(
		int lightsCount,
		LightType type,
		Vector3 pos,
		Vector3 target,
		Color color,
		Shader shader)
	{
		Light light = new();

		light.Enabled = true;
		light.Type = type;
		light.Position = pos;
		light.Target = target;
		light.Color = color;

		string enabledName = "lights[" + lightsCount + "].enabled";
		string typeName = "lights[" + lightsCount + "].type";
		string posName = "lights[" + lightsCount + "].position";
		string targetName = "lights[" + lightsCount + "].target";
		string colorName = "lights[" + lightsCount + "].color";

		light.EnabledLoc = GetShaderLocation(shader, enabledName);
		light.TypeLoc = GetShaderLocation(shader, typeName);
		light.PosLoc = GetShaderLocation(shader, posName);
		light.TargetLoc = GetShaderLocation(shader, targetName);
		light.ColorLoc = GetShaderLocation(shader, colorName);

		UpdateLightValues(shader, light);

		return light;
	}

	/// <summary>
	/// Updates the values of a light in the shader.
	/// </summary>
	/// <param name="shader">The shader to update.</param>
	/// <param name="light">The light to update.</param>
	public static void UpdateLightValues(Shader shader, Light light)
	{
		SetShaderValue(shader, light.EnabledLoc, light.Enabled ? 1 : 0, ShaderUniformDataType.Int);
		SetShaderValue(shader, light.TypeLoc, (int)light.Type, ShaderUniformDataType.Int);
		SetShaderValue(shader, light.PosLoc, light.Position, ShaderUniformDataType.Vec3);
		SetShaderValue(shader, light.TargetLoc, light.Target, ShaderUniformDataType.Vec3);

		float[] color = new[]
		{
				(float)light.Color.R / 255f,
				(float)light.Color.G / 255f,
				(float)light.Color.B / 255f,
				(float)light.Color.A / 255f
			};
		SetShaderValue(shader, light.ColorLoc, color, ShaderUniformDataType.Vec4);
	}
}
