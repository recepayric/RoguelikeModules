using System;
using System.Collections.Generic;
using Runtime.Data.ValueObject;
using Runtime.Model.Interfaces;
using Sirenix.OdinInspector;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Views.Minimap
{
    public class MinimapMediator : Mediator
    {
        [Inject] public MinimapView view { get; set; }
        [Inject] public IMinimapModel MinimapModel { get; set; }

        public float minimapSizeScale = 10;
        public float minimapIconScale = 1;

        private MinimapObjectVO playerObjectVO;

        private List<MinimapObjectVO> enemyList;

        [Button]
        public void CreateBases()
        {
            var allObjects = MinimapModel.MinimapObjectVos;
            var bases = allObjects.FindAll(t => t.type == MinimapObjectType.Base);

            foreach (var baseObject in view.baseObjects)
            {
                baseObject.gameObject.SetActive(false);
            }
            
            for (int i = 0; i < bases.Count; i++)
            {
                view.baseObjects[i].gameObject.SetActive(true);
                var pos = bases[i].minimapObject.transform.position * minimapSizeScale;

                view.baseObjects[i].sizeDelta = GetBaseSize(bases[i]) * minimapSizeScale;
                view.baseObjects[i].anchoredPosition = new Vector2(pos.x, pos.z);
            }


            playerObjectVO = allObjects.Find(t => t.type == MinimapObjectType.Player);
            view.playerIcon.sprite = playerObjectVO.minimapIcon;
            view.playerIcon.rectTransform.sizeDelta = new Vector2(playerObjectVO.minimapIconSize, playerObjectVO.minimapIconSize) * minimapIconScale;

        }

        private void Update()
        {
            if (playerObjectVO == null) return;

            DrawPlayer();
            CenterPlayer();
            
            DrawEnemies();
        }

        private void DrawPlayer()
        {
            var pos = playerObjectVO.minimapObject.transform.position * minimapSizeScale;
            var rotation = -playerObjectVO.minimapObject.transform.eulerAngles.y;

            view.playerIcon.rectTransform.anchoredPosition = new Vector2(pos.x, pos.z);
            view.playerIcon.rectTransform.eulerAngles = new Vector3(0, 0, rotation);
        }

        private void CenterPlayer()
        {
            var playerIconPos = view.playerIcon.rectTransform.anchoredPosition;
            view.minimapContainer.anchoredPosition = -playerIconPos;
        }

        private void DrawEnemies()
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                var pos = enemyList[i].minimapObject.transform.position * minimapSizeScale;
                var rotation = -enemyList[i].minimapObject.transform.eulerAngles.y;

                view.enemyIcons[i].rectTransform.anchoredPosition = new Vector2(pos.x, pos.z);
                view.enemyIcons[i].rectTransform.eulerAngles = new Vector3(0, 0, rotation);
            }
        }

        private void OnUpdateEnemies()
        {
            var allObjects = MinimapModel.MinimapObjectVos;
            var enemies = allObjects.FindAll(t => t.type == MinimapObjectType.Enemy);
            enemyList = enemies;
            
            foreach (var enemyIcon in view.enemyIcons)
            {
                enemyIcon.gameObject.gameObject.SetActive(false);
            }
            
            for (int i = 0; i < enemies.Count; i++)
            {
                view.enemyIcons[i].gameObject.SetActive(true);
                view.enemyIcons[i].sprite = enemies[i].minimapIcon;
                view.enemyIcons[i].rectTransform.sizeDelta = new Vector2(enemies[i].minimapIconSize, enemies[i].minimapIconSize) * minimapIconScale;
            }
        }

        private Vector2 GetBaseSize(MinimapObjectVO minimapObjectVo)
        {
            var baseCollider = minimapObjectVo.boxCollider;

            var width = baseCollider.bounds.size.x;
            var length = baseCollider.bounds.size.z;
            
            return new Vector2(width, length);
        }

        public override void OnRegister()
        {
            base.OnRegister();
            
            enemyList = new List<MinimapObjectVO>();
			CreateBases();
            MinimapModel.EnemyUpdatedEvent += OnUpdateEnemies;
        }

        public override void OnRemove()
        {
            base.OnRemove();
            MinimapModel.EnemyUpdatedEvent -= OnUpdateEnemies;

        }
    }
}
