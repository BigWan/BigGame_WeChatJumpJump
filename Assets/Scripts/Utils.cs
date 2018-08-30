using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtil {
    public enum Orientation {
        Forward = 0,
        Left = 1,
    }

    public static class Util {
        private static Vector3[] m_orientations = new Vector3[] {
            Vector3.forward,
            Vector3.left,
        };


        public static Vector3 GetOrientationVector(Orientation o) {
            return m_orientations[(int)o];
        }


    }
}
