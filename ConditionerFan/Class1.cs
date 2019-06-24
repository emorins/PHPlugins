using UnityEngine;
namespace UrbanTownScene
{
    public class ConditionerFan : MonoBehaviour
    {

        public float speed;

        void FixedUpdate()
        {
            transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
        }
    }
}
