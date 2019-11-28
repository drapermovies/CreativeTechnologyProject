using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Unity.Scenes
{
    [ExecuteAlways]
    public class SubScene : MonoBehaviour
    {
        #if UNITY_EDITOR
        [FormerlySerializedAs("sceneAsset")]
        [SerializeField] SceneAsset scene_asset;
        [SerializeField] Color hierachy_colour = Color.gray;

        static List<SubScene> sub_scenes = new List<SubScene>();
        public static IReadOnlyCollection<SubScene> all_sub_scenes { get { return sub_scenes; } }

        [NonSerialized] public LiveLinkScene live_link_data;
        #endif

        public bool AutoLoadScene = true;

        [NonSerialized] EntityManager scene_entity_manager;
        [NonSerialized] public List<Entity> scene_entities = new List<Entity>();

        //[SerializeField]
        //[HideInInspector]
        //SubSceneHeader sub_scene_header = null;
    }
}