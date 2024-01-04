using System;
using System.Collections;
using Components;
using UnityEngine;

namespace Creatures.Weapons
{
    public class CircularProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private CircularProjectileSettings[] _settings;
        
        private SpawnComponent _spawnComponent;

        private void Awake()
        {
            _spawnComponent = GetComponent<SpawnComponent>();
        }

        public int Stage { get; set; }

        [ContextMenu("LaunchProjectiles")]
        public void LaunchProjectiles()
        {
            StartCoroutine(SpawnProjectiles());
        }

        private IEnumerator SpawnProjectiles()
        {
            var setting = _settings[Stage];
            var sectorStep = 2 * Mathf.PI / setting.Count;

            for (int i = 0; i < setting.Count; i++)
            {
                var angle = sectorStep * i;
                var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                var instantiate = _spawnComponent.Spawn(setting.Projectile.gameObject);
                var projectile = instantiate.GetComponent<ProjectileDirectional>();
                projectile.Launch(direction);

                yield return new WaitForSeconds(setting.Delay);
            }
        }
    }

    [Serializable]
    public struct CircularProjectileSettings
    {
        [SerializeField] private ProjectileDirectional _projectile;
        [SerializeField] private int _count;
        [SerializeField] private float _delay;

        public ProjectileDirectional Projectile => _projectile;

        public int Count => _count;

        public float Delay => _delay;
    }
}