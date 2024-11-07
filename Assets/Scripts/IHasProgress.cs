using System;
using UnityEngine;

public interface IHasProgress
{
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs
    {
        public float progressNormalized;
    }
}
