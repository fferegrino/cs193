
using System;

namespace FaceIt
{
    public enum Eyes
    {
        Open,
        Closed,
        Squinting
    }

    public enum EyeBrows
    {
        Relaxed,
        Normal,
        Furrowed
    }

    public enum Mouth
    {
        Frown,
        Smirk,
        Neutral,
        Grin,
        Smile
    }

    public struct FacialExpression
    {

        public Eyes Eyes { get; set; }
        public EyeBrows EyeBrows { get; set; }
        public Mouth Mouth { get; set; }
    }

    public static class FacialExpresionExtensions
    {
        public static EyeBrows MoreRelaxedBrow(this EyeBrows brow)
        {
            try
            {
                return (EyeBrows)(((int)brow) - 1);
            }
            catch
            {
                return EyeBrows.Relaxed;
            }
        }


        public static EyeBrows MoreFurrowedBrow(this EyeBrows brow)
        {
            try
            {
                return (EyeBrows)(((int)brow) + 1);
            }
            catch
            {
                return EyeBrows.Furrowed;
            }
        }

        public static Mouth SadderMouth(this Mouth mouth)
        {
            try
            {
                return (Mouth)(((int)mouth) - 1);
            }
            catch
            {
                return Mouth.Frown;
            }
        }

        public static Mouth HappierMouth(this Mouth mouth)
        {
            try
            {
                return (Mouth)(((int)mouth) + 1);
            }
            catch
            {
                return Mouth.Smile;
            }
        }
    }
}