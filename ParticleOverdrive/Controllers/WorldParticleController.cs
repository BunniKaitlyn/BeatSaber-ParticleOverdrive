﻿using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = ParticleOverdrive.Misc.Logger;

namespace ParticleOverdrive.Controllers
{
    public class WorldParticleController : MonoBehaviour, IGlobalController
    {
        private ParticleSystem _dustPS;
        public ParticleSystem DustPS
        {
            get
            {
                if (_dustPS == null)
                    _dustPS = Find();

                return _dustPS;
            }
        }

        private bool _enabled;
        public bool Enabled
        {
            get => _enabled;
            set
            {
                string action = value ? "Enabling" : "Disabling";
                Logger.Log($"{action} world particles!");

                _enabled = value;
            }
        }

        public void Init(bool state)
        {
            DontDestroyOnLoad(this);
            _enabled = state;
        }

        private ParticleSystem Find()
        {
            // This needs updating, as this doesn't find inactive Objects and the menu object persists now
            ParticleSystem[] pss = FindObjectsOfType<ParticleSystem>();
            return pss.Where(x => x.name == "DustPS").FirstOrDefault();
        }

        private void Set()
        {
            DustPS.gameObject.SetActive(_enabled);
        }

        public void OnSceneChange(Scene scene)
        {
            StartCoroutine(SceneChangeHandler());
        }

        private IEnumerator SceneChangeHandler()
        {
            _dustPS = null;

            while (_dustPS == null)
            {
                yield return new WaitForSeconds(0.5f);
                _dustPS = Find();
            }

            Logger.Log("Found new ParticleSystem!");

            Set();
        }
    }
}
