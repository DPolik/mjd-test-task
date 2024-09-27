using System;
using System.Collections.Generic;

public interface IGameBoardView
{

    void Init(List<Layer> layers, int cellCount, int cellTypesAmount);

    void Reset(List<Layer> layers);

    void RotateBoard(int hDirection, Action onFinishRotationCallback = null);

}

