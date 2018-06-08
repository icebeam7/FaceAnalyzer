using System;
using Microsoft.ProjectOxford.Face.Contract;

namespace FaceAnalyzer.Helpers
{
    public static class ImageAnalyzer
    {
        public static double AnalizarPostura(Face f)
        {
            double headPoseDeviation = Math.Abs(f.FaceAttributes.HeadPose.Yaw);
            double deviationRatio = f.FaceAttributes.HeadPose.Yaw / 35;

            return headPoseDeviation;
        }

        public static double AnalizarBoca(Face f)
        {
            double mouthWidth = Math.Abs(f.FaceLandmarks.MouthRight.X - f.FaceLandmarks.MouthLeft.X);
            double mouthHeight = Math.Abs(f.FaceLandmarks.UpperLipBottom.Y - f.FaceLandmarks.UnderLipTop.Y);

            double mouthAperture = mouthHeight / mouthWidth;
            mouthAperture = Math.Min((mouthAperture - 0.1) / 0.4, 1);

            return mouthAperture;
        }

        public static double AnalizarOjos(Face f)
        {
            double leftEyeWidth = Math.Abs(f.FaceLandmarks.EyeLeftInner.X - f.FaceLandmarks.EyeLeftOuter.X);
            double leftEyeHeight = Math.Abs(f.FaceLandmarks.EyeLeftBottom.Y - f.FaceLandmarks.EyeLeftTop.Y);

            double rightEyeWidth = Math.Abs(f.FaceLandmarks.EyeRightInner.X - f.FaceLandmarks.EyeRightOuter.X);
            double rightEyeHeight = Math.Abs(f.FaceLandmarks.EyeRightBottom.Y - f.FaceLandmarks.EyeRightTop.Y);

            double eyeAperture = Math.Max(leftEyeHeight / leftEyeWidth, rightEyeHeight / rightEyeWidth);
            eyeAperture = Math.Min((eyeAperture - 0.2) / 0.3, 1);

            return eyeAperture;
        }
    }
}
