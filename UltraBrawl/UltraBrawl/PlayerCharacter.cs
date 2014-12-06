﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace UltraBrawl
{
    class PlayerCharacter : UserControlledSprite
    {
        // state pattern
        const int NUM_STATES = 11;
        public enum PlayerCharacterState
        {
            Idle,
            Running,
            Jumping,
            JumpKicking,
            Punching,
            Kicking,
            Blocking,
            BlockHit,
            Hit,
            Knockdown,
            Charging
        }
        PlayerCharacterState currentState;
        AbstractState[] states;

        public bool update = false;

        //sprite/sound variables
        static Point pcFrameSize;
        public List<Point> pcSegmentEndings = new List<Point>();
        public List<int> pcSegmentTimings = new List<int>();
        static SoundEffect chargeSound;
        static SoundEffect superLoop;
        static public ParticleEngine2D particleEngine;
        public List<Texture2D> particleList;
        public SoundEffectInstance chargeSoundInstance;
        public SoundEffectInstance superLoopInstance;

        //player variables
        public PlayerIndex pcPlayerNum;
        private PlayerController controller;
        private GamePadState oldGamePadState;
        public List<Keys> pcPlayerKeys = new List<Keys>();

        //state variables
        public bool canSuper = false;
        public bool canJump = false;
        public bool cancelSuper = false;
        public bool canMove = true;
        public int jumpCount = 0;
        public bool isSuper = false;
        public int knockDownEndFrame;
        public int chargeTimer = 0;
        public int currentHealth = 100;
        public bool flipped = false;

        //attack variables
        private bool isBlock = false;
        private bool isPunch = false;
        private bool isKick = false;
        private bool isJumpKick = false;
        private const int HIT_TYPE_PUNCH = 0;
        private const int HIT_TYPE_KICK = 1;
        private const int HIT_TYPE_JUMPKICK = 2;
        public double CHARACTER_DAMAGE;

        //character identification
        public int CHARACTER_ID;
        public String CHARACTER_NAME;


        // constructor
        public PlayerCharacter(SpriteSheet spriteSheet, Vector2 position, CollisionOffset collisionOffset, CollisionOffset hitboxOffset, CollisionOffset hitboxOffsetFlipped, CollisionOffset hitboxOffsetNotFlipped, Vector2 speed, Vector2 friction, SoundEffect sound1, SoundEffect sound2, Point frameSize, PlayerIndex playerIndex, PlayerController playerController)
            : base(spriteSheet, position, collisionOffset, hitboxOffset, hitboxOffsetFlipped, hitboxOffsetNotFlipped, speed, friction)
        {
            particleEngine = new ParticleEngine2D(particleList, position);
            pcPlayerNum = playerIndex;
            pcFrameSize = frameSize;
            chargeSound = sound1;
            superLoop = sound2;
            chargeSoundInstance = chargeSound.CreateInstance();
            superLoopInstance = superLoop.CreateInstance();
            superLoopInstance.IsLooped = true;
            controller = playerController;
            isCharacter = true;
        }


        public void setSegments()
        {
            // set the segments and frame size

            //idle
            spriteSheet.addSegment(pcFrameSize, new Point(0, 0), pcSegmentEndings.ElementAt(0), pcSegmentTimings.ElementAt(0));
            //running
            spriteSheet.addSegment(pcFrameSize, new Point(0, 1), pcSegmentEndings.ElementAt(1), pcSegmentTimings.ElementAt(1));
            //jumping
            spriteSheet.addSegment(pcFrameSize, new Point(0, 2), pcSegmentEndings.ElementAt(2), pcSegmentTimings.ElementAt(2));
            //jumpkick
            spriteSheet.addSegment(pcFrameSize, new Point(0, 3), pcSegmentEndings.ElementAt(3), pcSegmentTimings.ElementAt(3));
            //punch
            spriteSheet.addSegment(pcFrameSize, new Point(0, 4), pcSegmentEndings.ElementAt(4), pcSegmentTimings.ElementAt(4));
            //kick
            spriteSheet.addSegment(pcFrameSize, new Point(0, 5), pcSegmentEndings.ElementAt(5), pcSegmentTimings.ElementAt(5));
            //block
            spriteSheet.addSegment(pcFrameSize, new Point(0, 6), pcSegmentEndings.ElementAt(6), pcSegmentTimings.ElementAt(6));
            //blockhit
            spriteSheet.addSegment(pcFrameSize, new Point(0, 7), pcSegmentEndings.ElementAt(7), pcSegmentTimings.ElementAt(7));
            //hit
            spriteSheet.addSegment(pcFrameSize, new Point(0, 8), pcSegmentEndings.ElementAt(8), pcSegmentTimings.ElementAt(8));
            //knockdown
            spriteSheet.addSegment(pcFrameSize, new Point(0, 9), pcSegmentEndings.ElementAt(9), pcSegmentTimings.ElementAt(9));
            //charging
            spriteSheet.addSegment(pcFrameSize, new Point(0, 10), pcSegmentEndings.ElementAt(10), pcSegmentTimings.ElementAt(10));
            //superIdle
            spriteSheet.addSegment(pcFrameSize, new Point(0, 11), pcSegmentEndings.ElementAt(11), pcSegmentTimings.ElementAt(11));
            //superRunning
            spriteSheet.addSegment(pcFrameSize, new Point(0, 12), pcSegmentEndings.ElementAt(12), pcSegmentTimings.ElementAt(12));
            //superJumping
            spriteSheet.addSegment(pcFrameSize, new Point(0, 13), pcSegmentEndings.ElementAt(13), pcSegmentTimings.ElementAt(13));
            //superJumpKick
            spriteSheet.addSegment(pcFrameSize, new Point(0, 14), pcSegmentEndings.ElementAt(14), pcSegmentTimings.ElementAt(14));
            //superPunch
            spriteSheet.addSegment(pcFrameSize, new Point(0, 15), pcSegmentEndings.ElementAt(15), pcSegmentTimings.ElementAt(15));
            //superKick
            spriteSheet.addSegment(pcFrameSize, new Point(0, 16), pcSegmentEndings.ElementAt(16), pcSegmentTimings.ElementAt(16));
            //superBlock
            spriteSheet.addSegment(pcFrameSize, new Point(0, 17), pcSegmentEndings.ElementAt(17), pcSegmentTimings.ElementAt(17));
            //superBlockhit
            spriteSheet.addSegment(pcFrameSize, new Point(0, 18), pcSegmentEndings.ElementAt(18), pcSegmentTimings.ElementAt(18));
            //superHit
            spriteSheet.addSegment(pcFrameSize, new Point(0, 19), pcSegmentEndings.ElementAt(19), pcSegmentTimings.ElementAt(19));

            // define the states
            states = new AbstractState[NUM_STATES];
            states[(Int32)PlayerCharacterState.Idle] = new IdleState(this);
            states[(Int32)PlayerCharacterState.Running] = new RunningState(this);
            states[(Int32)PlayerCharacterState.Jumping] = new JumpingState(this);
            states[(Int32)PlayerCharacterState.Blocking] = new BlockingState(this);
            states[(Int32)PlayerCharacterState.BlockHit] = new BlockHitState(this);
            states[(Int32)PlayerCharacterState.Hit] = new HitState(this);
            states[(Int32)PlayerCharacterState.Knockdown] = new KnockdownState(this);
            states[(Int32)PlayerCharacterState.JumpKicking] = new JumpKickState(this);
            states[(Int32)PlayerCharacterState.Punching] = new PunchState(this);
            states[(Int32)PlayerCharacterState.Kicking] = new KickState(this);
            states[(Int32)PlayerCharacterState.Charging] = new ChargingState(this);

            // start in Idle state
            switchState(PlayerCharacterState.Idle);

        }

        //defined by individual characters
        public virtual void charging()
        {
        }
        public virtual void chargedOne()
        {
        }
        public virtual void chargedTwo()
        {
        }


        //get hit by other character
        public void getHit(SpriteEffects direction, int hitType, double oppDamage)
        {
            //if you aren't blocking or hit OR if you're facing the same direction(your back is to the other player)
            if ((canMove && !isBlock) || direction.Equals(effects))
            {
                if (currentState.Equals(PlayerCharacterState.Charging) || isSuper)
                {
                    isSuper = false;
                    cancelSuper = true;
                }
                if (hitType == HIT_TYPE_JUMPKICK)
                {
                    velocity.Y = -100f;
                    currentHealth -= (int) (10 * oppDamage);
                    if (direction.Equals(SpriteEffects.None))
                    {
                        effects = SpriteEffects.FlipHorizontally;
                        velocity.X = 1000f;
                    }
                    else
                    {
                        effects = SpriteEffects.None;
                        velocity.X = -1000f;
                    }
                    switchState(PlayerCharacterState.Knockdown);
                }
                if (hitType == HIT_TYPE_KICK)
                {
                    if (direction.Equals(SpriteEffects.None))
                    {
                        velocity.X += 100;
                    }
                    else
                    {
                        velocity.X -= 100;
                    }
                    switchState(PlayerCharacterState.Hit);
                    currentHealth -= (int)(6 * oppDamage);
                }
                if (hitType == HIT_TYPE_PUNCH)
                {
                    if (direction.Equals(SpriteEffects.None))
                    {
                        velocity.X += 100;
                    }
                    else
                    {
                        velocity.X -= 100;
                    }
                    switchState(PlayerCharacterState.Hit);
                    currentHealth -= (int)(3 * oppDamage);
                }
            }
            //if you are blocking AND facing the other player
            else if (isBlock && !direction.Equals(effects)){
                if (hitType == HIT_TYPE_JUMPKICK)
                {
                    currentHealth -= (int)(3 * oppDamage);
                    if (direction.Equals(SpriteEffects.None))
                    {
                        velocity.X += 100;
                    }
                    else
                    {
                        velocity.X -= 100;
                    }
                    switchState(PlayerCharacterState.BlockHit);
                }
                if (hitType == HIT_TYPE_KICK)
                {
                    if (direction.Equals(SpriteEffects.None))
                    {
                        velocity.X += 100;
                    }
                    else
                    {
                        velocity.X -= 100;
                    }
                    currentHealth -= (int)(2 * oppDamage);
                    switchState(PlayerCharacterState.BlockHit);
                }
                if (hitType == HIT_TYPE_PUNCH)
                {
                    currentHealth -= (int)(1 * oppDamage);
                }
            }
        }

        /* Direction (Do not modify)*/
        public override Vector2 direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;
                if (canMove)
                {
                    /* keyboard input */
                    if (Keyboard.GetState().IsKeyDown(controller.pcPlayerKeys.ElementAt(2)))
                        inputDirection.X -= 1;
                    if (Keyboard.GetState().IsKeyDown(controller.pcPlayerKeys.ElementAt(3)))
                        inputDirection.X += 1;

                    /* gamepad input */
                    GamePadState gamepadState = GamePad.GetState(pcPlayerNum);
                    if (gamepadState.ThumbSticks.Left.X != 0)
                        inputDirection.X += gamepadState.ThumbSticks.Left.X;
                }
                /* do we need to flip the image? */
                if (inputDirection.X < 0)
                {
                    effects = SpriteEffects.FlipHorizontally;
                    hitboxOffset = hitboxOffsetFlipped;
                }

                else if (inputDirection.X > 0)
                {
                    effects = SpriteEffects.None;
                    hitboxOffset = hitboxOffsetNotFlipped;
                }

                return inputDirection;
            }
        }

        /* Collision */
        public override void Collision(Sprite otherSprite)
        {
            // Platform platform = (Platform)otherSprite;
            System.Type type = otherSprite.GetType();
            if (type.ToString().Equals("UltraBrawl.Platform"))
            {
                Platform platform = (Platform)otherSprite;
                if (velocity.Y > 0 && position.Y + this.spriteSheet.scale * (this.spriteSheet.currentSegment.frameSize.Y - this.collisionOffset.south) > platform.collisionRect.Top && this.collisionRect.Bottom + platform.collisionRect.Top > 2 && this.collisionRect.Bottom - 10 <= platform.collisionRect.Top)
                {
                    velocity.Y = -1;
                    onGround = true;
                    position.Y = platform.collisionRect.Top - spriteSheet.scale * (spriteSheet.currentSegment.frameSize.Y - collisionOffset.south) + 1;
                }

            }

            else if (otherSprite.checkChar())
            {
                PlayerCharacter otherPlayer = (PlayerCharacter)otherSprite;
                if (isJumpKick)
                {
                    velocity.X = 0f;
                    otherPlayer.getHit(effects, HIT_TYPE_JUMPKICK, CHARACTER_DAMAGE);
                    isJumpKick = false;
                }
                else if (isKick)
                {
                    otherPlayer.getHit(effects, HIT_TYPE_KICK, CHARACTER_DAMAGE);
                    isKick = false;
                }
                else if (isPunch)
                {
                    otherPlayer.getHit(effects, HIT_TYPE_PUNCH, CHARACTER_DAMAGE);
                    isPunch = false;
                }
            }
        }

        /* Update */
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if(!update){
                if (isSuper)
                {
                    superLoopInstance.Pause();
                }
            }
            else
            {
                // call Update for the current state
                states[(Int32)currentState].Update(gameTime, clientBounds);

                //turn off SS effect if SS is off
                if (cancelSuper)
                {
                    if (canSuper)
                    {
                        CHARACTER_DAMAGE /= 1.2;
                    }
                    isSuper = false;
                    chargeSoundInstance.Stop();
                    chargeTimer = 0;
                    superLoopInstance.Pause();
                    cancelSuper = false;
                }
                if (isSuper)
                {
                    if (superLoopInstance.State.Equals(SoundState.Paused))
                    {
                        superLoopInstance.Play();
                    }
                }
                // Update the sprite (base class)
                base.Update(gameTime, clientBounds);
            }
        }


        /** Switch State **/
        private void switchState(PlayerCharacterState newState)
        {
            pauseAnimation = false; // just in case

            // switch the state to the given state
            currentState = newState;
            spriteSheet.setCurrentSegment((Int32)newState);
            if (isSuper)
            {
                spriteSheet.setCurrentSegment((Int32)newState + 11);
            }
            System.Diagnostics.Debug.WriteLine(spriteSheet.segments.Length);
            currentFrame = spriteSheet.currentSegment.startFrame;
        }


        /**        **/
        /** STATES **/
        /**        **/


        protected abstract class AbstractState
        {
            protected readonly PlayerCharacter player;
            protected AbstractState(PlayerCharacter player)
            {
                this.player = player;
            }

            public virtual void Update(GameTime gameTime, Rectangle clientBounds)
            {
            }
        }

        /* Idle State */
        private class IdleState : AbstractState
        {
            public IdleState(PlayerCharacter player)
                : base(player)
            {
                // define the standing still frame
                //stillFrame = new Point(14, 0);
            }

            public override void Update(GameTime gameTime, Rectangle clientBounds)
            {
                player.jumpCount = 1;
                player.canJump = true;
                player.pauseAnimation = false;
                //idle->charge
                if (!player.isSuper)
                {
                    if (GamePad.GetState(player.pcPlayerNum).Buttons.Y == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(player.controller.pcPlayerKeys.ElementAt(6)))
                    {
                        player.switchState(PlayerCharacterState.Charging);
                    }
                }
                //idle->run
                if (player.direction.X != 0 || player.direction.Y != 0)
                {
                    player.switchState(PlayerCharacterState.Running);
                }

                //idle->jump
                if (GamePad.GetState(player.pcPlayerNum).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(player.controller.pcPlayerKeys.ElementAt(4)))
                {
                    if (player.onGround)
                    {
                        player.jumpCount++;
                        player.canJump = false;
                        player.switchState(PlayerCharacterState.Jumping);
                        player.velocity.Y += -500;
                    }
                }

                //idle->fall
                if (player.velocity.Y > 0)
                {
                    player.switchState(PlayerCharacterState.Jumping);
                    player.currentFrame.X = player.pcSegmentEndings.ElementAt(2).X - 1;
                }

                //idle->kick
                if ((player.oldGamePadState.Buttons.B == ButtonState.Released && GamePad.GetState(player.pcPlayerNum).Buttons.B == ButtonState.Pressed)|| Keyboard.GetState().IsKeyDown(player.controller.pcPlayerKeys.ElementAt(5)))
                {
                    if (player.onGround)
                    {
                        player.isKick = true;
                        player.switchState(PlayerCharacterState.Kicking);
                    }
                }
                //idle->punch
                if ((player.oldGamePadState.Buttons.X == ButtonState.Released && GamePad.GetState(player.pcPlayerNum).Buttons.X == ButtonState.Pressed) || Keyboard.GetState().IsKeyDown(player.controller.pcPlayerKeys.ElementAt(8)))
                {
                    if (player.onGround)
                    {
                        player.isPunch = true;
                        player.switchState(PlayerCharacterState.Punching);
                    }
                }

                //idle->block
                if (GamePad.GetState(player.pcPlayerNum).Buttons.RightShoulder == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(player.controller.pcPlayerKeys.ElementAt(7)))
                {
                    player.switchState(PlayerCharacterState.Blocking);
                }

                player.oldGamePadState = GamePad.GetState(player.pcPlayerNum);
            }
        }

        /* Running State */
        private class RunningState : AbstractState
        {
            public RunningState(PlayerCharacter player)
                : base(player)
            {
            }

            public override void Update(GameTime gameTime, Rectangle clientBounds)
            {
                player.canJump = true;
                //run->idle
                if (player.direction.X == 0 && player.direction.Y == 0)
                {
                    player.switchState(PlayerCharacterState.Idle);
                }

                //run->punch
                if ((player.oldGamePadState.Buttons.X == ButtonState.Released && GamePad.GetState(player.pcPlayerNum).Buttons.X == ButtonState.Pressed)|| Keyboard.GetState().IsKeyDown(player.controller.pcPlayerKeys.ElementAt(8)))
                {
                    if (player.onGround)
                    {
                        player.isPunch = true;
                        player.switchState(PlayerCharacterState.Punching);
                    }
                }
                //run->kick
                if ((player.oldGamePadState.Buttons.B == ButtonState.Released && GamePad.GetState(player.pcPlayerNum).Buttons.B == ButtonState.Pressed)|| Keyboard.GetState().IsKeyDown(player.controller.pcPlayerKeys.ElementAt(5)))
                {
                    player.isKick = true;
                    player.switchState(PlayerCharacterState.Kicking);
                }

                //run->jump
                if (GamePad.GetState(player.pcPlayerNum).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(player.controller.pcPlayerKeys.ElementAt(4)))
                {
                    player.jumpCount++;
                    player.canJump = false;
                    player.switchState(PlayerCharacterState.Jumping);
                    player.velocity.Y += -500f;
                }
                //run->fall
                if (player.velocity.Y > 0)
                {
                    player.switchState(PlayerCharacterState.Jumping);
                    player.currentFrame.X = player.pcSegmentEndings.ElementAt(2).X - 1;
                }

                //run->block
                if (GamePad.GetState(player.pcPlayerNum).Buttons.RightShoulder == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(player.controller.pcPlayerKeys.ElementAt(7)))
                {
                    player.switchState(PlayerCharacterState.Blocking);
                }

                player.oldGamePadState = GamePad.GetState(player.pcPlayerNum);
            }


        }

        /* Jumping State */
        private class JumpingState : AbstractState
        {

            public JumpingState(PlayerCharacter player)
                : base(player)
            {
            }


            public override void Update(GameTime gameTime, Rectangle clientBounds)
            {
                if(player.velocity.X >= 250){
                    player.velocity.X -= 50;
                }
                if (player.velocity.X <= -250)
                {
                    player.velocity.X += 50;
                }
                //System.Diagnostics.Debug.WriteLine(player.jumpCount + " is less than 2");
                if (player.onGround)
                {
                    player.switchState(PlayerCharacterState.Idle);
                }
                // animate once through -- then go to standing still frame
                if (player.currentFrame.X == player.spriteSheet.currentSegment.endFrame.X)
                {
                    player.currentFrame.X = player.pcSegmentEndings.ElementAt(2).X - 1;
                }
                if (GamePad.GetState(player.pcPlayerNum).Buttons.A == ButtonState.Released && Keyboard.GetState().IsKeyUp(player.controller.pcPlayerKeys.ElementAt(4)))
                {
                    player.canJump = true;
                }

                if (player.jumpCount <= 2 && player.canJump)
                {
                    if (GamePad.GetState(player.pcPlayerNum).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(player.controller.pcPlayerKeys.ElementAt(4)))
                    {
                        System.Diagnostics.Debug.WriteLine("jumpcount is " + player.jumpCount);
                        player.jumpCount++;
                        player.canJump = false;
                        player.velocity.Y = -500f;
                        player.switchState(PlayerCharacterState.Jumping);
                    }
                }
                if (player.jumpCount < 4)
                {
                    if (GamePad.GetState(player.pcPlayerNum).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(player.controller.pcPlayerKeys.ElementAt(5)))
                    {
                        player.isJumpKick = true;
                        player.jumpCount += 4;
                        player.switchState(PlayerCharacterState.JumpKicking);
                    }
                }
            }
        }

        /* Knockdown State  (NOT SET UP)*/
        private class KnockdownState : AbstractState
        {

            public KnockdownState(PlayerCharacter player)
                : base(player)
            {
            }


            public override void Update(GameTime gameTime, Rectangle clientBounds)
            {
                player.canMove = false;
                player.canJump = false;
                if (player.currentFrame.X > player.knockDownEndFrame - 1 & player.onGround)
                {
                    player.velocity.X = 0;
                    player.velocity.Y = 0;
                }
                if (player.currentFrame.X < player.knockDownEndFrame - 2 && player.effects == SpriteEffects.None)
                {
                    player.velocity.X = -1700;
                }
                else if (player.currentFrame.X < player.knockDownEndFrame - 2 && player.effects == SpriteEffects.FlipHorizontally)
                {
                    player.velocity.X = 1700;
                }
                if (player.currentFrame.X >= player.knockDownEndFrame - 1 & !player.onGround)
                {
                    if (player.effects == SpriteEffects.None)
                    {
                        player.velocity.X = -250f;
                    }
                    else
                    {
                        player.velocity.X = 250f;
                    }
                    player.currentFrame.X = player.knockDownEndFrame - 1;
                }
                // animate once through -- then go to standing still frame
                if (player.currentFrame.X >= player.spriteSheet.currentSegment.endFrame.X)
                {

                    player.velocity.X = 0f;
                    player.canMove = true;
                    player.canJump = true;
                    player.switchState(PlayerCharacterState.Idle);
                }
            }
        }
        /* BlockHit State  (NOT SET UP)*/
        private class BlockHitState : AbstractState
        {

            public BlockHitState(PlayerCharacter player)
                : base(player)
            {
            }


            public override void Update(GameTime gameTime, Rectangle clientBounds)
            {
                player.canMove = false;
                player.canJump = false;
                // animate once through -- then go to standing still frame
                if (player.currentFrame.X == player.spriteSheet.currentSegment.endFrame.X)
                {
                    player.velocity.X = 0f;
                    player.canMove = true;
                    player.canJump = true;
                    player.switchState(PlayerCharacterState.Idle);
                }
            }
        }
        
        /* Hit State  (NOT SET UP)*/
        private class HitState : AbstractState
        {

            public HitState(PlayerCharacter player)
                : base(player)
            {
            }


            public override void Update(GameTime gameTime, Rectangle clientBounds)
            {
                player.canMove = false;
                player.canJump = false;
                // animate once through -- then go to standing still frame
                if (player.currentFrame.X == player.spriteSheet.currentSegment.endFrame.X)
                {
                    player.velocity.X = 0f;
                    player.canMove = true;
                    player.canJump = true;
                    player.switchState(PlayerCharacterState.Idle);
                }
            }
        }

        /* Kick State  (NOT SET UP)*/
        private class KickState : AbstractState
        {
            
            public KickState(PlayerCharacter player)
                : base(player)
            {
            }

            public override void Update(GameTime gameTime, Rectangle clientBounds)
            {
                player.canMove = false;
                Point ssEndFrame = new Point(8, 8);
                if (player.currentFrame.X == player.spriteSheet.currentSegment.endFrame.X || player.currentFrame.X == ssEndFrame.X)
                {
                    player.canMove = true;
                    player.isKick = false;
                    player.switchState(PlayerCharacterState.Idle);
                }
            }
        }

        /* Punch State  (NOT SET UP)*/
        private class PunchState : AbstractState
        {

            public PunchState(PlayerCharacter player)
                : base(player)
            {
            }

            public override void Update(GameTime gameTime, Rectangle clientBounds)
            {
                player.velocity.X = 0;
                if (player.currentFrame == player.spriteSheet.currentSegment.endFrame)
                {
                    player.isPunch = false;
                    if (player.onGround == false)
                    {
                        player.switchState(PlayerCharacterState.Jumping);
                    }
                    else
                    {
                        player.switchState(PlayerCharacterState.Idle);
                    }
                }
            }
        }

        /* Jump Kick State */
        private class JumpKickState : AbstractState
        {

            public JumpKickState(PlayerCharacter player)
                : base(player)
            {
            }

            public override void Update(GameTime gameTime, Rectangle clientBounds)
            {
                player.canMove = false;
                Point ssEndFrame = new Point(8, 8);
                if (player.isJumpKick)
                {
                    if (player.effects == SpriteEffects.None)
                    {
                        player.velocity.X += 400f;

                    }
                    else
                    {
                        player.velocity.X += -400f;
                    }
                }
                if (player.currentFrame.X + 2 >= player.spriteSheet.currentSegment.endFrame.X || player.currentFrame.X + 2 == ssEndFrame.X)
                {
                    player.isJumpKick = false;
                }

                if (player.currentFrame == player.spriteSheet.currentSegment.endFrame )
                {
                    player.canMove = true;
                    if (player.onGround == false)
                    {
                        player.switchState(PlayerCharacterState.Jumping);
                    }
                    else
                    {
                        player.switchState(PlayerCharacterState.Running);
                    }
                }
            }
        }

        /* Charging State */
        private class ChargingState : AbstractState
        {

            public ChargingState(PlayerCharacter player)
                : base(player)
            {
            }

            public override void Update(GameTime gameTime, Rectangle clientBounds)
            {
                if (player.chargeSoundInstance.State == SoundState.Stopped)
                {
                    player.chargeSoundInstance.Play();
                }

                System.Diagnostics.Debug.WriteLine("State " + player.chargeSoundInstance.State);
                player.chargeSoundInstance.Volume = 0.5f;
                const int chargeMax = 2500;
                if (GamePad.GetState(player.pcPlayerNum).Buttons.Y == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(player.controller.pcPlayerKeys.ElementAt(6)))
                {
                    player.chargeTimer += gameTime.ElapsedGameTime.Milliseconds;

                }
                else
                {
                    player.chargeTimer = 0;
                    player.switchState(PlayerCharacterState.Idle);
                    player.chargeSoundInstance.Stop(true);
                    player.cancelSuper = true;
                }

                if (player.direction.X != 0 || player.direction.Y != 0)
                {
                    player.chargeSoundInstance.Stop(true);
                    player.switchState(PlayerCharacterState.Running);
                    player.chargeTimer = 0;
                    player.cancelSuper = true;
                }

                if ((player.chargeTimer + 500) > chargeMax)
                {
                    player.chargedOne(); 
                }
                if (!player.canSuper)
                {
                    if (player.chargeTimer > chargeMax && Keyboard.GetState().IsKeyUp(player.controller.pcPlayerKeys.ElementAt(6)) && GamePad.GetState(player.pcPlayerNum).Buttons.Y == ButtonState.Released)
                    {
                        player.chargeTimer = 0;
                        player.chargeSoundInstance.Stop(true);
                        player.switchState(PlayerCharacterState.Idle);
                    }
                    if (player.currentFrame.X > player.pcSegmentEndings.ElementAt(10).X - 2 && (Keyboard.GetState().IsKeyDown(player.controller.pcPlayerKeys.ElementAt(6)) || GamePad.GetState(player.pcPlayerNum).Buttons.Y == ButtonState.Pressed))
                    {
                        player.currentFrame.X = player.pcSegmentEndings.ElementAt(10).X - 2;
                    }
                }
                else
                {
                    if (player.chargeTimer > chargeMax)
                    {
                        player.chargedTwo();
                        player.chargeTimer = 0;
                        player.chargeSoundInstance.Stop(true);
                        player.switchState(PlayerCharacterState.Idle);
                    }
                }
            }
        }
       
        /* Blocking State (NOT SET UP)*/
        private class BlockingState : AbstractState
        {
            public BlockingState(PlayerCharacter player)
                : base(player)
            {
            }

            public override void Update(GameTime gameTime, Rectangle clientBounds)
            {
                player.isBlock = true;
                player.canJump = false;
                player.canMove = false;
                player.pauseAnimation = false;
                if (GamePad.GetState(player.pcPlayerNum).Buttons.RightShoulder == ButtonState.Released && Keyboard.GetState().IsKeyUp(player.controller.pcPlayerKeys.ElementAt(7)))
                {
                    player.isBlock = false;
                    player.canJump = true;
                    player.canMove = true;
                    player.switchState(PlayerCharacterState.Idle);
                }
            }
        }

        //getter for currentstate
        public String getPlayerState()
        {
            return currentState.ToString();
        }


    }
}
