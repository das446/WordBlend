using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InputMode
{
   void OnClick();
   void OnClick(Vector2 v);
   void OnClick(Tile t);
   void OnHover(Tile t);
   void Enter();
   void Exit();
   bool CanExit();
}
