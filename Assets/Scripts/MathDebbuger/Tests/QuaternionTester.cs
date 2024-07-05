using CustomMath;
using UnityEngine;

namespace MathDebbuger.Tests
{
    enum QuaternionExcercise
    {
        One,
        Two,
        Three
    }
    
    public class QuaternionTester : MonoBehaviour
    {
        [SerializeField] private QuaternionExcercise excerciseNumber;
        [SerializeField] private float angle;
        [SerializeField] private Vec3 aVectorValues = new Vec3(10, 0, 0);

        private Vec3 _vector;
        private const string VectorIdentifier = "vector1";
        
        void Start()
        {
            AssignVectors();
        }
        
        private void OnValidate()
        {
            if(Application.isPlaying)
                UpdateVectors();
        }
        
        void Update()
        {
            switch (excerciseNumber)
            {
                case QuaternionExcercise.One:
                    MyQuaternion rotation = MyQuaternion.AngleAxis(angle, Vec3.Up);
                    _vector = rotation * _vector;
                    
                    UpdateVectorPositions();
                    break;
            }
        }

        void AssignVectors()
        {
            _vector = aVectorValues;
            
            Vector3Debugger.AddVector(_vector, Color.green, VectorIdentifier);
            Vector3Debugger.EnableEditorView();
        }

        private void UpdateVectors()
        {
            _vector = new Vec3(aVectorValues.x, aVectorValues.y, aVectorValues.z);
            
            UpdateVectorPositions();
        }

        private void UpdateVectorPositions()
        {
            Vector3Debugger.UpdatePosition(VectorIdentifier, _vector);
        }
    }
}
