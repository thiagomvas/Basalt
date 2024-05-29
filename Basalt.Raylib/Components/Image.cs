using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Utils;
using Basalt.Raylib.Graphics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;

namespace Basalt.Raylib.Components
{
	public class Image : UIComponent
	{
		private string _textureKey = string.Empty;
		public string TextureKey
		{
			get => _textureKey;
			set
			{
				_textureKey = value;
				_texture = null;
			}
		}
		public float Scale { get; set; } = 1f;
		public Color Tint { get; set; } = Color.White;

		private Texture2D? _texture;

		/// <summary>
		/// Initializes a new instance of the <see cref="Image"/> class.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public Image(Entity entity) : base(entity)
		{
		}

		/// <inheritdoc/>
		public override void OnStart()
		{

		}

		/// <inheritdoc/>
		public override void OnUpdate()
		{

		}

		/// <inheritdoc/>
		public override void OnUIRender()
		{
			if (_texture == null)
				_texture = ResourceCache.Instance.GetTexture(TextureKey);
			var position = GetPivotedPosition(new Vector2(GetScreenWidth(), GetScreenHeight())) + Offset;
			DrawTextureEx(_texture!.Value, position - new Vector2(_texture.Value.Width, _texture.Value.Height) * Scale * 0.5f, Rotation, Scale, Tint);
		}
	}
}
