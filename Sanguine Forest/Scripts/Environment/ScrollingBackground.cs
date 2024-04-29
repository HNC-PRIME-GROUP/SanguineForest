using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sanguine_Forest.Scripts.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanguine_Forest.Scripts.Environment
{
    internal class ScrollingBackground : Component
    {




        private bool _constantSpeed;

        private float _layerBackground;

        private float _scrollingSpeed;

        private List <Sprite> _sprites;

        private readonly Character2 _character;

        private float _speed;

        public float LayerBackground
        {
            get { return _layerBackground; }

            set
            {
                _layerBackground = value;

                foreach (var sprite in _sprites)
                    sprite.LayerBackground = _layerBackground;
            }
        }

        public ScrollingBackground(Texture2D texture, Character2 character, float scrollingSpeed, bool constantSpeed = false)
            : this (new List<Texture2D>() { texture, texture }, character, scrollingSpeed, constantSpeed)
        {

        }


        public ScrollingBackground(List<Texture2D> textures, Character2 character, float scrollingSpeed, bool constantSpeed = false)
        {
            _character = character;

            _scrollingSpeed = scrollingSpeed;

            _constantSpeed = constantSpeed;

            _sprites = new List<Sprite>();

            for (int i = 0;  i < textures.Count; i++)
            {
                var texture = textures[i];

                _sprites.Add(new Sprite(texture)
                {
                    // Lock image in the screen
                    PositionBackground = new Vector2(_character.GetPosition().X-(i * texture.Width) - texture.Width*2/3 - 1, _character.GetPosition().Y-  texture.Height/2),

                });
            }
        }

        public override void Update(GameTime gameTime)
        {
            ApplySpeed(gameTime);
            CheckPosition();

        }
        private void ApplySpeed(GameTime gameTime)
        {
            _speed = (float)(_scrollingSpeed * gameTime.ElapsedGameTime.TotalSeconds);

            if (!_constantSpeed || _character.GetVelocity() > 0)
                _speed *= _character.GetVelocity();

            foreach (var sprite in _sprites)
            {
                sprite.PositionBackground = new Vector2(sprite.PositionBackground.X - _speed, sprite.PositionBackground.Y);

            }
        }

        private void CheckPosition()
        {
            for(int i = 0; i < _sprites.Count; i++)
            {
                var sprite = _sprites[i];
                
                if (sprite.RectangleBackground.Right <= 0)
                {
                    var index = i - 1;

                    if (index < 0)
                        index = _sprites.Count - 1;

                    sprite.PositionBackground.X = _sprites[index].RectangleBackground.Right;
                }
            }
        }



        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var sprite in _sprites)
                sprite.Draw(gameTime, spriteBatch);

           // DebugManager.DebugRectangle(_sprites[1].RectangleBackground);
        }

    }
}
