using System;
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
       
        private  List<PlayerCharacter> playableCharacters = new List<PlayerCharacter>();
        private Game game;
        public FighterFactory(Game game)
        {
            this.game = game;
            playableCharacters.Add(new Goku(game.Content.Load<Texture2D>(@"Images/Goku"), game.Content.Load<SoundEffect>(@"Sound/Dragonball Z Charge Sound"), game.Content.Load<SoundEffect>(@"Sound/SSloop")));
            playableCharacters.Add(new Megaman(game.Content.Load<Texture2D>(@"Images/Megaman"), game.Content.Load<SoundEffect>(@"Sound/Dragonball Z Charge Sound"), game.Content.Load<SoundEffect>(@"Sound/SSloop")));
        }

        public PlayerCharacter selectCharacter(int CharID, PlayerPreset playerNum)
        {
            foreach (PlayerCharacter character in playableCharacters)
            {
                if (CharID == character.CHARACTER_ID)
                {                    
                    return character;
                }
            }
            return null;
        }
    }
}
