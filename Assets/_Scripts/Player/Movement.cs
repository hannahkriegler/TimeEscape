using UnityEngine;

namespace TE
{
    public class Movement
    {
        private Player _player;
         private Game _game;
 
         public Movement(Player player, Game game)
         {
             _player = player;
             _game = game;
         }
         
         public void Move(Vector2 direction)
         {
             //TODO-Philip Move Character left and Right
             Debug.Log("Moved in direction: " + direction);
         }
 
         public void Jump()
         {
             //TODO-Philip Jump
             Debug.Log("Jumped!");
         }
 
         public void Dash()
         {
             if (_game.session.IsDashUnlocked())
             {
                 Debug.Log("Player dashed!");
             }
         }
     }
 }