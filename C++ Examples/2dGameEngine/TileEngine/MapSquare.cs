using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine
{
    
    [Serializable]
    public class MapSquare
    {
        #region Declarations
        

        public int[] LayerTiles = new int[4];   
        public string CodeValue = "";           
        public bool Passable = true;            

        
        #endregion



        #region Constructor
        

        public MapSquare(int background, int interactive, int foreground, int parallax, string code, bool passable)
        {
            LayerTiles[0] = background;     
            LayerTiles[1] = interactive;    
            LayerTiles[2] = foreground;
            LayerTiles[3] = parallax;
            CodeValue = code;               
            Passable = passable;            
        }

        
        #endregion



        #region Public Methods
        

        public void TogglePassable()
        {
            Passable = !Passable;  
        }

        
        #endregion


    }
}
