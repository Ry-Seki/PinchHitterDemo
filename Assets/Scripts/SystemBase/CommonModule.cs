using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameConst;

public class CommonModule {
    /// <summary>
    /// 配列が空か判定
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static bool IsEmpty<T>(T[] array) {
        return array == null || array.Length <= 0;
    }
    /// <summary>
    /// 配列が空か判別 (二次元配列)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static bool IsEmpty<T>(T[][] array) {
        if (array == null || array.Length == 0)
            return true;

        foreach (var inner in array) {
            if (inner != null && inner.Length > 0)
                return false;
        }
        return true;
    }
    /// <summary>
    /// リストが空か判定
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static bool IsEmpty<T>(List<T> list) {
        return list == null || list.Count <= 0;
    }
    /// <summary>
    /// 配列に対して有効な要素数か判定
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static bool IsEnableIndex<T>(T[] array, int index) {
        if (IsEmpty(array))
            return false;

        return index >= 0 && array.Length > index;
    }
    /// <summary>
    /// リストに対して有効な要素数か判定
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static bool IsEnableIndex<T>(List<T> list, int index) {
        if (IsEmpty(list))
            return false;

        return index >= 0 && list.Count > index;
    }
    /// <summary>
    /// リストの初期化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="capacity"></param>
    public static void InitializeList<T>(ref List<T> list, int capacity = -1) {
        if (list == null) {
            if (capacity < 1) {
                list = new List<T>();
            } else {
                list = new List<T>(capacity);
            }
        } else {
            if (list.Capacity < capacity)
                list.Capacity = capacity;

            list.Clear();
        }
    }
    /// <summary>
    /// 複数のタスクの終了を待つ
    /// </summary>
    /// <param name="taskList"></param>
    /// <returns></returns>
    public static async UniTask WaitTask(List<UniTask> taskList) {
        if (taskList == null) return;

        //リストが空になるまで待つ
        while (true) {
            //末尾から処理
            for (int i = taskList.Count - 1; i >= 0; i--) {
                //タスクが終了していたらリストから抜く
                if(!taskList[i].Status.IsCompleted()) continue;

                taskList.RemoveAt(i);
            }
            //リストが空ならループを抜ける
            if(IsEmpty(taskList)) break;

            await UniTask.DelayFrame(1);
        }
    }
    /// <summary>
    /// 百分率に変換（数値が小さいほど百分率が大きくなる）
    /// </summary>
    /// <param name="setValue"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    public static float ReversePercentageScaling(float setValue, float minValue, float maxValue) {
        return ((minValue - Mathf.Abs(setValue)) / (minValue - maxValue)) * MAX_PERCENTAGE;
    }

    public static float ReverseNormScaling(float setValue, float minValue, float maxValue) {
        return MIN_NORM + ((minValue - Mathf.Abs(setValue)) / (minValue - maxValue)) * (MAX_NORM - MIN_NORM);
    }
}
