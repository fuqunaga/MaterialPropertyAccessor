#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MaterialPropertyAccessor
{

    [ExecuteInEditMode]
    public class MaterialPropertyBehaviour : MonoBehaviour
    {
        #region TypeDefine

        [System.Serializable]
        public class PropertySet
        {
            [System.Serializable]
            public class RangeData
            {
                public string name;
                public float min;
                public float max;
            }

            public List<string> colors;
            public List<string> vectors;
            public List<string> floats;
            public List<RangeData> ranges;

            public void Clear()
            {
                colors.Clear();
                vectors.Clear();
                floats.Clear();
                ranges.Clear();
            }
        }

        #endregion

        public Material _material;
        public List<string> _ignoreProperties;
        public PropertySet _propertySet = new PropertySet();



        public virtual void Update()
        {
            UpdatePropertySet();
        }

        void UpdatePropertySet()
        {
#if UNITY_EDITOR
            _propertySet.Clear();
            if (_material != null)
            {
                var shader = _material.shader;
                var count = ShaderUtil.GetPropertyCount(shader);
                for (var i = 0; i < count; ++i)
                {
                    var name = ShaderUtil.GetPropertyName(shader, i);
                    if (!_ignoreProperties.Contains(name))
                    {
                        var type = ShaderUtil.GetPropertyType(shader, i);
                        switch (type)
                        {
                            case ShaderUtil.ShaderPropertyType.Color: _propertySet.colors.Add(name); break;
                            case ShaderUtil.ShaderPropertyType.Vector: _propertySet.vectors.Add(name); break;
                            case ShaderUtil.ShaderPropertyType.Float: _propertySet.floats.Add(name); break;
                            case ShaderUtil.ShaderPropertyType.Range:
                                {
                                    var rangeData = new PropertySet.RangeData()
                                    {
                                        name = name,
                                        min = ShaderUtil.GetRangeLimits(shader, i, 1),
                                        max = ShaderUtil.GetRangeLimits(shader, i, 2),
                                    };
                                    _propertySet.ranges.Add(rangeData);
                                }
                                break;

                                /* ignore texture */
                        }
                    }
                }
            }
#endif
        }
    }
}
