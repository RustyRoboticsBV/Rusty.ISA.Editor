using System;

namespace Rusty.ActionGraph.Serialization;

public struct BinaryAttributes
{
    /* Fields. */
    private string _0;
    private string _1;
    private string _2;
    private string _3;
    private string _4;
    private string _5;
    private string _6;
    private string _7;

    /* Indexers. */
    public string this[int index]
    {
        readonly get => index switch
        {
            0 => _0,
            1 => _1,
            2 => _2,
            3 => _3,
            4 => _4,
            5 => _5,
            6 => _6,
            7 => _7,
            _ => throw new IndexOutOfRangeException(nameof(index))
        };
        set
        {
            switch (index)
            {
                case 0: _0 = value; break;
                case 1: _1 = value; break;
                case 2: _2 = value; break;
                case 3: _3 = value; break;
                case 4: _4 = value; break;
                case 5: _5 = value; break;
                case 6: _6 = value; break;
                case 7: _7 = value; break;
                default: throw new IndexOutOfRangeException(nameof(index));
            }
        }
    }

    /* Public methods. */
    public byte GetBitmask()
    {
        byte mask = 0;
        for (int i = 0; i < 8; i++)
        {
            mask = Bitmask.SetBit(mask, i, this[i] != null);
            Godot.GD.Print(i + " - " + (this[i] != null));
        }
        Godot.GD.Print("result: " + mask);
        return mask;
    }
}
