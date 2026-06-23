using System;

[Flags]
public enum TargetType
{
    None = 0,

    Self = 1,

    Ally = 2,

    Enemy = 4,

    Neutral = 8
}