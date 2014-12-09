﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace UltraBrawl
{
    class FighterFactory
    {

        private Game game;
        public FighterFactory(Game game)
        {
            this.game = game;
        }

        public PlayerCharacter selectCharacter(int CharID)
        {
            if (CharID == 0)
            {
                return new Goku(game.Content.Load<Texture2D>(@"Images/Goku"), game.Content.Load<SoundEffect>(@"Sound/Dragonball Z Charge Sound"), game.Content.Load<SoundEffect>(@"Sound/SSloop"));
            }
            else if (CharID == 1)
            {
                return new Megaman(game.Content.Load<Texture2D>(@"Images/Megaman"), game.Content.Load<SoundEffect>(@"Sound/Mega Man Charging"), game.Content.Load<SoundEffect>(@"Sound/Mega Man Charging"));
            }
            else if (CharID == 2)
            {
                return new Ryu(game.Content.Load<Texture2D>(@"Images/Ryu"), game.Content.Load<SoundEffect>(@"Sound/Dragonball Z Charge Sound"), game.Content.Load<SoundEffect>(@"Sound/SSloop"));
            }
            else if (CharID == 3)
            {
                return new Guile(game.Content.Load<Texture2D>(@"Images/Guile"), game.Content.Load<SoundEffect>(@"Sound/Dragonball Z Charge Sound"), game.Content.Load<SoundEffect>(@"Sound/SSloop"));
            }
            else if (CharID == 4)
            {
                return new Venom(game.Content.Load<Texture2D>(@"Images/Venom"), game.Content.Load<SoundEffect>(@"Sound/Venom Laugh"), game.Content.Load<SoundEffect>(@"Sound/SSloop"));
            }
            else if (CharID == 5)
            {
                return new Zero(game.Content.Load<Texture2D>(@"Images/Zero"), game.Content.Load<SoundEffect>(@"Sound/Lazer Sword"), game.Content.Load<SoundEffect>(@"Sound/SSloop"));
            }
            else if (CharID == 6)
            {
                return new Kazuya(game.Content.Load<Texture2D>(@"Images/Kazuya"), game.Content.Load<SoundEffect>(@"Sound/Dragonball Z Charge Sound"), game.Content.Load<SoundEffect>(@"Sound/SSloop"));
            }
            return null;
        }
    }
}
