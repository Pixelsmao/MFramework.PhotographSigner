﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Wing.uPainter;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Wing.uPainter
{
    [RequireComponent(typeof(RawImagePaintCanvas))]
    public class PaintRawImage : RawImage, IPointerClickHandler, IPointerUpHandler,
    IPointerDownHandler, IPointerExitHandler, IPointerEnterHandler
    {
        public bool EnablePaint = true;

        [SerializeField]
        Drawer _drawer = new Drawer();
        public Drawer Drawer
        {
            get
            {
                return _drawer;
            }
        }

        private RawImagePaintCanvas _paintCanvas;
        private Vector3 _lastMousePosition = Vector3.zero;

        public RawImagePaintCanvas PaintCanvas
        {
            get
            {
                return _paintCanvas;
            }
        }

        protected override void Awake()
        {
            _paintCanvas = GetComponent<RawImagePaintCanvas>();
            _drawer.Catch(_paintCanvas);

        }

        public void OnPointerClick(PointerEventData eventData)
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Input.GetMouseButton(0))
            {
                _drawer.Begin();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _drawer.End();
        }

        public void OnPointerMove(Vector3 pos)
        {
            if(!EnablePaint)
            {
                return;
            }

            if (!Input.GetMouseButton(0) && !_drawer.InPreview)
            {
                _drawer.End();
            }

            if (_drawer.InPreview)
            {
                _drawer.HoverMove(pos, pos);
            }
            else
            {
                _drawer.TouchMove(pos, pos);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_drawer.InPreview && Input.GetMouseButton(0))
            {
                _drawer.Begin();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _drawer.End();
        }

        private void Update()
        {
            _lastMousePosition = Input.mousePosition;

            OnPointerMove(_lastMousePosition);
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(PaintRawImage))]
        public class PaintRawImageEditor : Editor
        {
            protected SerializedObject InspectorObjects;
            protected SerializedProperty InspectorObjectsIterator;
            protected SerializedProperty _drawerProperty;

            void OnEnable()
            {
                InspectorObjects = new SerializedObject(target);
                _drawerProperty = InspectorObjects.FindProperty("_drawer");
            }

            public override void OnInspectorGUI()
            {
                InspectorObjectsIterator = InspectorObjects.GetIterator();
                PaintRawImage targetObject = (PaintRawImage)target;

                base.OnInspectorGUI();
                EditorGUILayout.PropertyField(_drawerProperty);

                InspectorObjects.ApplyModifiedProperties();
            }
        }
#endif
    }
}
