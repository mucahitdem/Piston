using Core;

namespace PistonSimulation
{
    public static class Defs
    {
       public static readonly MinMax XVal = new MinMax(-2,2);
       public static readonly MinMax YVal = new MinMax(3.6f, 6f);
       public static readonly float FIXED_Z_POS = 4.5f;

       public static readonly string SHADER_COLOR_KEY = "_Color";
    }
}