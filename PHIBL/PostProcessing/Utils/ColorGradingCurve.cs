using System;
using UnityEngine;
using MessagePack;
namespace PHIBL.PostProcessing
{
    // Small wrapper on top of AnimationCurve to handle zero-key curves and keyframe looping
    [MessagePackObject]
    [Serializable]
    public sealed class ColorGradingCurve
    {
        [Key("curve")]
        public AnimationCurve curve;
        [Key("loop")]
        [SerializeField]
        public bool m_Loop;
        [Key("zeroValue")]
        [SerializeField]
        public float m_ZeroValue;
        [Key("range")]
        [SerializeField]
        public float m_Range;

        AnimationCurve m_InternalLoopingCurve;
        [SerializationConstructor]
        public ColorGradingCurve(AnimationCurve curve, float zeroValue, bool loop, float range)
        {
            this.curve = curve;
            m_ZeroValue = zeroValue;
            m_Loop = loop;
            m_Range = range;
        }
        public ColorGradingCurve(AnimationCurve curve, float zeroValue, bool loop, Vector2 bounds)
        {
            this.curve = curve;
            m_ZeroValue = zeroValue;
            m_Loop = loop;
            m_Range = bounds.magnitude;
        }

        public void Cache()
        {
            if (!m_Loop)
                return;

            var length = curve.length;

            if (length < 2)
                return;

            if (m_InternalLoopingCurve == null)
                m_InternalLoopingCurve = new AnimationCurve();

            var prev = curve[length - 1];
            prev.time -= m_Range;
            var next = curve[0];
            next.time += m_Range;
            m_InternalLoopingCurve.keys = curve.keys;
            m_InternalLoopingCurve.AddKey(prev);
            m_InternalLoopingCurve.AddKey(next);
        }

        public float Evaluate(float t)
        {
            if (curve.length == 0)
                return m_ZeroValue;

            if (!m_Loop || curve.length == 1)
                return curve.Evaluate(t);

            return m_InternalLoopingCurve.Evaluate(t);
        }
    }
}
