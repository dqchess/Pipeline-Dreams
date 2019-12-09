﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PipelineDreams {

    public class TaskManager : MonoBehaviour {

        PlayerInputBroadcaster PC;
        public float Clock { get; private set; } = 0;
        float AccumulatedClock = 0;


        /// <summary>
        /// These tasks are done sequentially.
        /// </summary>
        List<IClockTask> SequentialTasks = new List<IClockTask>();
        /// <summary>
        /// This is a list of undone tasks.
        /// </summary>
        List<IClockTask> UndoneTasks = new List<IClockTask>();
        public event Action OnTaskEnd;

        public event Action<float> OnClockModified;
        bool[] TaskEndFlag;
        // Start is called before the first frame update
        private void Awake() {
            PC = FindObjectOfType<PlayerInputBroadcaster>();
        }
        private void Start() {
            AddTime(0);//Alert all event receivers.
        }
        public void AddTime(float time) {
            Clock += time;

            OnClockModified?.Invoke(Clock);
            StartCoroutine(RunTasks());
        }
        public void AddSequentialTask(IClockTask f) {
            UndoneTasks.Add(f);

        }

        IEnumerator RunTasks() {
            PC.SetPlayerInputEnabled(PlayerInputFlag.TASKMANAGER, false);

            for (int i = 0; i < UndoneTasks.Count; i++) {
                if (UndoneTasks[i].StartClock <= Clock) {
                    SequentialTasks.Add(UndoneTasks[i]);
                    UndoneTasks.RemoveAt(i);
                    i--;
                }
            }

            Debug.Log("Clock : " + Clock);
            while (SequentialTasks.Count != 0) {


                SequentialTasks = SequentialTasks.OrderBy((x) => x.Priority).OrderBy((x) => x.StartClock).ToList();
                var t = SequentialTasks[0];
                var task = t.Run();
                if (task != null)
                    yield return task;

                //Debug.Log("Task : " + t.ToString() + ", Priority : " + t.Priority + ", Time : " + t.StartClock + ", Frame : " + Time.frameCount + ", RealTime : " + Time.unscaledTime);
                SequentialTasks.Remove(t);
                OnTaskEnd?.Invoke();

                for (int i = 0; i < UndoneTasks.Count; i++) {
                    if (UndoneTasks[i].StartClock <= Clock) {
                        SequentialTasks.Add(UndoneTasks[i]);
                        UndoneTasks.RemoveAt(i);
                        i--;
                    }
                }


            }

            //ImmediateTasks.Clear();
            SequentialTasks.Clear();
            PC.SetPlayerInputEnabled(PlayerInputFlag.TASKMANAGER, true);
        }
        IEnumerator RunTaskAndRaiseFlag(IClockTask f, int i) {
            yield return f.Run();
            TaskEndFlag[i] = true;
        }

    }
    public interface IClockTask {
        int Priority { get; }
        float StartClock { get; }
        IEnumerator Run();
    }
}