using UnityEngine;

namespace FirstARPG.Saving
{
    /// <summary>
    /// 可序列化vector3
    /// </summary>
    [System.Serializable]
    public class SerializableVector3
    {
        float x, y, z;
        
        public SerializableVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }
        
        public Vector3 ToVector()
        {
            return new Vector3(x, y, z);
        }
    }
}