using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Utils;
using Basalt.Raylib.Graphics;
using Raylib_cs;

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
		public Image(Entity entity) : base(entity)
		{
		}

		public override void OnStart()
		{

		}

		public override void OnUpdate()
		{

		}

		public override void OnUIRender()
		{
			if (_texture == null)
				_texture = ResourceCache.Instance.GetTexture(TextureKey);
			var position = GetPivotedPosition(new(Raylib_cs.Raylib.GetScreenWidth(), Raylib_cs.Raylib.GetScreenHeight())) + Offset;
			Raylib_cs.Raylib.DrawTextureEx(_texture.Value, position, Rotation, Scale, Tint);
		}
	}
}
