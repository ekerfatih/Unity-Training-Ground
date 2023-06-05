using UnityEngine;

public class VoxelStencilCircle : VoxelStencil {
	
    private int sqrRadius;
	
    public override void Initialize (bool fillType, int radius) {
        base.Initialize (fillType, radius);
        sqrRadius = radius * radius;
    }
	
    public override bool Apply (int x, int y, bool voxel) {
        x -= _centerX;
        y -= _centerY;
        if (x * x + y * y <= sqrRadius) {
            return _fillType;
        }
        return voxel;
    }
}