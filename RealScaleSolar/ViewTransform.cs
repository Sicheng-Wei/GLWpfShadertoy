using System;

namespace RealScaleSolar
{
    internal class MathLib 
    {
        public static readonly float PI = 3.14159265359f;
    }

    internal class ViewTransform
    {
        private static float stepDT;
        public static void StepTangent(float[] iSite, GLSpherAngle iTang, bool iPositive)
        {
            stepDT = iPositive ? MainWindow.DT : -MainWindow.DT;
        }

        public static void StepNormal(float[] iSite, GLSpherAngle iNorm, bool iPositive)
        {
            stepDT = iPositive ? MainWindow.DT : -MainWindow.DT;
        }
        
        public static void StepRotate(GLSpherAngle iNorm, bool iPositive)
        {
        }

        public static void StepBinormal(float[] iSite, GLSpherAngle iBiNorm, bool iPositive)
        {
        }
    }
}
