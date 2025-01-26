using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using R3;
using UnityEngine;

namespace PCoreAdapters.Gameplay
{
    public class ChaserComputeController : MonoBehaviour
    {
        public ComputeShader ChaserCompute;
        public ChaserComputeConfig Config;
        public Transform Target;

        private List<TempEnemy> _npcs;
        private int _numNPC;

        private int _gridX, _gridY;

        // Буферы
        private ComputeBuffer _npcInputBuffer;
        private ComputeBuffer _npcOutputBuffer;
        private ComputeBuffer _gridBuffer;
        private ComputeBuffer _indicesBuffer;

        private NPCData[] _npcDataArray;

        private int _kClearGrid;
        private int _kFillGrid;
        private int _kChaser;

        private const int MaxInCell = 64;
        
        private CompositeDisposable _disposables = new CompositeDisposable();

        [StructLayout(LayoutKind.Sequential)]
        struct NPCData
        {
            public Vector2 Position;
            public Vector2 Up;
            public Vector2 Direction;
            public uint IsSeparating;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct CellData
        {
            public uint Count;
        }

        void Start()
        {
            _npcs = new List<TempEnemy>(transform.childCount);

            for (int childIndex = 0; childIndex < transform.childCount; childIndex++)
            {
                var enemy = transform.GetChild(childIndex).GetComponent<TempEnemy>();

                // enemy.isDead
                //     .Where(isDead => isDead)
                //     .Subscribe(_ =>
                //     {
                //         enemy.transform.position = new Vector3(99999, 99999, 99999);
                //     })
                //     .AddTo(_disposables);
                
                _npcs.Add(enemy);
            }

            _numNPC = _npcs.Count;

            // Получаем ядра
            _kClearGrid = ChaserCompute.FindKernel("ClearGridKernel");
            _kFillGrid = ChaserCompute.FindKernel("FillGridKernel");
            _kChaser = ChaserCompute.FindKernel("ChaserKernel");

            // Рассчитываем размер сетки
            Vector2 size = Config.DomainMax - Config.DomainMin;
            _gridX = Mathf.CeilToInt(size.x / Config.CellSize);
            _gridY = Mathf.CeilToInt(size.y / Config.CellSize);
            int cellCount = _gridX * _gridY;

            _npcDataArray = new NPCData[_numNPC];

            _npcInputBuffer = new ComputeBuffer(_numNPC, Marshal.SizeOf(typeof(NPCData)), ComputeBufferType.Structured);
            _npcOutputBuffer = new ComputeBuffer(_numNPC, Marshal.SizeOf(typeof(NPCData)), ComputeBufferType.Structured);
            _gridBuffer = new ComputeBuffer(cellCount, Marshal.SizeOf(typeof(CellData)), ComputeBufferType.Structured);
            _indicesBuffer = new ComputeBuffer(cellCount * MaxInCell, sizeof(uint), ComputeBufferType.Structured);

            ChaserCompute.SetBuffer(_kClearGrid, "g_Grid", _gridBuffer);
            ChaserCompute.SetBuffer(_kFillGrid, "g_Grid", _gridBuffer);
            ChaserCompute.SetBuffer(_kChaser, "g_Grid", _gridBuffer);

            ChaserCompute.SetBuffer(_kFillGrid, "npcInput", _npcInputBuffer);
            ChaserCompute.SetBuffer(_kFillGrid, "npcOutput", _npcOutputBuffer);
            ChaserCompute.SetBuffer(_kChaser, "npcInput", _npcInputBuffer);
            ChaserCompute.SetBuffer(_kChaser, "npcOutput", _npcOutputBuffer);

            ChaserCompute.SetBuffer(_kClearGrid, "g_Indices", _indicesBuffer);
            ChaserCompute.SetBuffer(_kFillGrid, "g_Indices", _indicesBuffer);
            ChaserCompute.SetBuffer(_kChaser, "g_Indices", _indicesBuffer);

            ChaserCompute.SetFloats("domainMin", Config.DomainMin.x, Config.DomainMin.y);
            ChaserCompute.SetFloats("domainMax", Config.DomainMax.x, Config.DomainMax.y);
            ChaserCompute.SetFloat("cellSize", Config.CellSize);
            ChaserCompute.SetInts("gridResolution", _gridX, _gridY);
        }

        void Update()
        {
            for (int i = 0; i < _numNPC; i++)
            {
                Vector2 pos2D = _npcs[i].transform.position;
                Vector2 up2D = _npcs[i].transform.up;

                _npcDataArray[i].Position = pos2D;
                _npcDataArray[i].Up = up2D;
                _npcDataArray[i].Direction = Vector2.zero;
                _npcDataArray[i].IsSeparating = Convert.ToUInt32(_npcs[i].IsSeparating);
            }

            _npcInputBuffer.SetData(_npcDataArray);

            ChaserCompute.SetFloat("separationRadius", Config.SeparationRadius);
            ChaserCompute.SetFloat("separationStrength", Config.SeparationStrength);
            ChaserCompute.SetFloat("baseSpeed", Config.BaseSpeed);
            ChaserCompute.SetFloat("minArriveDistance", Config.MinArriveDistance);
            ChaserCompute.SetFloat("arriveRange", Config.ArriveRange);
            ChaserCompute.SetFloat("maxChaseDistance", Config.MaxChaseDistance);
            ChaserCompute.SetFloat("minDistanceToSlow", Config.MinDistanceToSlow);
            ChaserCompute.SetFloats("targetPosition", Target.position.x, Target.position.y);
            ChaserCompute.SetInt("g_NumNPCs", _numNPC);

            int cellCount = _gridX * _gridY;
            int threadsGrid = Mathf.CeilToInt(cellCount / 64f);
            ChaserCompute.Dispatch(_kClearGrid, threadsGrid, 1, 1);

            int threadsNPC = Mathf.CeilToInt(_numNPC / 64f);
            ChaserCompute.Dispatch(_kFillGrid, threadsNPC, 1, 1);
            ChaserCompute.Dispatch(_kChaser, threadsNPC, 1, 1);

            _npcOutputBuffer.GetData(_npcDataArray);

            for (int i = 0; i < _numNPC; i++)
            {
                Vector2 dir = _npcDataArray[i].Direction;
                _npcs[i].Morph.Move(dir);

                Vector2 newUp = _npcDataArray[i].Up;
                var separation = _npcDataArray[i].IsSeparating;
                _npcs[i].IsSeparating = separation > 0;
                if (newUp.sqrMagnitude > 1e-5f)
                {
                    _npcs[i].transform.up = new Vector3(newUp.x, newUp.y, 0);
                }
            }
        }

        void OnDestroy()
        {
            if (_npcInputBuffer != null) _npcInputBuffer.Release();
            if (_npcOutputBuffer != null) _npcOutputBuffer.Release();
            if (_gridBuffer != null) _gridBuffer.Release();
            if (_indicesBuffer != null) _indicesBuffer.Release();
            _disposables.Dispose();
        }
    }
}
