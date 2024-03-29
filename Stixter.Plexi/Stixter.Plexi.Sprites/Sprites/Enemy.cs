﻿using System;
using Microsoft.Xna.Framework;

namespace Stixter.Plexi.Sprites.Sprites
{
    public class Enemy : Character
    {
        private readonly Random _random;
        private readonly float _startPosition;

        public Enemy(Game game, string texture, float startPosition, Random random) : base(game, texture)
        {
            _startPosition = startPosition;
            _random = random;
            MaxCharacterVelocity = 3.0f;
            Reset();
            EnableJumpSound = false;
        }

        public void Reset()
        {
            LastPlayerY = 0f;
            Sprite.Position.X = _startPosition;
        }

        private int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }

        public void MoveCharacter()
        {
            Sprite.Position.Y = LastPlayerY;

            SetCorretSpeedOnPlayer();
            ForceToTurnOnScreenEdge();

            if(RandomNumber(0, 500) > 495)
            {
                CurrentPlayerDirection = CurrentPlayerDirection.Equals(AnimatedSprite.PlayerDirection.Right) 
                    ? AnimatedSprite.PlayerDirection.Left 
                    : AnimatedSprite.PlayerDirection.Right;
            }

            var maxValue = 1000 - (int)Sprite.Position.Y;
            //DebugText = "MaxValue " + maxValue.ToString();
            if (RandomNumber(0, maxValue) >  maxValue- 10)
            {
                PlayerState = State.Jumping;

                if (PlayerState.Equals(State.Jumping) && AllowJump)
                    Jump();
            }
            else
                PlayerState = State.Walking;

            SetCorretSpeedOnPlayer();
            MovePlayerLeftOrRight();
            LastplayerDirection = CurrentPlayerDirection;

            if (!JumpInProgress)
                Sprite.Position.Y = LastPlayerY + 5.0f;
            else
                Sprite.Position.Y = LastPlayerY - 5.0f;

            LastPlayerY = Sprite.Position.Y;
            
            Sprite.Alive = true;
        }

        private void ForceToTurnOnScreenEdge()
        {
            if (Sprite.Position.X >= 1230 && CurrentPlayerDirection == AnimatedSprite.PlayerDirection.Right)
                CurrentPlayerDirection = AnimatedSprite.PlayerDirection.Left;

            else if (Sprite.Position.X <= 10 && CurrentPlayerDirection == AnimatedSprite.PlayerDirection.Left)
                CurrentPlayerDirection = AnimatedSprite.PlayerDirection.Right;
        }
    }
}