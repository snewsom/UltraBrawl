using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace UltraBrawl
{
    class PlayerCharacter : UserControlledSprite
    {
        // state pattern
        const int NUM_STATES = 9;
        public enum PlayerCharacterState
        {
            Idle,
            Running,
            Jumping,
            Hit,
            JumpKicking,
            Punching,
            Kicking,
            Blocking,
            Charging
        }
        PlayerCharacterState currentState;
        AbstractState[] states;

        //sprite/sound variables
        static Point pcFrameSize;
        public List<Point> pcSegmentEndings = new List<Point>();
        static SoundEffect chargeSound;
        static SoundEffect superLoop;
        public SoundEffectInstance chargeSoundInstance;
        public SoundEffectInstance superLoopInstance;

        //player variables
        public PlayerIndex pcPlayerNum;
        private PlayerController controller;
        public List<Keys> pcPlayerKeys = new List<Keys>();

        //state variables
        public bool canJump = false;
        public bool cancelSuper = false;
        public bool canMove = true;
        public int jumpCount = 0;
        public bool isSuper = false;
        public int chargeTimer = 0;
        public int currentHealth = 100;

        //attack variables
        private bool isBlock = false;
        private bool isPunch = false;
        private bool isKick = false;
        private bool isJumpKick = false;
        private const int HIT_TYPE_PUNCH = 0;
        private const int HIT_TYPE_KICK = 1;
        private const int HIT_TYPE_JUMPKICK = 2;

        //character identification
        public int CHARACTER_ID;
        public String CHARACTER_NAME;


        // constructor
        public PlayerCharacter(SpriteSheet spriteSheet, Vector2 position,
            CollisionOffset collisionOffset, Vector2 speed, Vector2 friction, SoundEffect sound1, SoundEffect sound2, Point frameSize, PlayerIndex playerIndex, PlayerController playerController)
            : base(spriteSheet, position, collisionOffset, speed, friction)
        {
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
            spriteSheet.addSegment(pcFrameSize, new Point(0, 0), pcSegmentEndings.ElementAt(0), 50);
            //running
            spriteSheet.addSegment(pcFrameSize, new Point(0, 1), pcSegmentEndings.ElementAt(1), 80);
            //jumping
            spriteSheet.addSegment(pcFrameSize, new Point(0, 2), pcSegmentEndings.ElementAt(2), 120);
            //hit
            spriteSheet.addSegment(pcFrameSize, new Point(0, 11), pcSegmentEndings.ElementAt(11), 40);
            //jumpkick
            spriteSheet.addSegment(pcFrameSize, new Point(0, 3), pcSegmentEndings.ElementAt(3), 40);
            //punch
            spriteSheet.addSegment(pcFrameSize, new Point(0, 9), pcSegmentEndings.ElementAt(9), 40);
            //kick
            spriteSheet.addSegment(pcFrameSize, new Point(0, 10), pcSegmentEndings.ElementAt(10), 40);
            //block
            spriteSheet.addSegment(pcFrameSize, new Point(0, 0), pcSegmentEndings.ElementAt(0), 50);
            //charging
            spriteSheet.addSegment(pcFrameSize, new Point(0, 4), pcSegmentEndings.ElementAt(4), 140);
            //superIdle
            spriteSheet.addSegment(pcFrameSize, new Point(0, 5), pcSegmentEndings.ElementAt(5), 50);
            //superRunning
            spriteSheet.addSegment(pcFrameSize, new Point(0, 6), pcSegmentEndings.ElementAt(6), 80);
            //superJumping
            spriteSheet.addSegment(pcFrameSize, new Point(0, 7), pcSegmentEndings.ElementAt(7), 120);
            //superHit
            spriteSheet.addSegment(pcFrameSize, new Point(0, 11), pcSegmentEndings.ElementAt(11), 40);
            //superJumpKick
            spriteSheet.addSegment(pcFrameSize, new Point(0, 8), pcSegmentEndings.ElementAt(8), 40);
            //superPunch
            spriteSheet.addSegment(pcFrameSize, new Point(0, 9), pcSegmentEndings.ElementAt(9), 40);
            //superKick
            spriteSheet.addSegment(pcFrameSize, new Point(0, 10), pcSegmentEndings.ElementAt(10), 40);
            //superBlock
            spriteSheet.addSegment(pcFrameSize, new Point(0, 5), pcSegmentEndings.ElementAt(5), 50);

            // define the states
            states = new AbstractState[NUM_STATES];
            states[(Int32)PlayerCharacterState.Idle] = new IdleState(this);
            states[(Int32)PlayerCharacterState.Running] = new RunningState(this);
            states[(Int32)PlayerCharacterState.Jumping] = new JumpingState(this);
            states[(Int32)PlayerCharacterState.Hit] = new HitState(this);
            states[(Int32)PlayerCharacterState.JumpKicking] = new JumpKickState(this);
            states[(Int32)PlayerCharacterState.Punching] = new PunchState(this);
            states[(Int32)PlayerCharacterState.Kicking] = new KickState(this);
            states[(Int32)PlayerCharacterState.Blocking] = new BlockingState(this);
            states[(Int32)PlayerCharacterState.Charging] = new ChargingState(this);

            // start in Idle state
            switchState(PlayerCharacterState.Idle);

        }

        //defined by individual characters
        public virtual void charging()
        {
        }
        public virtual void charged()
        {
        }


        //get hit by other character
        public void getHit(SpriteEffects direction, int hitType)
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
                    currentHealth -= 10;
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
                    switchState(PlayerCharacterState.Hit);
                }
                if (hitType == HIT_TYPE_KICK)
                {
                    switchState(PlayerCharacterState.Idle);
                    currentHealth -= 6;
                }
                if (hitType == HIT_TYPE_PUNCH)
                {
                    switchState(PlayerCharacterState.Idle);
                    currentHealth -= 3;
                }
            }
            //if you are blocking AND facing the other player
            else if (isBlock && !direction.Equals(effects)){
                if (hitType == HIT_TYPE_JUMPKICK)
                {
                    currentHealth -= 3;
                    if (direction.Equals(SpriteEffects.None))
                    {
                        velocity.X += 100;
                    }
                    else
                    {
                        velocity.X -= 100;
                    }
                }
                if (hitType == HIT_TYPE_KICK)
                {
                    currentHealth -= 2;
                }
                if (hitType == HIT_TYPE_PUNCH)
                {
                    currentHealth -= 1;
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
                    if (gamepadState.ThumbSticks.Left.Y < 0)
                        inputDirection.Y -= gamepadState.ThumbSticks.Left.Y;
                }
                /* do we need to flip the image? */
                if (inputDirection.X < 0)
                    effects = SpriteEffects.FlipHorizontally;
                else if (inputDirection.X > 0)
                    effects = SpriteEffects.None;

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
                    otherPlayer.getHit(effects, HIT_TYPE_JUMPKICK);
                    isJumpKick = false;
                }
                else if (isKick)
                {
                    otherPlayer.getHit(effects, HIT_TYPE_KICK);
                    isKick = false;
                }
                else if (isPunch)
                {
                    otherPlayer.getHit(effects, HIT_TYPE_PUNCH);
                    isPunch = false;
                }
            }
        }

        /* Update */
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // call Update for the current state
            states[(Int32)currentState].Update(gameTime, clientBounds);

            //turn off SS effect if SS is off
            if (cancelSuper)
            {
                isSuper = false;
                chargeSoundInstance.Stop();
                chargeTimer = 0;
                superLoopInstance.Pause();
                cancelSuper = false;
            }

            // Update the sprite (base class)
            base.Update(gameTime, clientBounds);
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
                spriteSheet.setCurrentSegment((Int32)newState + 9);
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
                player.canJump = true;
                player.pauseAnimation = false;

                //idle->charge
                if (player.isSuper)
                {
                    player.currentFrame.Y = 5;
                }
                else
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
                        player.velocity.Y += -400f;
                    }
                }

                //idle->fall
                if (player.velocity.Y > 0)
                {
                    player.switchState(PlayerCharacterState.Jumping);
                    player.currentFrame.X = 8;
                }

                //idle->kick
                if (GamePad.GetState(player.pcPlayerNum).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(player.controller.pcPlayerKeys.ElementAt(5)))
                {
                    if (player.onGround)
                    {
                        player.isKick = true;
                        player.switchState(PlayerCharacterState.Kicking);
                    }
                }

                //idle->block
                if (GamePad.GetState(player.pcPlayerNum).Buttons.RightShoulder == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(player.controller.pcPlayerKeys.ElementAt(7)))
                {
                    player.switchState(PlayerCharacterState.Blocking);
                }
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
                if (player.isSuper)
                {
                    player.currentFrame.Y = 6;
                }
                //run->idle
                if (player.direction.X == 0 && player.direction.Y == 0)
                {
                    player.switchState(PlayerCharacterState.Idle);
                }

                //run->kick
                if (GamePad.GetState(player.pcPlayerNum).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(player.controller.pcPlayerKeys.ElementAt(5)))
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
                    player.velocity.Y += -400f;
                }
                //run->fall
                if (player.velocity.Y > 0)
                {
                    player.switchState(PlayerCharacterState.Jumping);
                    player.currentFrame.X = 8;
                }
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
                    player.jumpCount = 1;
                    player.switchState(PlayerCharacterState.Idle);
                }
                // animate once through -- then go to standing still frame
                if (player.currentFrame == player.spriteSheet.currentSegment.endFrame)
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
                        player.velocity.Y = -400f;
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
                if (player.currentFrame.X > 5 & player.onGround)
                {
                    player.velocity.X = 0;
                    player.velocity.Y = 0;
                }
                if (player.currentFrame.X < 4 && player.effects == SpriteEffects.None)
                {
                    player.velocity.X = -1500;
                }
                else if (player.currentFrame.X < 4 && player.effects == SpriteEffects.FlipHorizontally)
                {
                    player.velocity.X = 1500;
                }
                if (player.currentFrame.X >= 5 & !player.onGround)
                {
                    if (player.effects == SpriteEffects.None)
                    {
                        player.velocity.X = -400f;
                    }
                    else
                    {
                        player.velocity.X = 400f;
                    }
                    player.currentFrame.X = 4;
                }
                // animate once through -- then go to standing still frame
                if (player.currentFrame == player.spriteSheet.currentSegment.endFrame)
                {

                    player.velocity.X = 0f;
                    player.canMove = true;
                    player.canJump = true;
                    player.switchState(PlayerCharacterState.Idle);
                    player.currentFrame.X = player.pcSegmentEndings.ElementAt(2).X - 1;
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
                if (player.isSuper)
                {
                    player.currentFrame.Y = 8;
                }
                Point ssEndFrame = new Point(8, 8);
                if (player.currentFrame == player.spriteSheet.currentSegment.endFrame || player.currentFrame == ssEndFrame)
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
                if (player.isSuper)
                {
                    player.currentFrame.Y = 8;
                }
                Point ssEndFrame = new Point(8, 8);
                if (player.isPunch)
                {
                    if (player.effects == SpriteEffects.None)
                    {
                        player.velocity.X += 160f;

                    }
                    else
                    {
                        player.velocity.X += -160f;
                    }
                }
                if (player.onGround)
                {
                    player.velocity.Y += -20f;
                }
                if (player.currentFrame == player.spriteSheet.currentSegment.endFrame || player.currentFrame == ssEndFrame)
                {
                    player.isPunch = false;
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
                if (player.isSuper)
                {
                    player.currentFrame.Y = 8;
                }
                Point ssEndFrame = new Point(8, 8);
                if (player.isJumpKick)
                {
                    if (player.effects == SpriteEffects.None)
                    {
                        player.velocity.X += 300f;

                    }
                    else
                    {
                        player.velocity.X += -300f;
                    }
                }
                if (player.onGround)
                {
                    player.velocity.Y += -20f;
                }

                if (player.currentFrame.X + 2 >= player.spriteSheet.currentSegment.endFrame.X || player.currentFrame.X + 2 == ssEndFrame.X)
                {
                    player.isJumpKick = false;
                }
                if (player.currentFrame == player.spriteSheet.currentSegment.endFrame || player.currentFrame == ssEndFrame)
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
                    player.charged();
                }

                if (player.chargeTimer > chargeMax)
                {
                    player.chargeTimer = 0;
                    player.chargeSoundInstance.Stop(true);
                    player.switchState(PlayerCharacterState.Idle);
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
