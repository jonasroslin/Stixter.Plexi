using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stixter.Plexi.Sprites.Sprites;

namespace Stixter.Plexi.ScreenManager.Menu
{
    public class MenuComponent : DrawableGameComponent
    {
        private readonly List<MenuItems.MenuItem> _menuItems;
        private int _selectedIndex;
        private readonly Color _notSelectedColor = Color.Yellow;
        private readonly Color _slectedColor = Color.Orange;
        private KeyboardState _keyboardState;
        private KeyboardState _oldKeyboardState;
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _spriteFont;
        private Vector2 _position;
        private float _width;
        private float _height;
        private readonly MenuBackgroundItem _menuBackgroundItem;
        private readonly SoundEffect _menuChangeSelected;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                if (_selectedIndex < 0)
                    _selectedIndex = 0;
                if (_selectedIndex >= _menuItems.Count)
                    _selectedIndex = _menuItems.Count - 1;
            }
        }

        public MenuComponent(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, List<MenuItems.MenuItem> menuItems)
            : base(game)
        {
            _menuChangeSelected = game.Content.Load<SoundEffect>("Sounds\\button-16");
           
            _spriteBatch = spriteBatch;
            _spriteFont = spriteFont;
            _menuItems = menuItems;
            MeasureMenu();
            _menuBackgroundItem = new MenuBackgroundItem(game.Content) {Sprite = {Alive = true}};
        }

        private void MeasureMenu()
        {
            _height = 0;
            _width = 0;
            foreach (var item in _menuItems)
            {
                var size = _spriteFont.MeasureString(item.ToString());
                if (size.X > _width)
                    _width = size.X;
                _height += _spriteFont.LineSpacing + 50;
            }

            _position = new Vector2(
                (Game.Window.ClientBounds.Width - _width) / 2, 
                (Game.Window.ClientBounds.Height - _height) / 2);
        }

        private bool CheckKey(Keys theKey)
        {
            return _keyboardState.IsKeyUp(theKey) && _oldKeyboardState.IsKeyDown(theKey);
        }

        public override void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();

            if (CheckKey(Keys.Down))
            {
                _menuChangeSelected.Play();
                _selectedIndex++;
                if (_selectedIndex == _menuItems.Count)
                    _selectedIndex = 0;
            }

            if (CheckKey(Keys.Up))
            {
                _menuChangeSelected.Play();
                _selectedIndex--;
                if (_selectedIndex < 0)
                    _selectedIndex = _menuItems.Count - 1;
            }

            if (CheckKey(Keys.Enter))
            {
            }

            base.Update(gameTime);
            _oldKeyboardState = _keyboardState;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            var location = _position;
            var boxLocation = _position;
            boxLocation.X = location.X + 80f;
            boxLocation.Y = location.Y + 15f;
            
            for (var i = 0; i < _menuItems.Count; i++)
            {
                var color = i == _selectedIndex 
                                 ? _slectedColor 
                                 : _notSelectedColor;

                _menuBackgroundItem.Sprite.Position = boxLocation;

                if (i == _selectedIndex)
                    _menuBackgroundItem.SetSelected();
                else
                    _menuBackgroundItem.SetNotSelected();

                _menuBackgroundItem.Draw(_spriteBatch);
                _spriteBatch.DrawString(_spriteFont, MenuItems.GetLabelFromItem(_menuItems[i]), location, color);
                location.Y += _spriteFont.LineSpacing + 30;
                boxLocation.Y += _spriteFont.LineSpacing + 30;
                
            }
        }      
    }
}
