using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InputMode
{
   void OnClick();
   void OnClick(Vector2 v);
   void OnClick(Tile t, Board board);
   void Enter();
   void Exit();
}
