﻿using SchiffeVersenken.Data.Sea;

namespace SchiffeVersenken.Data.View

{
    public class BattlefieldPlayer: Battlefield
    {
        public BattlefieldPlayer(int size) : base(size)
        {
            _size = size;
            CreateField();
        }
    }
}
