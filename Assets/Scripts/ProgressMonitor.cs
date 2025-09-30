using System;
using UnityEngine;

public class ProgressMonitor
{
    // 当前进度 (0.0 - 1.0)
    private float _progress;

    // 当前提示信息
    private string _message;

    // 进度变更事件
    public event Action<float> OnProgressChanged;

    // 进度属性
    public float Progress
    {
        get => _progress;
        set
        {
            if (Math.Abs(_progress - value) > 0.0001f)
            {
                
                _progress = Mathf.Clamp01(value);
                NotifyProgressChanged();
            }
        }
    }

    public void AddProgress(float progress)
    {
        Progress += progress;
    }

    // 通知进度变更
    private void NotifyProgressChanged()
    {
        OnProgressChanged?.Invoke(_progress);
    }
}