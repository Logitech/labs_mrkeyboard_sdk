//---------------------------------------------------------------------------
//
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//
//---------------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;

// D3D convention (right hand) (trackable devices (e.g. keyboard), when put down on desk, facing up): 
//
//                   |y  
//                   |  
//                   | 
//                   --------x
//                  /
//                 /
//                /z

// Unity coordinate system and object in Unity:        pose in Unity :
// (Unity is left handed system)
//     
//                      |y                          y(up)|   / z(forward)
//                      |                                |  /        
//                      |                                | /         
//              x--------                                --------x(right)
//                     /
//                    /
//                   /z


namespace MrKeyboard.Hands
{
    class QuadTracker
    {
        // Note: matrix initilization are column-based, so it's transpose of what's seen visually.

        // reverse z axis
        public readonly static Matrix4x4 devicePoseD3dToDevicePoseUnity =
            new Matrix4x4(new Vector4(1.0f, 0.0f, 0.0f, 0.0f),
                          new Vector4(0.0f, 1.0f, 0.0f, 0.0f),
                          new Vector4(0.0f, 0.0f, -1.0f, 0.0f),
                          new Vector4(0.0f, 0.0f, 0.0f, 1.0f));

        // reverse x axis
        public readonly static Matrix4x4 objectUnityToObjectD3d =
            new Matrix4x4(new Vector4(-1.0f, 0.0f, 0.0f, 0.0f),
                          new Vector4(0.0f, 1.0f, 0.0f, 0.0f),
                          new Vector4(0.0f, 0.0f, 1.0f, 0.0f),
                          new Vector4(0.0f, 0.0f, 0.0f, 1.0f));

        // Object's transform relative to the device's holding pose, as defined in CAD model
        private Matrix4x4 objectD3dToDevicePoseD3d = Matrix4x4.identity;

        private Vector4[] objectCornersInObjectD3d = new Vector4[4];

        public void GetQuad(
                out Vector3 quadPosition,
                out Quaternion quadOrientation,
                out Vector3 quadScale,
                Vector3 devicePosition,
                Quaternion deviceOrientation)
        {
            List<Vector3> cornerPoints = new List<Vector3>();
            quadPosition = new Vector3();
            quadOrientation = new Quaternion();
            quadScale = new Vector3();

            // 1. Convert device's position and orientation to matrix.
            Matrix4x4 devicePoseUnityToWorldUnity = Matrix4x4.TRS(devicePosition, deviceOrientation, new Vector3(1.0f, 1.0f, 1.0f));

            // 2. Get KB's corner points in world coordinate frame.
            Matrix4x4 objectD3dToWorldUnity = devicePoseUnityToWorldUnity * devicePoseD3dToDevicePoseUnity * objectD3dToDevicePoseD3d;
            for (int i = 0; i < 4; ++i)
            {
                cornerPoints.Add(objectD3dToWorldUnity * objectCornersInObjectD3d[i]);
            }

            // 4. Get KB's directions
            Vector3 objectForwardDirection = Vector3.Normalize(cornerPoints[1] - cornerPoints[3]);
            Vector3 objectRightDirection = Vector3.Normalize(cornerPoints[1] - cornerPoints[0]);
            Vector3 objectDownDirection = Vector3.Normalize(Vector3.Cross(objectRightDirection, objectForwardDirection));

            // 5. Get quad position and orientation
            quadPosition = (cornerPoints[0] + cornerPoints[3]) / 2;
            quadOrientation.SetLookRotation(objectDownDirection, objectForwardDirection);

            // 6. Get quad scale
            float horizontalLength = (cornerPoints[1] - cornerPoints[0]).magnitude;
            float verticalLength = (cornerPoints[2] - cornerPoints[0]).magnitude;
            quadScale = new Vector3(horizontalLength, verticalLength, 1.0f);
        }

        public Matrix4x4 GetObjectUnityToDevicePoseUnity()
        {
            return devicePoseD3dToDevicePoseUnity * objectD3dToDevicePoseD3d * objectUnityToObjectD3d;
        }

        public void SetObjectCornersInObjectD3d(Vector4[] vecIn)
        {
            objectCornersInObjectD3d = vecIn;
        }
    }
}
