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
            LightAttack,
            HeavyAttack,
            Blocking,
            BlockHit,
            Hit,
            Knockdown,
            Charging
        }
        PlayerCharacterState currentState;
        AbstractState[] states;


        //sprite/sound variables
        static Point pcFrameSize;
        public List<Point> pcSegmentEndings = new List<Point>();
        public List<int> pcSegmentTimings = new List<int>();
        public SoundEffect chargeSound;
        public SoundEffect fireSound;
        public SoundEffect superLoop;
        public SoundEffectInstance chargeSoundInstance;
        public SoundEffectInstance fireSoundInstance;
        public SoundEffectInstance superLoopInstance;
        public bool hasChargeSound;
        public bool update = false;
        public bool flipped = false;
        public bool canSpecial = true;

        //player variables
        public PlayerIndex pcPlayerNum;
        public GamePadState oldGamePadState;
        public bool canAOE = false;
        public bool canBlock = false;
        public bool canFire = false;
        public bool canBeam = false;
        public bool canSuper = false;
        public bool canSmash = false;
        public bool canJump = false;
        public bool canJumpKick = false;
        public bool canJumpSpecial = false;
        public bool JKknockdown = false;
        public float JKvelocity = 400;
        public int chargeMax = 2500;


        //attack states
        public bool isAOE = false;
        public bool isSmash = false;
        public bool isBeam = false;
        public bool isFire = false;
        public bool isBlock = false;
        public bool isLGT = false;
        public bool isHVY = false;
        public bool isJumpKick = false;
        private const int HIT_TYPE_LIGHT = 0;
        private const int HIT_TYPE_HEAVY = 1;
        private const int HIT_TYPE_JUMPKICK = 2;
        private const int HIT_TYPE_BLAST = 3;
        public double CHARACTER_DAMAGE;

        //state variables
        public int currentHealth = 100;
        public int visibleHealth = 100;
        public bool canMove = true;
        public int jumpCount = 0;
        public int knockDownEndFrame;
        public int fireChargeFrame;
        public int fireFrame;
        public int chargeTimer = 0;
        public bool noSpam = false;
        public long pauseTime = 0;
        public long spamTimer = 0;
        public long disableTimer = 0;
        public bool isSuper = false;
        public bool hasFired = false;
        public bool hasReleased = false;
        public bool cancelSpecial = false;
        public bool chargePlayed = false;
        public bool disableProjectile = false;

        //character identification
        public int CHARACTER_ID;
        public String CHARACTER_NAME;


        // constructor
        public PlayerCharacter(SpriteSheet spriteSheet, CollisionOffset collisionOffset, CollisionOffset hitboxOffset, CollisionOffset hitboxOffsetFlipped, CollisionOffset hitboxOffsetNotFlipped,
            Vector2 speed, Vector2 friction, Point frameSize)
            : base(spriteSheet, collisionOffset, hitboxOffset, hitboxOffsetFlipped, hitboxOffsetNotFlipped, speed, friction)
        {
            update = false;
            pcFrameSize = frameSize;
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
            if (canSuper)
            {
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
                //superKnockdown
                spriteSheet.addSegment(pcFrameSize, new Point(0, 20), pcSegmentEndings.ElementAt(20), pcSegmentTimings.ElementAt(20));
                //superCharge
                spriteSheet.addSegment(pcFrameSize, new Point(0, 21), pcSegmentEndings.ElementAt(21), pcSegmentTimings.ElementAt(21));
            }
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
            states[(Int32)PlayerCharacterState.LightAttack] = new LightAttackState(this);
            states[(Int32)PlayerCharacterState.HeavyAttack] = new HeavyAttackState(this);
            states[(Int32)PlayerCharacterState.Charging] = new ChargingState(this);

            // start in Idle state
            switchState(PlayerCharacterState.Idle);

        }

        //defined by individual characters
        public virtual void spawn(PlayerPreset preset)
        {
        }
        public virtual void charging()
        {
        }
        public virtual void chargedOne()
        {
        }
        public virtual void chargedTwo()
        {
        }
        public virtual void cancelCharge()
        {
        }
        public virtual void heavyAttack()
        {
        }
        public virtual void lightAttack()
        {
        }
        public virtual void jumpKick()
        {
        }
        public virtual void special()
        {
        }
        public virtual void projTimer()
        {
        }

        public void stopChar()
        {
            if (isSuper)
            {
                superLoopInstance.Stop();
            }
            if (hasChargeSound)
            chargeSoundInstance.Stop();
            update = false;
        }
        public void pauseChar()
        {
            if (isSuper)
            {
                superLoopInstance.Pause();
            }
            if(hasChargeSound)
            chargeSoundInstance.Pause();
            update = false;
        }
        public void resumeChar()
        {
            if (currentHealth > 0)
            {
                if (isSuper)
                    superLoopInstance.Resume();
                if (currentState == PlayerCharacterState.Charging)
                    if (hasChargeSound)
                    chargeSoundInstance.Resume();
                update = true;
            }
            spamTimer += System.Environment.TickCount - pauseTime;
        }

        //get hit by other character
        public void getHit(SpriteEffects direction, int hitType, double oppDamage, bool otherCanKnockdown)
        {
            //if you aren't blocking or hit OR if you're facing the same direction(your back is to the other player)
            if ((canMove && !isBlock) || (direction.Equals(effects) && isBlock)|| currentState.Equals(PlayerCharacterState.Charging))
            {
                if (currentState.Equals(PlayerCharacterState.Charging))
                {
                    if (!isSuper)
                    {
                        cancelSpecial = true;
                    }
                }
                if (hitType == HIT_TYPE_BLAST)
                {
                    canMove = false;
                    velocity.Y = -100f;
                    currentHealth -= (int)(10 * oppDamage);
                    if (direction.Equals(SpriteEffects.None))
                    {
                        flipped = true;
                        effects = SpriteEffects.FlipHorizontally;
                        velocity.X = 1000f;
                    }
                    else
                    {
                        flipped = false;
                        effects = SpriteEffects.None;
                        velocity.X = -1000f;
                    }
                    switchState(PlayerCharacterState.Knockdown);
                }
                if (hitType == HIT_TYPE_JUMPKICK)
                {
                    velocity.Y = -100f;
                    currentHealth -= (int)(5 * oppDamage);
                    if (direction.Equals(SpriteEffects.None))
                    {
                        flipped = true;
                        effects = SpriteEffects.FlipHorizontally;
                        velocity.X = 1000f;
                    }
                    else
                    {
                        flipped = false;
                        effects = SpriteEffects.None;
                        velocity.X = -1000f;
                    }
                    jumpCount = 4;

                    if (otherCanKnockdown)
                    switchState(PlayerCharacterState.Knockdown);
                }
                if (hitType == HIT_TYPE_HEAVY)
                {
                    if (direction.Equals(SpriteEffects.None))
                    {
                        velocity.X += 1500;
                    }
                    else
                    {
                        velocity.X -= 1500;
                    }
                    switchState(PlayerCharacterState.Hit);
                    currentHealth -= (int)(8 * oppDamage);
                }
                if (hitType == HIT_TYPE_LIGHT)
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
                    currentHealth -= (int)(5 * oppDamage);
                }
                if (isAOE || isSmash || isBeam)
                {
                    cancelCharge();
                }
            }
            //if you are blocking AND facing the other player
            else if (isBlock && !direction.Equals(effects))
            {
                if (hitType == HIT_TYPE_BLAST)
                {
                    if (isAOE || isSmash)
                    {
                        chargedTwo();
                    }
                    canMove = false;
                    isBlock = false;
                    velocity.Y = -100f;
                    if (CHARACTER_ID != 4)
                    {
                        currentHealth -= (int)(10 * oppDamage);
                    }
                    else
                    {
                        currentHealth -= (int)(5 * oppDamage);
                    }
                    if (direction.Equals(SpriteEffects.None))
                    {
                        flipped = true;
                        effects = SpriteEffects.FlipHorizontally;
                        velocity.X = 1000f;
                    }
                    else
                    {
                        flipped = false;
                        effects = SpriteEffects.None;
                        velocity.X = -1000f;
                    }
                    switchState(PlayerCharacterState.Knockdown);
                }
                if (hitType == HIT_TYPE_JUMPKICK)
                {
                    if (isAOE || isSmash)
                    {
                        chargedTwo();
                    }
                    if (CHARACTER_ID != 4)
                        currentHealth -= (int)(2 * oppDamage);
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
                if (hitType == HIT_TYPE_HEAVY)
                {
                    if (isAOE || isSmash)
                    {
                        chargedTwo();
                    }
                    if (direction.Equals(SpriteEffects.None))
                    {
                        velocity.X += 100;
                    }
                    else
                    {
                        velocity.X -= 100;
                    } 
                    if (CHARACTER_ID != 4)
                    currentHealth -= (int)(2 * oppDamage);
                    switchState(PlayerCharacterState.BlockHit);
                }
                if (hitType == HIT_TYPE_LIGHT)
                {
                    if (isAOE || isSmash)
                    {
                        chargedTwo();
                    }
                    currentHealth -= (int)(1 * oppDamage);
                }
            }
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                if (isSuper)
                {
                    isSuper = false;
                    cancelSpecial = true;
                }
                switchState(PlayerCharacterState.Knockdown);
            }
        }

        /* Direction */
        public override Vector2 direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;
                if (canMove)
                {
                    /* gamepad input */
                    GamePadState gamepadState = GamePad.GetState(pcPlayerNum);
                    if (gamepadState.DPad.Left == ButtonState.Pressed)
                        inputDirection.X -= 1;
                    if (gamepadState.DPad.Right == ButtonState.Pressed)
                        inputDirection.X += 1;
                    if (gamepadState.ThumbSticks.Left.X != 0)
                        inputDirection.X += gamepadState.ThumbSticks.Left.X;
                }
                /* do we need to flip the image? */
                if (inputDirection.X < 0)
                {
                    effects = SpriteEffects.FlipHorizontally;
                }

                else if (inputDirection.X > 0)
                {
                    effects = SpriteEffects.None;
                }

                return inputDirection;
            }
        }

        /* Collision */
        public override void Collision(Sprite otherSprite)
        {
            if (otherSprite.isPlatform)
            {
                Platform platform = (Platform)otherSprite; 

                //This if statement is crazy but I can't make it work any other way. I don't understand.
                if (velocity.Y > 0 && position.Y + (this.spriteSheet.scale * (this.currentSegment.frameSize.Y - this.collisionOffset.south)) > platform.collisionRect.Top && this.collisionRect.Bottom + platform.collisionRect.Top > 2 && this.collisionRect.Bottom - 15 <= platform.collisionRect.Top)
                {
                    velocity.Y = -1;
                    onGround = true;
                    position.Y = platform.collisionRect.Top - spriteSheet.scale * (spriteSheet.currentSegment.frameSize.Y - collisionOffset.south) + 1;
                }
            }

            else if (otherSprite.isCharacter)
            {
                PlayerCharacter otherPlayer = (PlayerCharacter)otherSprite;
                if (otherPlayer.currentHealth > 0)
                {
                    if (isAOE || isSmash)
                    {
                        if (otherPlayer.position.X > position.X)
                        {
                            otherPlayer.getHit(SpriteEffects.None, HIT_TYPE_BLAST, CHARACTER_DAMAGE, JKknockdown);
                        }
                        else
                        {
                            otherPlayer.getHit(SpriteEffects.FlipHorizontally, HIT_TYPE_BLAST, CHARACTER_DAMAGE, JKknockdown);
                        }
                    }
                    if (isJumpKick)
                    {
                        velocity.X = 0f;
                        if (!otherPlayer.isBlock)
                        {
                            canJump = true;
                            jumpCount = 2;
                        }
                        if (otherPlayer.isJumpKick)
                        {
                            getHit(effects, HIT_TYPE_JUMPKICK, otherPlayer.CHARACTER_DAMAGE, JKknockdown);
                        }
                        otherPlayer.getHit(effects, HIT_TYPE_JUMPKICK, CHARACTER_DAMAGE, JKknockdown);
                        isJumpKick = false;
                    }
                    else if (isHVY)
                    {
                        otherPlayer.getHit(effects, HIT_TYPE_HEAVY, CHARACTER_DAMAGE, JKknockdown);
                        isHVY = false;
                    }
                    else if (isLGT)
                    {
                        otherPlayer.getHit(effects, HIT_TYPE_LIGHT, CHARACTER_DAMAGE, JKknockdown);
                        isLGT = false;
                    }
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
                if (currentHealth == 0)
                {
                    pauseTime = System.Environment.TickCount;
                    if (visibleHealth != 0)
                    {
                        visibleHealth--;
                    }
                }
            }
            else
            {
                projTimer();
                pauseTime = System.Environment.TickCount;

                noSpam = false;
                if ((canAOE || canSmash || canBeam) && pauseTime < spamTimer)
                {
                    noSpam = true;
                }

                if (currentHealth < visibleHealth)
                {
                    visibleHealth--;
                }

                if (effects == SpriteEffects.FlipHorizontally)
                {
                    flipped = true;
                    hitboxOffset = hitboxOffsetFlipped;
                }
                else
                {
                    flipped = false;
                    hitboxOffset = hitboxOffsetNotFlipped;
                }

                // call Update for the current state
                states[(Int32)currentState].Update(gameTime, clientBounds);

                
                if (cancelSpecial)
                {
                    if (canSuper)
                    {
                        CHARACTER_DAMAGE /= 1.2;
                        superLoopInstance.Pause();
                        chargeSoundInstance.Stop();
                        
                    }
                    isSuper = false;
                    
                    chargeTimer = 0;
                    
                    cancelSpecial = false;
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
            currentSegment = spriteSheet.setCurrentSegment((Int32)newState);
            if (isSuper)
            {
                currentSegment = spriteSheet.setCurrentSegment((Int32)newState + 11);
            }
            currentFrame = currentSegment.startFrame;
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
            }

            public override void Update(GameTime gameTime, Rectangle clientBounds)
            {
                player.jumpCount = 1;
                player.canJump = true;
                player.pauseAnimation = false;
                //idle->charge
                if (!player.noSpam && player.canSpecial)
                {
                    if ((player.oldGamePadState.Buttons.Y == ButtonState.Released && GamePad.GetState(player.pcPlayerNum).Buttons.Y == ButtonState.Pressed))
                    {
                        player.switchState(PlayerCharacterState.Charging);
                    }
                }

                //idle->run
                if (player.direction.X != 0)
                {
                    player.switchState(PlayerCharacterState.Running);
                }

                //idle->jump
                if (GamePad.GetState(player.pcPlayerNum).Buttons.A == ButtonState.Pressed)
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
                if ((player.oldGamePadState.Buttons.B == ButtonState.Released && GamePad.GetState(player.pcPlayerNum).Buttons.B == ButtonState.Pressed))
                {
                    if (player.onGround)
                    {
                        player.isHVY = true;
                        player.switchState(PlayerCharacterState.HeavyAttack);
                    }
                }
                //idle->punch
                if ((player.oldGamePadState.Buttons.X == ButtonState.Released && GamePad.GetState(player.pcPlayerNum).Buttons.X == ButtonState.Pressed))
                {
                    if (player.onGround)
                    {
                        player.isLGT = true;
                        player.switchState(PlayerCharacterState.LightAttack);
                    }
                }

                //idle->block
                if (GamePad.GetState(player.pcPlayerNum).Buttons.RightShoulder == ButtonState.Pressed)
                {
                    if (player.canBlock == true)
                    {
                        player.switchState(PlayerCharacterState.Blocking);
                    }
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
                //run->charge
                if (!player.noSpam && player.canSpecial)
                {
                    if ((player.oldGamePadState.Buttons.Y == ButtonState.Released && GamePad.GetState(player.pcPlayerNum).Buttons.Y == ButtonState.Pressed))
                    {
                        player.switchState(PlayerCharacterState.Charging);
                    }
                }
                //run->punch
                if ((player.oldGamePadState.Buttons.X == ButtonState.Released && GamePad.GetState(player.pcPlayerNum).Buttons.X == ButtonState.Pressed))
                {
                    if (player.onGround)
                    {
                        player.isLGT = true;
                        player.switchState(PlayerCharacterState.LightAttack);
                    }
                }
                //run->kick
                if ((player.oldGamePadState.Buttons.B == ButtonState.Released && GamePad.GetState(player.pcPlayerNum).Buttons.B == ButtonState.Pressed))
                {
                    player.isHVY = true;
                    player.switchState(PlayerCharacterState.HeavyAttack);
                }

                //run->jump
                if (GamePad.GetState(player.pcPlayerNum).Buttons.A == ButtonState.Pressed)
                {
                    player.jumpCount++;
                    player.canJump = false;
                    player.switchState(PlayerCharacterState.Jumping);
                    player.velocity.Y += -500f;
                }
                //run->fall
                if (player.velocity.Y > 0)
                {
                    player.jumpCount = 2;
                    player.switchState(PlayerCharacterState.Jumping);
                    player.currentFrame.X = player.pcSegmentEndings.ElementAt(2).X - 1;
                }

                //run->block
                if (GamePad.GetState(player.pcPlayerNum).Buttons.RightShoulder == ButtonState.Pressed)
                {
                     if (player.canBlock == true)
                     {
                
                    player.switchState(PlayerCharacterState.Blocking);
                     }
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
                if (player.velocity.X >= 250)
                {
                    player.velocity.X -= 50;
                }
                if (player.velocity.X < -250)
                {
                    player.velocity.X += 50;
                }
                if (player.onGround)
                {
                    player.switchState(PlayerCharacterState.Idle);
                }
                //jump->charge
                if (player.jumpCount < 4 && !player.noSpam && player.canSpecial && player.canJumpSpecial)
                {
                    if ((player.oldGamePadState.Buttons.Y == ButtonState.Released && GamePad.GetState(player.pcPlayerNum).Buttons.Y == ButtonState.Pressed))
                    {
                        player.jumpCount = 4;
                        player.switchState(PlayerCharacterState.Charging);
                    }
                }
                // animate once through -- then go to falling frame
                if (player.currentFrame.X == player.currentSegment.endFrame.X)
                {
                    player.currentFrame.X = player.pcSegmentEndings.ElementAt(2).X - 1;
                }
                if (GamePad.GetState(player.pcPlayerNum).Buttons.A == ButtonState.Released)
                {
                    player.canJump = true;
                }
                //quickfall (not implemented yet... still working on it)
                //if (GamePad.GetState(player.pcPlayerNum).Buttons.RightShoulder == ButtonState.Pressed)
                //{
                //}

                //air block
                if (GamePad.GetState(player.pcPlayerNum).Buttons.RightShoulder == ButtonState.Pressed)
                {
                    if (player.canBlock == true)
                    {
                        player.jumpCount = 4;
                        player.velocity.X = 0;
                        player.isBlock = true;
                    }
                }
                else
                {
                    player.isBlock = false;
                }

                player.oldGamePadState = GamePad.GetState(player.pcPlayerNum);
                if (player.jumpCount <= 2 && player.canJump)
                {
                    if (GamePad.GetState(player.pcPlayerNum).Buttons.A == ButtonState.Pressed)
                    {
                        System.Diagnostics.Debug.WriteLine("jumpcount is " + player.jumpCount);
                        player.jumpCount++;
                        player.canJump = false;
                        player.velocity.Y = -500f;
                        player.switchState(PlayerCharacterState.Jumping);
                    }
                }
                if (player.jumpCount < 4 && player.canJumpKick)
                {
                    if (GamePad.GetState(player.pcPlayerNum).Buttons.B == ButtonState.Pressed)
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
                player.jumpCount = 4;

                if ((player.currentFrame.X < player.knockDownEndFrame - 2 && !player.flipped))
                {
                    player.velocity.X = -1700;
                }
                else if (player.currentFrame.X < player.knockDownEndFrame - 2 && player.flipped)
                {
                    player.velocity.X = 1700;
                }

                if (player.currentFrame.X >= player.knockDownEndFrame - 1 & !player.onGround)
                {
                    if (player.flipped)
                    {
                        player.velocity.X = -250f;
                    }
                    else
                    {
                        player.velocity.X = 250f;
                    }
                    player.currentFrame.X = player.knockDownEndFrame - 1;
                }

                if (player.currentFrame.X > player.knockDownEndFrame - 1 & player.onGround)
                {
                    player.velocity.X = 0;
                    player.velocity.Y = 0;
                }

                if (player.currentHealth <= 0 && player.currentFrame.X == player.knockDownEndFrame && player.onGround)
                {
                    player.currentFrame.X = player.knockDownEndFrame;
                    player.update = false;
                }

                if (player.currentFrame.X >= player.currentSegment.endFrame.X && player.currentHealth > 0)
                {

                    player.velocity.X = 0f;
                    player.canMove = true;
                    player.canJump = true;
                    player.switchState(PlayerCharacterState.Idle);
                }
            }
        }
        /* BlockHit State */
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
                if (player.currentFrame.X == player.currentSegment.endFrame.X && GamePad.GetState(player.pcPlayerNum).Buttons.RightShoulder == ButtonState.Pressed)
                {
                    player.velocity.X = 0f;
                    player.switchState(PlayerCharacterState.Blocking);
                }
                if (player.currentFrame.X == player.currentSegment.endFrame.X && GamePad.GetState(player.pcPlayerNum).Buttons.RightShoulder == ButtonState.Released)
                {
                    player.isBlock = false;
                    player.canJump = true;
                    player.canMove = true;
                    player.switchState(PlayerCharacterState.Idle);
                }
            }
        }
        
        /* Hit State */
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

                if (player.currentFrame.X == player.currentSegment.endFrame.X)
                {
                    player.canMove = true;
                    player.velocity.X = 0f;
                    if (player.currentFrame.X == player.currentSegment.endFrame.X && player.onGround)
                    {
                        player.canJump = true;
                        player.switchState(PlayerCharacterState.Idle);
                    }
                    else if (player.currentFrame.X == player.currentSegment.endFrame.X && !player.onGround)
                    {
                        player.switchState(PlayerCharacterState.Jumping);
                        player.currentFrame.X = player.pcSegmentEndings.ElementAt(2).X - 1;
                    }
                }
            }
        }

        /* Heavy Attack State */
        private class HeavyAttackState : AbstractState
        {
            
            public HeavyAttackState(PlayerCharacter player)
                : base(player)
            {
            }

            public override void Update(GameTime gameTime, Rectangle clientBounds)
            {
                player.canMove = false;
                player.heavyAttack();
                if (player.currentFrame.X == player.currentSegment.endFrame.X)
                {
                    player.canMove = true;
                    player.isHVY = false;
                    player.switchState(PlayerCharacterState.Idle);
                }
            }
        }

        /* Light Attack State */
        private class LightAttackState : AbstractState
        {

            public LightAttackState(PlayerCharacter player)
                : base(player)
            {
            }

            public override void Update(GameTime gameTime, Rectangle clientBounds)
            {
                player.canMove = false;
                player.lightAttack();
                player.velocity.X = 0;
                if (player.currentFrame == player.currentSegment.endFrame)
                {
                    player.canMove = true;
                    player.isLGT = false;
                    player.switchState(PlayerCharacterState.Idle);
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
                player.jumpKick();
                player.canMove = false;
                if (player.isJumpKick)
                {
                    if (player.effects == SpriteEffects.None)
                    {
                        player.velocity.X += player.JKvelocity;
                    }
                    else
                    {
                        player.velocity.X -= player.JKvelocity;
                    }
                }
                if (player.currentFrame.X + 2 >= player.currentSegment.endFrame.X)
                {
                    player.isJumpKick = false;
                }

                if (player.currentFrame == player.currentSegment.endFrame )
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
                player.charging();
                if (player.canFire && GamePad.GetState(player.pcPlayerNum).Buttons.Y == ButtonState.Released)
                {
                    player.hasReleased = true;
                }
                else
                {
                    player.hasReleased = false;
                }
                /////
                if (GamePad.GetState(player.pcPlayerNum).Buttons.Y == ButtonState.Pressed || player.canFire || player.canSmash || player.canAOE)
                {
                    player.chargeTimer += gameTime.ElapsedGameTime.Milliseconds;
                    player.canMove = false;
                }
                else
                {
                    player.canMove = true;
                    //charge->fall
                    if (!player.onGround)
                    {
                        player.switchState(PlayerCharacterState.Jumping);
                        player.currentFrame.X = player.pcSegmentEndings.ElementAt(2).X - 1;
                    }
                    else
                    {
                        player.switchState(PlayerCharacterState.Idle);
                    }
                    player.chargeSoundInstance.Stop(true);
                    player.cancelSpecial = true;
                }

                if ((player.chargeTimer + 200) > player.chargeMax)
                {
                    player.chargedOne();
                }
                if (player.canFire && !player.hasFired)
                {
                    if ((player.currentFrame.X > player.fireChargeFrame - 2 && ((player.chargeTimer + 200) < player.chargeMax)) ||
                        (GamePad.GetState(player.pcPlayerNum).Buttons.Y == ButtonState.Pressed && !player.hasReleased && player.currentFrame.X > player.fireChargeFrame - 2))
                    {
                        player.currentFrame.X = player.fireChargeFrame - 1;
                    }
                    if (player.currentFrame.X >= player.fireFrame && player.chargeTimer > player.chargeMax && GamePad.GetState(player.pcPlayerNum).Buttons.Y == ButtonState.Released && player.hasReleased)
                    {
                        player.chargedTwo();
                    }
                }

                if (player.hasFired && player.hasReleased && player.currentFrame.X >= (player.pcSegmentEndings.ElementAt(player.isSuper ? 21 : 10).X - 2) && GamePad.GetState(player.pcPlayerNum).Buttons.Y == ButtonState.Released)
                {
                    player.hasReleased = false;
                    player.hasFired = false;
                    player.chargeTimer = 0;
                    player.canMove = true;
                    player.chargePlayed = false;

                    if (!player.onGround)
                    {
                        player.switchState(PlayerCharacterState.Jumping);
                        player.currentFrame.X = player.pcSegmentEndings.ElementAt(player.isSuper ? 13 : 2).X - 1;
                    }
                    else
                    {
                        player.switchState(PlayerCharacterState.Idle);
                    }
                } 
                if (player.chargeTimer < player.chargeMax && player.currentFrame.X >= player.currentSegment.endFrame.X - 1 &&!player.canFire)
                {
                    player.currentFrame.X = player.currentSegment.endFrame.X - 3;
                }
                if (player.chargeTimer > player.chargeMax && !player.canFire)
                {
                    player.chargedTwo();
                    player.chargePlayed = false;
                    player.hasReleased = false;
                    player.chargeTimer = 0;
                    player.canMove = true;
                    //charge->fall
                    if (!player.onGround)
                    {
                        player.switchState(PlayerCharacterState.Jumping);
                        player.currentFrame.X = player.pcSegmentEndings.ElementAt(player.isSuper ? 13 : 2).X - 1;
                    }
                    else
                    {
                        player.switchState(PlayerCharacterState.Idle);
                    }
                }
                player.oldGamePadState = GamePad.GetState(player.pcPlayerNum);
            }
        }
       
        /* Blocking State */
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
               
                    if (GamePad.GetState(player.pcPlayerNum).Buttons.RightShoulder == ButtonState.Released)
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
