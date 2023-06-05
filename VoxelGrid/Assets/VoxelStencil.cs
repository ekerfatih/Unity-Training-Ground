using UnityEngine;

public class VoxelStencil {
    protected bool _fillType;
    protected int _centerX, _centerY, _radius;
    public int XStart {
        get {
            return _centerX - _radius;
        }
    }

    public int XEnd {
        get {
            return _centerX + _radius;
        }
    }

    public int YStart {
        get {
            return _centerY - _radius;
        }
    }

    public int YEnd {
        get {
            return _centerY + _radius;
        }
    }
    public virtual void Initialize(bool fillType, int radius) {
        _fillType = fillType;
        _radius = radius;
    }
    public virtual void SetCenter(int x, int y) {
        _centerX = x;
        _centerY = y;
    }
    public virtual bool Apply(int x, int y,bool voxel) {
        return _fillType;
    }
    
}