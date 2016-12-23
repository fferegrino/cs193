
using System;
using System.Linq;

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
            var newValue = (((int)brow) - 1);
            return (EyeBrows)Math.Max(0, newValue);
        }


        public static EyeBrows MoreFurrowedBrow(this EyeBrows brow)
        {
            var vals = Enum.GetValues(typeof(EyeBrows)) as int[];
            return (EyeBrows)Math.Min((((int)brow) + 1), vals.Max());
        }

        public static Mouth SadderMouth(this Mouth mouth)
        {
            var newValue = (((int)mouth) - 1);
            return (Mouth)Math.Max(0, newValue);
        }

        public static Mouth HappierMouth(this Mouth mouth)
        {
            var vals = Enum.GetValues(typeof(Mouth)) as int[];
            return (Mouth)Math.Min((((int)mouth) + 1), vals.Max());
        }
    }
}