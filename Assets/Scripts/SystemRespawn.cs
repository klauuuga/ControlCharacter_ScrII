using UnityEngine;

public class SystemRespawn : MonoBehaviour
{
   public float threshold;

   void FixUpdate()
   {
      if (transform.position.y < threshold)
      {
         transform.position = new Vector3(232.5f, -36.1f, -159.56f);
      }
   }
}
